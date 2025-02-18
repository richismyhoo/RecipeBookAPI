using Microsoft.EntityFrameworkCore;
using RecipeBookAPI.Context;
using RecipeBookAPI.Controllers;
using RecipeBookAPI.DTOs;
using RecipeBookAPI.Enums;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services;

public class RecipeService
{
    private readonly ApplicationDbContext _context;

    public RecipeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecipeDto> AddRecipe(CreateRecipeRequest request, int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new Exception("Пользователь с данным ID не найден");
        }
        
        var recipe = new Recipe
        {
            Title = request.Title,
            Description = request.Description,
            SpicyLevel = request.SpicyLevel,
            Complexity = request.Complexity,
            Country = request.Country,
            TimeToCompleteInMinutes = request.TimeToCompleteInMinutes,
            UserId = userId
        };
        
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        
        return new RecipeDto(recipe);
    }

    public async Task<RecipeDto> GetRecipe(int id)
    {
        var recipe = await _context.Recipes 
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
            throw new Exception("Рецепт не найден");

        return new RecipeDto(recipe);
    }

    public async Task<RecipeDto> UpdateRecipe(Recipe recipe, int issuerId)
    {
        var creator = await _context.Recipes
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == recipe.UserId);
        var creatorId = creator.UserId;

        if (creatorId != issuerId)
            throw new Exception("У вас нет прав изменять рецепт");
        
        _context.Entry(recipe).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return new RecipeDto(recipe);
    }

    public async Task<RecipeDto> DeleteRecipe(int id, int issuerId)
    {
        
        
        var recipe = await _context.Recipes.FindAsync(id);
        
        var creator = await _context.Recipes
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == recipe.UserId);
        var creatorId = creator.UserId;

        if (creatorId != issuerId)
            throw new Exception("У вас нет прав удалять рецепт");
        
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        
        return new RecipeDto(recipe);
    }

    public async Task<List<RecipeDto>> GetRecipes(int page, int pageSize, SpicyLevels? spicyLevel, RecipeComplexity? complexity, Country? country)
    {
        if (page < 1) page = 1;

        IQueryable<Recipe> query = _context.Recipes.Include(r => r.User);

        if (spicyLevel.HasValue)
        {
            query = query.Where(r => r.SpicyLevel == spicyLevel.Value);
        }

        if (complexity.HasValue)
        {
            query = query.Where(r => r.Complexity == complexity.Value);
        }

        if (country.HasValue)
        {
            query = query.Where(r => r.Country == country.Value);
        }

        return await query
            .OrderBy(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeDto(r))
            .ToListAsync();
    }
}