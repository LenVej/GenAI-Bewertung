using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenAI_Bewertung.Enums;
using GenAI_Bewertung.Repositories;
using Xunit;

namespace GenAI_Bewertung.Tests.Controller
{
    public class QuestionsControllerTests
    {
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            // Set up mock repository
            Mock<IQuestionRepository> mockRepository = new();

            // Sample data: konkrete abgeleitete Klassen verwenden
            List<Question> sampleQuestions = new()
            {
                new MultipleChoiceQuestion
                {
                    QuestionId = 1,
                    QuestionText = "Welche Farben sind Primärfarben?",
                    QuestionType = QuestionType.MultipleChoice,
                    Subject = "Kunst",
                    CreatedBy = 1,
                    Choices = new() { "Rot", "Grün", "Blau" },
                    CorrectIndices = new() { 0, 2 }
                },
                new OneWordQuestion
                {
                    QuestionId = 2,
                    QuestionText = "Hauptstadt von Frankreich?",
                    QuestionType = QuestionType.OneWord,
                    Subject = "Geographie",
                    CreatedBy = 2,
                    ExpectedAnswer = "Paris"
                }
            };

            // Mock Verhalten
            mockRepository.Setup(r => r.GetQuestionsAsync()).ReturnsAsync(sampleQuestions);
            mockRepository.Setup(r => r.GetQuestionByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => sampleQuestions.FirstOrDefault(q => q.QuestionId == id));
            mockRepository.Setup(r => r.QuestionExistsAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => sampleQuestions.Any(q => q.QuestionId == id));

            // Service + Controller
            var mockService = new QuestionService(mockRepository.Object);
            _controller = new QuestionsController(mockService);
        }

        [Fact]
        public async Task GetQuestions_ReturnsOkResult_WithListOfQuestions()
        {
            var result = await _controller.GetQuestions();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var questions = Assert.IsAssignableFrom<List<Question>>(okResult.Value);
            Assert.Equal(2, questions.Count);
        }

        [Fact]
        public async Task GetQuestion_ReturnsNotFound_ForInvalidId()
        {
            var result = await _controller.GetQuestion(999); // Non-existing ID

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetQuestion_ReturnsOkResult_WithQuestion_ForValidId()
        {
            var result = await _controller.GetQuestion(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var question = Assert.IsType<MultipleChoiceQuestion>(okResult.Value); // Typ beachten!
            Assert.Equal(1, question.QuestionId);
            Assert.Equal("Welche Farben sind Primärfarben?", question.QuestionText);
        }
    }
}
