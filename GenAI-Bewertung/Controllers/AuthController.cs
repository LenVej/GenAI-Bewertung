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
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);

            if (!result.Success)
                return Unauthorized(result.Message);

            return Ok(new { token = result.Token });
        }

        // GET: api/auth/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kein Benutzer gefunden");

            var user = await _authService.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
                return NotFound();

            return Ok(new { user.Username, user.Email, user.CreatedAt });
        }
    }
}