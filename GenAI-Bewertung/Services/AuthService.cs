using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GenAI_Bewertung.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(RegisterDto dto)
        {
            if (await _repository.UserExistsAsync(dto.Email))
                return (false, "Benutzer existiert bereits");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repository.AddUserAsync(user);
            return (true, "Benutzer erfolgreich registriert");
        }

        public async Task<(bool Success, string Message, string? Token)> LoginUserAsync(LoginDto dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return (false, "Ungültige Zugangsdaten", null);

            var token = GenerateJwtToken(user);
            return (true, "Login erfolgreich", token);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _repository.GetUserByIdAsync(userId);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
