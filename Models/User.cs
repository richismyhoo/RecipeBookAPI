namespace RecipeBookAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; }

    public User()
    {
        Recipes = new HashSet<Recipe>();
    }
}