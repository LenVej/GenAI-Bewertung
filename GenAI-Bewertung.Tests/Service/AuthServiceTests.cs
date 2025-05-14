using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using GenAI_Bewertung.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GenAI_Bewertung.Tests.Service
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockRepo;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IAuthRepository>();

            var mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "this_is_a_super_secure_test_key_123!" }
                }!)
                .Build();

            _service = new AuthService(_mockRepo.Object, mockConfig);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsSuccess_WhenNewUser()
        {
            var dto = new RegisterDto
            {
                Username = "tester",
                Email = "test@example.com",
                Password = "pw"
            };

            _mockRepo.Setup(r => r.UserExistsAsync(dto.Email)).ReturnsAsync(false);

            var result = await _service.RegisterUserAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("Benutzer erfolgreich registriert", result.Message);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailure_WhenEmailExists()
        {
            _mockRepo.Setup(r => r.UserExistsAsync("test@example.com")).ReturnsAsync(true);

            var dto = new RegisterDto { Email = "test@example.com" };
            var result = await _service.RegisterUserAsync(dto);

            Assert.False(result.Success);
            Assert.Contains("existiert", result.Message);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginUserAsync_ReturnsSuccess_WhenValidCredentials()
        {
            var password = "pw";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User { UserId = 1, Email = "x@test.de", Username = "test", PasswordHash = hash };
            var dto = new LoginDto { Email = user.Email, Password = password };

            _mockRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync(user);

            var result = await _service.LoginUserAsync(dto);

            Assert.True(result.Success);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task LoginUserAsync_ReturnsFailure_WhenInvalidCredentials()
        {
            var dto = new LoginDto { Email = "notfound@test.de", Password = "wrong" };
            _mockRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync((User)null!);

            var result = await _service.LoginUserAsync(dto);

            Assert.False(result.Success);
            Assert.Null(result.AccessToken);
            Assert.Null(result.RefreshToken);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenExists()
        {
            var user = new User { UserId = 10, Username = "u" };
            _mockRepo.Setup(r => r.GetUserByIdAsync(10)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("u", result.Username);
        }

        [Fact]
        public async Task RefreshLoginAsync_ReturnsNewTokens_WhenValid()
        {
            var user = new User
            {
                UserId = 1,
                RefreshToken = "old",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                Username = "x"
            };

            _mockRepo.Setup(r => r.GetUserByRefreshTokenAsync("old")).ReturnsAsync(user);

            var result = await _service.RefreshLoginAsync("old");

            Assert.True(result.Success);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.NewRefreshToken);
        }

        [Fact]
        public async Task RefreshLoginAsync_ReturnsFailure_WhenInvalid()
        {
            _mockRepo.Setup(r => r.GetUserByRefreshTokenAsync("expired")).ReturnsAsync((User)null!);

            var result = await _service.RefreshLoginAsync("expired");

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsTrue_WhenDeleted()
        {
            var user = new User { UserId = 1 };
            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _service.DeleteUserAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsFalse_WhenUserMissing()
        {
            _mockRepo.Setup(r => r.GetUserByIdAsync(123)).ReturnsAsync((User)null!);

            var result = await _service.DeleteUserAsync(123);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsRepository()
        {
            var user = new User { UserId = 1 };
            await _service.UpdateUserAsync(user);
            _mockRepo.Verify(r => r.UpdateUserAsync(user), Times.Once);
        }
    }
}
