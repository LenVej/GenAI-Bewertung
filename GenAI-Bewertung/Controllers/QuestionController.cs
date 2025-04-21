using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Enums;
using Microsoft.AspNetCore.Authorization;
using GenAI_Bewertung.Mappers;


namespace GenAI_Bewertung.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionService _service;

        public QuestionsController(QuestionService service)
        {
            _service = service;
        }

        // GET: api/questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            var questions = await _service.GetAllQuestionsAsync();
            var mapped = questions.Select(QuestionMapper.ToDto);
            return Ok(mapped);
        }

        // GET: api/questions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _service.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // POST: api/questions
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostQuestion([FromBody] CreateQuestionDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Kein Benutzer-Token gefunden.");

            var userId = int.Parse(userIdClaim.Value);
            var question = QuestionMapper.FromCreateDto(dto, userId);

            await _service.AddQuestionAsync(question);
            return Ok(QuestionMapper.ToDto(question));
        }



        // PUT: api/questions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest();
            }

            if (!await _service.QuestionExistsAsync(id))
            {
                return NotFound();
            }

            await _service.UpdateQuestionAsync(question);
            return NoContent();
        }

        // DELETE: api/questions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _service.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            await _service.DeleteQuestionAsync(question);
            return NoContent();
        }

        [HttpGet("by-user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetMyQuestions()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            var questions = await _service.GetQuestionsByUserIdAsync(userId);
            return Ok(questions.Select(QuestionMapper.ToDto));
        }
    }
}