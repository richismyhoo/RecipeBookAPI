using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Enums;
using RecipeBookAPI.Models;
using RecipeBookAPI.Services;
using System.Security.Claims;
using RecipeBookAPI.DTOs;

namespace RecipeBookAPI.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("all/{pageNumber}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber, [FromQuery] int pageSize = 30, SpicyLevels? spicyLevel = null, RecipeComplexity? complexity = null, Country? country = null)
        {
            var recipeDtos = await _recipeService.GetRecipes(pageNumber, pageSize, spicyLevel, complexity, country);
            
            return Ok(recipeDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe([FromRoute] int id)
        {
            var recipe = await _recipeService.GetRecipe(id);

            if (recipe == null)
                return NotFound("Рецепт не найден");

            return Ok(recipe);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddRecipe([FromBody] CreateRecipeRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var recipeDto = await _recipeService.AddRecipe(request, userId);
            return CreatedAtAction(nameof(GetRecipe), new { id = recipeDto.Id }, recipeDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRecipe([FromBody] Recipe recipe)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _recipeService.UpdateRecipe(recipe, userId);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRecipe([FromRoute] int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _recipeService.DeleteRecipe(id, userId);
            return NoContent();
        }
    }

    public class CreateRecipeRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public SpicyLevels SpicyLevel { get; set; }
        [Required]
        public RecipeComplexity Complexity { get; set; }
        [Required]
        public Country Country { get; set; }
        [Required]
        public int TimeToCompleteInMinutes { get; set; }
    }
}