using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Service
{
    public class ExamServiceTests
    {
        private readonly Mock<IExamRepository> _mockRepo;
        private readonly ExamService _service;

        public ExamServiceTests()
        {
            _mockRepo = new Mock<IExamRepository>();
            _service = new ExamService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllExams()
        {
            var exams = new List<Exam>
            {
                new Exam { ExamId = 1, Title = "Test 1" },
                new Exam { ExamId = 2, Title = "Test 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(exams);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Title == "Test 1");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsExam_WhenExists()
        {
            var exam = new Exam { ExamId = 1, Title = "Mathe" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Mathe", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Exam)null!);

            var result = await _service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserIdAsync_ReturnsExamsForUser()
        {
            var exams = new List<Exam>
            {
                new Exam { ExamId = 1, CreatedBy = 42 },
                new Exam { ExamId = 2, CreatedBy = 99 }
            };

            _mockRepo.Setup(r => r.GetByUserIdAsync(42)).ReturnsAsync(exams.Where(e => e.CreatedBy == 42));

            var result = await _service.GetByUserIdAsync(42);

            Assert.Single(result);
            Assert.Equal(42, result.First().CreatedBy);
        }

        [Fact]
        public async Task AddAsync_CallsRepository()
        {
            var exam = new Exam { Title = "Physik" };

            await _service.AddAsync(exam);

            _mockRepo.Verify(r => r.AddAsync(exam), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            var exam = new Exam { ExamId = 3 };

            await _service.DeleteAsync(exam);

            _mockRepo.Verify(r => r.DeleteAsync(exam), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var exam = new Exam { ExamId = 5, Title = "Bio" };

            await _service.UpdateAsync(exam);

            _mockRepo.Verify(r => r.UpdateAsync(exam), Times.Once);
        }
    }
}
