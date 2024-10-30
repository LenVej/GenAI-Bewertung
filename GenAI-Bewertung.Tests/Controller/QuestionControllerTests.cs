using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // Sample data
            List<Question> sampleQuestions = new()
            {
                new Question { QuestionId = 1, QuestionText = "Sample Question 1", QuestionType = "MultipleChoice", Subject = "Math", CreatedBy = 1 },
                new Question { QuestionId = 2, QuestionText = "Sample Question 2", QuestionType = "OneWord", Subject = "Science", CreatedBy = 2 }
            };

            // Configure mock behavior
            mockRepository.Setup(r => r.GetQuestionsAsync()).ReturnsAsync(sampleQuestions);
            mockRepository.Setup(r => r.GetQuestionByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => sampleQuestions.FirstOrDefault(q => q.QuestionId == id));
            mockRepository.Setup(r => r.QuestionExistsAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => sampleQuestions.Any(q => q.QuestionId == id));

            // Initialize service and controller with mock repository
            var mockService = new QuestionService(mockRepository.Object);
            _controller = new QuestionsController(mockService);
        }

        [Fact]
        public async Task GetQuestions_ReturnsOkResult_WithListOfQuestions()
        {
            // Act
            var result = await _controller.GetQuestions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var questions = Assert.IsType<List<Question>>(okResult.Value);
            Assert.Equal(2, questions.Count);
        }

        [Fact]
        public async Task GetQuestion_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = await _controller.GetQuestion(999); // Non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetQuestion_ReturnsOkResult_WithQuestion_ForValidId()
        {
            // Act
            var result = await _controller.GetQuestion(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var question = Assert.IsType<Question>(okResult.Value);
            Assert.Equal(1, question.QuestionId);
            Assert.Equal("Sample Question 1", question.QuestionText);
        }

        // Additional tests for PostQuestion, PutQuestion, and DeleteQuestion can be added similarly
    }
}
