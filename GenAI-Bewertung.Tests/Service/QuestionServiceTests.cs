using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Enums;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Service
{
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _mockRepo;
        private readonly QuestionService _service;

        public QuestionServiceTests()
        {
            _mockRepo = new Mock<IQuestionRepository>();
            _service = new QuestionService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllQuestionsAsync_ReturnsAllQuestions()
        {
            // Arrange
            var questions = new List<Question>
            {
                new OneWordQuestion { QuestionId = 1, QuestionText = "Hauptstadt?", ExpectedAnswer = "Berlin" },
                new MultipleChoiceQuestion { QuestionId = 2, QuestionText = "Farben?", Choices = new() { "Rot", "Blau" }, CorrectIndices = new() { 0 } }
            };
            _mockRepo.Setup(r => r.GetQuestionsAsync()).ReturnsAsync(questions);

            // Act
            var result = await _service.GetAllQuestionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetQuestionByIdAsync_ReturnsCorrectQuestion()
        {
            var question = new OneWordQuestion { QuestionId = 1, QuestionText = "Test", ExpectedAnswer = "Haus" };
            _mockRepo.Setup(r => r.GetQuestionByIdAsync(1)).ReturnsAsync(question);

            var result = await _service.GetQuestionByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Haus", ((OneWordQuestion)result!).ExpectedAnswer);
        }

        [Fact]
        public async Task AddQuestionAsync_CallsRepository()
        {
            var question = new OneWordQuestion { QuestionText = "Test", ExpectedAnswer = "Antwort" };

            await _service.AddQuestionAsync(question);

            _mockRepo.Verify(r => r.AddQuestionAsync(question), Times.Once);
        }

        [Fact]
        public async Task DeleteQuestionAsync_CallsRepository()
        {
            var question = new OneWordQuestion { QuestionId = 5 };
            await _service.DeleteQuestionAsync(question);
            _mockRepo.Verify(r => r.DeleteQuestionAsync(question), Times.Once);
        }

        [Fact]
        public async Task GetQuestionsByUserIdAsync_FiltersByUser()
        {
            var questions = new List<Question>
            {
                new OneWordQuestion { QuestionId = 1, CreatedBy = 1 },
                new OneWordQuestion { QuestionId = 2, CreatedBy = 2 }
            };

            _mockRepo.Setup(r => r.GetQuestionsByUserIdAsync(1)).ReturnsAsync(questions.Where(q => q.CreatedBy == 1));

            var result = await _service.GetQuestionsByUserIdAsync(1);

            Assert.Single(result);
            Assert.Equal(1, result.First().QuestionId);
        }
    }
}
