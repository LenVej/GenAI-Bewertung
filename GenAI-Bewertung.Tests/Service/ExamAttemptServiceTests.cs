using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Service
{
    public class ExamAttemptServiceTests
    {
        private readonly Mock<IExamAttemptRepository> _mockRepo;
        private readonly ExamAttemptService _service;

        public ExamAttemptServiceTests()
        {
            _mockRepo = new Mock<IExamAttemptRepository>();
            _service = new ExamAttemptService(_mockRepo.Object);
        }

        [Fact]
        public async Task StartAttemptAsync_ReturnsAttempt_WhenExamExists()
        {
            var exam = new Exam { ExamId = 1, Title = "Test" };
            var attempt = new ExamAttempt { ExamAttemptId = 100, ExamId = 1, Exam = exam };

            _mockRepo.Setup(r => r.CreateAttemptAsync(1, 42)).ReturnsAsync(attempt);

            var result = await _service.StartAttemptAsync(1, 42);

            Assert.NotNull(result);
            Assert.Equal(100, result.ExamAttemptId);
            Assert.Equal("Test", result.Exam.Title);
        }

        [Fact]
        public async Task StartAttemptAsync_ReturnsNull_WhenExamMissing()
        {
            _mockRepo.Setup(r => r.CreateAttemptAsync(999, 42)).ReturnsAsync((ExamAttempt)null!);

            var result = await _service.StartAttemptAsync(999, 42);

            Assert.Null(result);
        }

        [Fact]
        public async Task SubmitAttemptAsync_ReturnsResult_WhenValid()
        {
            var dto = new SubmitExamAttemptDto { AttemptId = 1 };
            var resultDto = new ExamAttemptResultDto
            {
                AttemptId = 1,
                ScorePercent = 85,
                Results = new List<AnswerResultDto>()
            };

            _mockRepo.Setup(r => r.SaveAnswersAndEvaluateAsync(dto)).ReturnsAsync(resultDto);

            var result = await _service.SubmitAttemptAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(85, result.ScorePercent);
        }

        [Fact]
        public async Task SubmitAttemptAsync_ReturnsNull_WhenInvalid()
        {
            var dto = new SubmitExamAttemptDto { AttemptId = 999 };
            _mockRepo.Setup(r => r.SaveAnswersAndEvaluateAsync(dto)).ReturnsAsync((ExamAttemptResultDto)null!);

            var result = await _service.SubmitAttemptAsync(dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAttemptResultAsync_ReturnsResult_WhenExists()
        {
            var resultDto = new ExamAttemptResultDto { AttemptId = 5, ScorePercent = 77 };

            _mockRepo.Setup(r => r.GetAttemptResultAsync(5, 42)).ReturnsAsync(resultDto);

            var result = await _service.GetAttemptResultAsync(5, 42);

            Assert.NotNull(result);
            Assert.Equal(77, result.ScorePercent);
        }

        [Fact]
        public async Task GetAttemptResultAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetAttemptResultAsync(999, 42)).ReturnsAsync((ExamAttemptResultDto)null!);

            var result = await _service.GetAttemptResultAsync(999, 42);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAttemptsWithEvaluationAsync_ReturnsList()
        {
            var attempts = new List<ExamAttempt>
            {
                new ExamAttempt
                {
                    ExamAttemptId = 1,
                    Exam = new Exam { Title = "Test" },
                    Evaluation = new ExamAttemptEvaluation { Score = 0.8, IsPassed = true },
                    SubmittedAt = DateTime.UtcNow
                }
            };

            _mockRepo.Setup(r => r.GetCompletedAttemptsWithEvaluationAsync(42)).ReturnsAsync(attempts);

            var result = await _service.GetAttemptsWithEvaluationAsync(42);

            Assert.Single(result);
            Assert.Equal("Test", result[0].Exam.Title);
        }
    }
}
