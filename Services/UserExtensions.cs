using System.Security.Claims;

namespace RecipeBookAPI.Services;

public static class UserExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("ID пользователя отсутствует в токене, свяжитесь с администрацией");

        return userId;
    }
}