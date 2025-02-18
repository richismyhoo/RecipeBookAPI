using Microsoft.EntityFrameworkCore;
using RecipeBookAPI.Context;
using RecipeBookAPI.DTOs;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserWithRecipesAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Recipes)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Recipes = user.Recipes.Select(r => new RecipeDto(r)).ToList()
        };
    }
}