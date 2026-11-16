using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace SleepyDuck.Api.Controllers;

/// <summary>The one account allowed to sign in, read from AUTH_EMAIL and AUTH_PASSWORD.</summary>
public record AuthCredentials(string Email, string Password);

public record LoginRequest(string? Email, string? Password);

public record SessionDto(string Email);

[ApiController]
[Route("api/auth")]
public class AuthController(AuthCredentials credentials) : ControllerBase
{
    public const string LoginRateLimit = "login";

    [AllowAnonymous]
    [EnableRateLimiting(LoginRateLimit)]
    [HttpPost("login")]
    public async Task<ActionResult<SessionDto>> Login(LoginRequest request)
    {
        var email = request.Email?.Trim() ?? "";

        // Check both values every time, so the response doesn't reveal which one was wrong.
        var emailMatches = SecureEquals(email.ToLowerInvariant(), credentials.Email.ToLowerInvariant());
        var passwordMatches = SecureEquals(request.Password ?? "", credentials.Password);
        if (!emailMatches || !passwordMatches)
        {
            return Unauthorized(new { message = "Wrong email or password." });
        }

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Email, credentials.Email)],
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });

        return new SessionDto(credentials.Email);
    }

    [HttpGet("me")]
    public SessionDto Me() => new(User.FindFirstValue(ClaimTypes.Email) ?? credentials.Email);

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    private static bool SecureEquals(string a, string b) =>
        CryptographicOperations.FixedTimeEquals(
            SHA256.HashData(Encoding.UTF8.GetBytes(a)),
            SHA256.HashData(Encoding.UTF8.GetBytes(b)));
}
