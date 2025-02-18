using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.DTOs;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        private readonly string notFound = "Пользователь не найден";

        [Authorize]
        [HttpGet("self/detail")]
        public async Task<IActionResult> GetUserDetail()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserWithRecipesAsync(userId);

            if (user == null)
                return NotFound(new { message = notFound });
            
            return Ok(new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Recipes = user.Recipes,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userService.GetUserWithRecipesAsync(id);

            if (user == null)
                return NotFound(new { message = notFound });

            return Ok(new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Recipes = user.Recipes,
            });
        }
    }
}