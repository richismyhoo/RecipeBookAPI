using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegisterRequest request)
        {
            var (success, message, user) = await _authService.Register(request.Email, request.Password, request.Name);

            var token = await _authService.Login(request.Email, request.Password);

            if (!success)
                return BadRequest(new { message });
            
            return Ok(new { message, user, token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.Login(request.Email, request.Password);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new {ex.Message});
            }
        }
    }

    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, MinLength(8)]
        public string Password { get; set; }
        
        [Required, MinLength(4)]
        public string Name { get; set; }
    }

    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}