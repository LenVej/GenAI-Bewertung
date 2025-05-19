using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Mappers;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GenAI_Bewertung.Tests.Controller
{
    public class ExamsControllerTests
    {
        private readonly Mock<IExamRepository> _mockRepo;
        private readonly ExamService _service;
        private readonly ExamsController _controller;

        public ExamsControllerTests()
        {
            _mockRepo = new Mock<IExamRepository>();
            _service = new ExamService(_mockRepo.Object);
            _controller = new ExamsController(_service);
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
        public async Task GetAll_ReturnsOk_WithExams()
        {
            var exams = new List<Exam> { new Exam { ExamId = 1, Title = "Test", CreatedBy = 1 } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(exams);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<ExamDto>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var exam = new Exam { ExamId = 1, Title = "Test", CreatedBy = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ExamDto>(ok.Value);
            Assert.Equal("Test", dto.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Exam)null!);

            var result = await _controller.GetById(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = await _controller.Create(new CreateExamDto { Title = "Test" });
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOk_WhenValid()
        {
            SetUser(1);

            var dto = new CreateExamDto { Title = "Test", Description = "Beschreibung" };
            var result = await _controller.Create(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var exam = Assert.IsType<ExamDto>(ok.Value);
            Assert.Equal("Test", exam.Title);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            var exam = new Exam { ExamId = 1, CreatedBy = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);

            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Exam)null!);

            var result = await _controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetMyExams_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = await _controller.GetMyExams();
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetMyExams_ReturnsOk_WithUserExams()
        {
            SetUser(5);
            var exams = new List<Exam> { new Exam { ExamId = 1, Title = "User Exam", CreatedBy = 5 } };
            _mockRepo.Setup(r => r.GetByUserIdAsync(5)).ReturnsAsync(exams);

            var result = await _controller.GetMyExams();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<ExamDto>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenExamMissing()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Exam)null!);
            SetUser(1);

            var result = await _controller.Update(999, new UpdateExamDto());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsForbidden_WhenNotOwner()
        {
            var exam = new Exam { ExamId = 1, CreatedBy = 2 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);
            SetUser(1);

            var result = await _controller.Update(1, new UpdateExamDto());
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenOwner()
        {
            var exam = new Exam { ExamId = 1, CreatedBy = 1, Title = "Alt" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);
            SetUser(1);

            var result = await _controller.Update(1, new UpdateExamDto { Title = "Neu" });

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ExamDto>(ok.Value);
            Assert.Equal("Neu", dto.Title);
        }
    }
}
