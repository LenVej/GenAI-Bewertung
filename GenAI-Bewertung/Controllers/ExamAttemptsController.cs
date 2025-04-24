using System.Security.Claims;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Mappers;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenAI_Bewertung.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamAttemptsController : ControllerBase
{
    private readonly ExamAttemptService _service;

    public ExamAttemptsController(ExamAttemptService service)
    {
        _service = service;
    }

    [HttpPost("start")]
    [Authorize]
    public async Task<ActionResult<StartedExamAttemptDto>> StartExam([FromBody] StartExamAttemptDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var attempt = await _service.StartAttemptAsync(dto.ExamId, userId);

        if (attempt == null) return NotFound("Prüfung nicht gefunden");

        return Ok(ExamAttemptMapper.ToStartedDto(attempt));
    }

    [HttpPost("submit")]
    [Authorize]
    public async Task<ActionResult<ExamAttemptResultDto>> Submit([FromBody] SubmitExamAttemptDto dto)
    {
        var result = await _service.SubmitAttemptAsync(dto);
        if (result == null) return BadRequest("Ungültiger Versuch oder bereits eingereicht.");

        return Ok(result);
    }

    [HttpGet("{id}/result")]
    [Authorize]
    public async Task<ActionResult<ExamAttemptResultDto>> GetResult(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.GetResultAsync(id, userId);
        if (result == null) return NotFound();

        return Ok(result);
    }
}