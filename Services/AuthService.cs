using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeBookAPI.Context;
using RecipeBookAPI.Models;

namespace RecipeBookAPI.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<(bool Success, string Message, User user)> Register(string email, string password, string name)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
            return (false, "Пользователь с такой почтой уже существует", null);
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Email = email,
            PasswordHash = hashPassword,
            Name = name
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, "Успешная регистрация", user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new Exception("Пользователь не найден");
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Неверный пароль");

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var token = new JwtSecurityToken(
            null,
            _config["Jwt:Audience"],
            claims,
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}