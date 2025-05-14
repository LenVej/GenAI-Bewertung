using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Service
{
    public class StatsServiceTests
    {
        private readonly Mock<IStatsRepository> _mockRepo;
        private readonly StatsService _service;

        public StatsServiceTests()
        {
            _mockRepo = new Mock<IStatsRepository>();
            _service = new StatsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetUserStatsAsync_ReturnsCorrectStats()
        {
            // Arrange
            var expectedStats = new ProfileStatsDto
            {
                AverageScorePercent = 82.3,
                TotalCorrect = 10,
                TotalIncorrect = 2,
                WeakSubjects = new List<SubjectWeaknessDto>
                {
                    new SubjectWeaknessDto
                    {
                        Subject = "Mathe",
                        TotalQuestions = 4,
                        IncorrectAnswers = 2
                    }
                }
            };

            _mockRepo.Setup(r => r.GetStatsForUserAsync(1)).ReturnsAsync(expectedStats);

            // Act
            var result = await _service.GetUserStatsAsync(1);

            // Assert
            Assert.Equal(82.3, result.AverageScorePercent);
            Assert.Equal(10, result.TotalCorrect);
            Assert.Single(result.WeakSubjects);
            Assert.Equal("Mathe", result.WeakSubjects[0].Subject);
        }
    }
}