using System.Security.Claims;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenAI_Bewertung.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly StatsService _service;

    public StatsController(StatsService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("profile-stats")]
    public async Task<IActionResult> GetUserStats()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var stats = await _service.GetUserStatsAsync(int.Parse(userIdStr));
        return Ok(stats);
    }
}