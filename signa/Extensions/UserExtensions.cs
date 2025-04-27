using System.Security.Claims;
using ErrorOr;

namespace signa.Extensions;

public static class UserExtensions
{
    internal static ErrorOr<Guid> GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (claim == null)
            return Error.Failure("General.Failure", "Невозможно определить ваш ID.");
        
        if (!Guid.TryParse(claim, out var userId))
            return Error.Failure("General.Failure", "Неверный токен или ID пользователя.");
        
        return userId;
    }
}