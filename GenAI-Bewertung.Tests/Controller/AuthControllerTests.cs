using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthRepository> _mockRepo;
        private readonly AuthService _authService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockRepo = new Mock<IAuthRepository>();

            var mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "this_is_a_very_secure_test_key_123!" } // >= 256-bit
                }!).Build();

            _authService = new AuthService(_mockRepo.Object, mockConfig);
            _controller = new AuthController(_authService);
        }

        private void SetUser(int userId)
        {
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claims }
            };
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenSuccess()
        {
            var dto = new RegisterDto { Username = "test", Email = "t@t.de", Password = "pw" };
            _mockRepo.Setup(r => r.UserExistsAsync(dto.Email)).ReturnsAsync(false);

            var result = await _controller.Register(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(ok.Value));
            Assert.Equal("Benutzer erfolgreich registriert", dict?["message"]);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists()
        {
            var dto = new RegisterDto { Email = "t@t.de" };
            _mockRepo.Setup(r => r.UserExistsAsync(dto.Email)).ReturnsAsync(true);

            var result = await _controller.Register(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(bad.Value));
            Assert.Contains("existiert", dict?["message"]);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsValid()
        {
            var dto = new LoginDto { Email = "test@test.de", Password = "pw" };
            var user = new User
            {
                UserId = 1,
                Email = dto.Email,
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pw")
            };

            _mockRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync(user);

            var result = await _controller.Login(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(ok.Value));
            Assert.NotNull(tokenData?["accessToken"]);
            Assert.NotNull(tokenData?["refreshToken"]);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalid()
        {
            var dto = new LoginDto { Email = "notfound@test.de", Password = "pw" };
            _mockRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync((User)null!);

            var result = await _controller.Login(dto);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetProfile_ReturnsOk_WhenAuthenticated()
        {
            SetUser(1);
            var user = new User
            {
                UserId = 1,
                Username = "Tester",
                Email = "x@test.de",
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetProfile();
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<UserProfileDto>(ok.Value);
            Assert.Equal("Tester", dto.Username);
        }

        [Fact]
        public async Task GetProfile_ReturnsUnauthorized_WhenUserMissing()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var result = await _controller.GetProfile();
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Refresh_ReturnsOk_WhenValidToken()
        {
            var user = new User
            {
                UserId = 1,
                RefreshToken = "old",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            _mockRepo.Setup(r => r.GetUserByRefreshTokenAsync("old")).ReturnsAsync(user);

            var result = await _controller.Refresh("old");
            var ok = Assert.IsType<OkObjectResult>(result);
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(ok.Value));
            Assert.NotNull(tokenData?["accessToken"]);
        }

        [Fact]
        public async Task Refresh_ReturnsUnauthorized_WhenInvalidToken()
        {
            _mockRepo.Setup(r => r.GetUserByRefreshTokenAsync("expired")).ReturnsAsync((User)null!);

            var result = await _controller.Refresh("expired");
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsOk_WhenUserExists()
        {
            SetUser(1);
            var user = new User { UserId = 1 };

            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.DeleteAccount();
            var ok = Assert.IsType<OkObjectResult>(result);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(ok.Value));
            Assert.Equal("Benutzer erfolgreich gelöscht", dict?["message"]);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsNotFound_WhenUserNotFound()
        {
            SetUser(99);
            _mockRepo.Setup(r => r.GetUserByIdAsync(99)).ReturnsAsync((User)null!);

            var result = await _controller.DeleteAccount();
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateSettings_ReturnsOk_WhenUserExists()
        {
            SetUser(1);
            var user = new User { UserId = 1 };
            var dto = new UpdateUserSettingsDto
            {
                Tolerance = "medium",
                CaseSensitive = true,
                EstimateTolerance = 10
            };

            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.UpdateSettings(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(ok.Value));
            Assert.Contains("Einstellungen", dict?["message"]);
        }

        [Fact]
        public async Task UpdateSettings_ReturnsNotFound_WhenUserMissing()
        {
            SetUser(1);
            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync((User)null!);

            var result = await _controller.UpdateSettings(new UpdateUserSettingsDto());
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
