using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SleepyDuck.Api;
using SleepyDuck.Api.Controllers;
using SleepyDuck.Api.Data;
using SleepyDuck.Api.Services;

// Database and login credentials live in the repo-root .env (see .env.example), not in appsettings.
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

string Required(string key) =>
    builder.Configuration[key] is { Length: > 0 } value
        ? value
        : throw new InvalidOperationException($"{key} is not set. Copy .env.example to .env and fill it in.");

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? new NpgsqlConnectionStringBuilder
    {
        Host = builder.Configuration["POSTGRES_HOST"] ?? "localhost",
        Port = int.Parse(builder.Configuration["POSTGRES_PORT"] ?? "5432"),
        Database = Required("POSTGRES_DB"),
        Username = Required("POSTGRES_USER"),
        Password = Required("POSTGRES_PASSWORD"),
    }.ConnectionString;

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<StudyService>();

// Single-account sign-in: the credentials come from .env and the session is an HttpOnly cookie.
builder.Services.AddSingleton(new AuthCredentials(Required("AUTH_EMAIL").Trim(), Required("AUTH_PASSWORD")));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "sleepyduck_session";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;

        // This is an API: answer with status codes instead of redirecting to a login page.
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

// Every endpoint needs a signed-in user unless it is marked [AllowAnonymous].
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

// Slow down password guessing: 5 sign-in attempts per minute per client address.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy(AuthController.LoginRateLimit, context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
