using System.Security.Claims;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Mappers;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenAI_Bewertung.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly ExamService _service;

    public ExamsController(ExamService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetAll()
    {
        var exams = await _service.GetAllAsync();
        return Ok(exams.Select(ExamMapper.ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExamDto>> GetById(int id)
    {
        var exam = await _service.GetByIdAsync(id);
        if (exam == null) return NotFound();

        return Ok(ExamMapper.ToDto(exam));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateExamDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);
        var exam = ExamMapper.FromCreateDto(dto, userId);

        await _service.AddAsync(exam);
        return Ok(ExamMapper.ToDto(exam));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var exam = await _service.GetByIdAsync(id);
        if (exam == null) return NotFound();

        await _service.DeleteAsync(exam);
        return NoContent();
    }

    [HttpGet("by-user")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetMyExams()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);
        var exams = await _service.GetByUserIdAsync(userId);
        return Ok(exams.Select(ExamMapper.ToDto));
    }
}
