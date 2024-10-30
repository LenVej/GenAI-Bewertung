using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var questions = await _service.GetAllQuestionsAsync();
            return Ok(questions); // Ensure this returns Ok with the list of questions
        }

        // GET: api/questions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _service.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound(); // Return NotFound if the question is not found
            }

            return Ok(question); // Return Ok with the found question
        }

        // POST: api/questions
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            await _service.AddQuestionAsync(question);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.QuestionId }, question); // Return Created response
        }

        // PUT: api/questions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest(); // Return BadRequest if IDs do not match
            }

            if (!await _service.QuestionExistsAsync(id))
            {
                return NotFound(); // Return NotFound if the question does not exist
            }

            await _service.UpdateQuestionAsync(question);
            return NoContent(); // Return NoContent on successful update
        }

        // DELETE: api/questions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _service.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound(); // Return NotFound if the question does not exist
            }

            await _service.DeleteQuestionAsync(question);
            return NoContent(); // Return NoContent on successful deletion
        }
    }
}
