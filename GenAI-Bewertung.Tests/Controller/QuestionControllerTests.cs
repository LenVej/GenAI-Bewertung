using System.Security.Claims;
using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Enums;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace GenAI_Bewertung.Tests.Controller
{
    public class QuestionsControllerExtendedTests
    {
        private readonly Mock<IQuestionRepository> _mockRepo;
        private readonly QuestionService _service;
        private readonly QuestionsController _controller;

        public QuestionsControllerExtendedTests()
        {
            _mockRepo = new Mock<IQuestionRepository>();
            _service = new QuestionService(_mockRepo.Object);
            _controller = new QuestionsController(_service);
        }

        private void SetUser(int userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task PostQuestion_ReturnsOk_WhenValid()
        {
            SetUser(1);

            var dto = new CreateQuestionDto
            {
                QuestionText = "Testfrage?",
                QuestionType = QuestionType.OneWord,
                Subject = "Test",
                ExpectedAnswer = "Antwort"
            };

            var result = await _controller.PostQuestion(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnDto = Assert.IsType<QuestionDto>(okResult.Value);
            Assert.Equal("Testfrage?", returnDto.QuestionText);
        }

        [Fact]
        public async Task PostQuestion_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };

            var dto = new CreateQuestionDto
            {
                QuestionText = "Testfrage?",
                QuestionType = QuestionType.OneWord,
                Subject = "Test",
                ExpectedAnswer = "Antwort"
            };

            var result = await _controller.PostQuestion(dto);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }


        /*[Fact]
        public async Task PutQuestion_ReturnsNoContent_WhenSuccessful()
        {
            var question = new OneWordQuestion
            {
                QuestionId = 1,
                QuestionText = "Test?",
                QuestionType = QuestionType.OneWord,
                Subject = "Test",
                CreatedBy = 1,
                ExpectedAnswer = "Antwort"
            };

            _mockRepo.Setup(r => r.QuestionExistsAsync(1)).ReturnsAsync(true);

            var result = await _controller.PutQuestion(1, question);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutQuestion_ReturnsBadRequest_WhenIdMismatch()
        {
            var result = await _controller.PutQuestion(1, new OneWordQuestion { QuestionId = 2 });
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
        {
            var question = new OneWordQuestion { QuestionId = 5 };
            _mockRepo.Setup(r => r.QuestionExistsAsync(5)).ReturnsAsync(false);

            var result = await _controller.PutQuestion(5, question);
            Assert.IsType<NotFoundResult>(result);
        }*/

        [Fact]
        public async Task DeleteQuestion_ReturnsNoContent_WhenFound()
        {
            var q = new OneWordQuestion { QuestionId = 1 };
            _mockRepo.Setup(r => r.GetQuestionByIdAsync(1)).ReturnsAsync(q);

            var result = await _controller.DeleteQuestion(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteQuestion_ReturnsNotFound_WhenMissing()
        {
            _mockRepo.Setup(r => r.GetQuestionByIdAsync(99)).ReturnsAsync((Question)null);
            var result = await _controller.DeleteQuestion(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetMyQuestions_ReturnsOk_WithUserQuestions()
        {
            SetUser(2);

            var list = new List<Question>
            {
                new OneWordQuestion { QuestionId = 2, CreatedBy = 2, QuestionText = "Hauptstadt?", ExpectedAnswer = "Berlin" }
            };

            _mockRepo.Setup(r => r.GetQuestionsByUserIdAsync(2)).ReturnsAsync(list);

            var result = await _controller.GetMyQuestions();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var questions = Assert.IsAssignableFrom<IEnumerable<QuestionDto>>(okResult.Value);
            Assert.Single(questions);
        }
    }
}
