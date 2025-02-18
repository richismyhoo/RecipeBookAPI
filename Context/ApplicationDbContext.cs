using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId);
    }
    
}