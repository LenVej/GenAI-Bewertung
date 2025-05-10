using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GenAI_Bewertung.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterUserAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);

            if (!result.Success)
                return Unauthorized(result.Message);

            return Ok(new
            {
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken
            });

        }

        // GET: api/auth/profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _authService.GetUserByIdAsync(int.Parse(userId));
            if (user == null) return NotFound();

            return Ok(new UserProfileDto
            {
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Tolerance = user.Tolerance,
                CaseSensitive = user.CaseSensitive,
                EstimateTolerance = user.EstimateTolerance
            });
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var (success, newAccessToken, newRefreshToken) = await _authService.RefreshLoginAsync(refreshToken);
            if (!success) return Unauthorized("Refresh Token ungültig oder abgelaufen");

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
        
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = int.Parse(userIdStr);
            var success = await _authService.DeleteUserAsync(userId);

            if (!success)
                return NotFound(new { message = "Benutzer nicht gefunden" });

            return Ok(new { message = "Benutzer erfolgreich gelöscht" });
        }

        [Authorize]
        [HttpPut("profile/settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateUserSettingsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _authService.GetUserByIdAsync(int.Parse(userId));
            if (user == null) return NotFound();

            user.Tolerance = dto.Tolerance;
            user.CaseSensitive = dto.CaseSensitive;
            user.EstimateTolerance = dto.EstimateTolerance;

            await _authService.UpdateUserAsync(user);
            return Ok(new { message = "Einstellungen gespeichert" });
        }


    }
}