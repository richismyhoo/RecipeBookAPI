using RecipeBookAPI.Enums;

namespace RecipeBookAPI.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SpicyLevels SpicyLevel { get; set; }
    public int TimeToCompleteInMinutes { get; set; }
    public RecipeComplexity Complexity { get; set; }
    public Country Country { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}
