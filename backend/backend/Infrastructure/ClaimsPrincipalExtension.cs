using Backend.Jwtauthentication;

using System.Linq;
using System.Security.Claims;

namespace Backend.Api.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserName)
                ?.Value ?? string.Empty;
        }

        public static string GetUserRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserRoles)
                ?.Value ?? string.Empty;
        }
    }
}
