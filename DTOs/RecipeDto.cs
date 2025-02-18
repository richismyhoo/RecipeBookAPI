using RecipeBookAPI.Enums;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.DTOs;

public class RecipeDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SpicyLevels SpicyLevel { get; set; }
    public int TimeToCompleteInMinutes { get; set; }
    public RecipeComplexity Complexity { get; set; }
    public Country Country { get; set; }
    public int UserId { get; set; }
    public string AuthorName { get; set; }

    public RecipeDto(Recipe recipe)
    {
        Id = recipe.Id;
        Title = recipe.Title;
        Description = recipe.Description;
        TimeToCompleteInMinutes = recipe.TimeToCompleteInMinutes;
        SpicyLevel = recipe.SpicyLevel;
        Complexity = recipe.Complexity;
        Country = recipe.Country;
        UserId = recipe.UserId;
        AuthorName = recipe.User.Name;
    }
}