using System.Security.Claims;

namespace Web.Domain.Enums
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
