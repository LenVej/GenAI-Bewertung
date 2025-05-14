using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Controller
{
    public class StatsControllerTests
    {
        private readonly Mock<IStatsRepository> _mockRepo;
        private readonly StatsService _service;
        private readonly StatsController _controller;

        public StatsControllerTests()
        {
            _mockRepo = new Mock<IStatsRepository>();
            _service = new StatsService(_mockRepo.Object);
            _controller = new StatsController(_service);
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
        public async Task GetUserStats_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = await _controller.GetUserStats();
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetUserStats_ReturnsStats_WhenUserIsAuthenticated()
        {
            // Arrange
            SetUser(42);

            var mockStats = new ProfileStatsDto
            {
                AverageScorePercent = 85.5,
                TotalCorrect = 12,
                TotalIncorrect = 3,
                WeakSubjects = new List<SubjectWeaknessDto>
                {
                    new SubjectWeaknessDto
                    {
                        Subject = "Mathematik",
                        TotalQuestions = 5,
                        IncorrectAnswers = 2
                    }
                }
            };

            _mockRepo.Setup(r => r.GetStatsForUserAsync(42)).ReturnsAsync(mockStats);

            // Act
            var result = await _controller.GetUserStats();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ProfileStatsDto>(ok.Value);
            Assert.Equal(85.5, dto.AverageScorePercent);
            Assert.Single(dto.WeakSubjects);
            Assert.Equal("Mathematik", dto.WeakSubjects[0].Subject);
        }
    }
}
