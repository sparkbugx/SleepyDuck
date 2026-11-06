using Microsoft.AspNetCore.Mvc;
using SleepyDuck.Api.Dtos;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(StudyService study) : ControllerBase
{
    [HttpGet("stats")]
    public Task<DashboardStatsDto> GetStats(CancellationToken ct) => study.GetDashboardStatsAsync(ct);
}
