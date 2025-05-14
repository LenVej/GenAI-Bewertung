using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Controller
{
    public class ExamAttemptsControllerTests
    {
        private readonly Mock<IExamAttemptRepository> _mockRepo;
        private readonly ExamAttemptService _service;
        private readonly ExamAttemptsController _controller;

        public ExamAttemptsControllerTests()
        {
            _mockRepo = new Mock<IExamAttemptRepository>();
            _service = new ExamAttemptService(_mockRepo.Object);
            _controller = new ExamAttemptsController(_service);
        }

        private void SetUser(int userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task StartExam_ReturnsNotFound_WhenExamMissing()
        {
            SetUser(1);
            _mockRepo.Setup(r => r.CreateAttemptAsync(999, 1)).ReturnsAsync((ExamAttempt)null!);

            var result = await _controller.StartExam(new StartExamAttemptDto { ExamId = 999 });

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Contains("nicht gefunden", notFound.Value?.ToString());
        }

        [Fact]
        public async Task StartExam_ReturnsStartedAttempt_WhenSuccess()
        {
            SetUser(2);
            var exam = new Exam { ExamId = 1, Title = "Test" };
            var attempt = new ExamAttempt { ExamAttemptId = 10, Exam = exam };

            _mockRepo.Setup(r => r.CreateAttemptAsync(1, 2)).ReturnsAsync(attempt);

            var result = await _controller.StartExam(new StartExamAttemptDto { ExamId = 1 });

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<StartedExamAttemptDto>(ok.Value);
            Assert.Equal(10, dto.AttemptId);
            Assert.Equal("Test", dto.ExamTitle);
        }

        [Fact]
        public async Task Submit_ReturnsBadRequest_WhenFailed()
        {
            var dto = new SubmitExamAttemptDto { AttemptId = 99 };

            _mockRepo.Setup(r => r.SaveAnswersAndEvaluateAsync(dto)).ReturnsAsync((ExamAttemptResultDto)null!);

            var result = await _controller.Submit(dto);
            var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Ungültig", bad.Value?.ToString());
        }

        [Fact]
        public async Task Submit_ReturnsResult_WhenSuccess()
        {
            var dto = new SubmitExamAttemptDto { AttemptId = 1 };
            var resultDto = new ExamAttemptResultDto { AttemptId = 1, ScorePercent = 88 };

            _mockRepo.Setup(r => r.SaveAnswersAndEvaluateAsync(dto)).ReturnsAsync(resultDto);

            var result = await _controller.Submit(dto);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnDto = Assert.IsType<ExamAttemptResultDto>(ok.Value);
            Assert.Equal(88, returnDto.ScorePercent);
        }

        [Fact]
        public async Task GetResult_ReturnsNotFound_WhenInvalid()
        {
            SetUser(1);
            _mockRepo.Setup(r => r.GetAttemptResultAsync(1, 1)).ReturnsAsync((ExamAttemptResultDto)null!);

            var result = await _controller.GetResult(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetResult_ReturnsResult_WhenExists()
        {
            SetUser(1);
            var resultDto = new ExamAttemptResultDto
            {
                AttemptId = 1,
                ExamId = 1,
                ExamTitle = "Prüfung",
                ScorePercent = 70
            };

            _mockRepo.Setup(r => r.GetAttemptResultAsync(1, 1)).ReturnsAsync(resultDto);

            var result = await _controller.GetResult(1);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ExamAttemptResultDto>(ok.Value);
            Assert.Equal("Prüfung", dto.ExamTitle);
        }

        [Fact]
        public async Task GetMyProgress_ReturnsList_WhenExists()
        {
            SetUser(5);
            var attempts = new List<ExamAttempt>
            {
                new ExamAttempt
                {
                    ExamAttemptId = 1,
                    Exam = new Exam { Title = "Prüfung 1" },
                    SubmittedAt = DateTime.UtcNow,
                    Evaluation = new ExamAttemptEvaluation { Score = 0.7, IsPassed = true }
                }
            };

            _mockRepo.Setup(r => r.GetCompletedAttemptsWithEvaluationAsync(5)).ReturnsAsync(attempts);

            var result = await _controller.GetMyProgress();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsType<List<ExamAttemptOverviewDto>>(ok.Value);
            Assert.Single(list);
            Assert.True(list[0].IsPassed);
        }
    }
}
