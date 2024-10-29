using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.Data;
using GenAI_Bewertung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Tests.Controller
{
    public class QuestionsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            // In-Memory-Datenbank einrichten
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            // Sample-Daten hinzufügen
            _context.Questions.AddRange(
                new Question { Id = 1, Text = "What is AI?" },
                new Question { Id = 2, Text = "Define machine learning." }
            );
            _context.SaveChanges();

            // Controller initialisieren
            _controller = new QuestionsController(_context);
        }

        [Fact]
        public async Task GetQuestions_ReturnsAllQuestions()
        {
            // Act
            var result = await _controller.GetQuestions();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Question>>>(result);
            var questions = Assert.IsType<List<Question>>(actionResult.Value);
            Assert.Equal(2, questions.Count);
        }

        [Fact]
        public async Task GetQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
        {
            // Act
            var result = await _controller.GetQuestion(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostQuestion_CreatesNewQuestion()
        {
            // Arrange
            var newQuestion = new Question { Text = "What is deep learning?" };

            // Act
            var result = await _controller.PostQuestion(newQuestion);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Question>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var question = Assert.IsType<Question>(createdResult.Value);
            Assert.Equal("What is deep learning?", question.Text);
        }

        [Fact]
        public async Task PutQuestion_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var updateQuestion = new Question { Id = 1, Text = "Updated Question" };

            // Act
            var result = await _controller.PutQuestion(999, updateQuestion);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteQuestion_RemovesQuestion_WhenQuestionExists()
        {
            // Act
            var result = await _controller.DeleteQuestion(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Questions.FindAsync(1));
        }
    }
}
