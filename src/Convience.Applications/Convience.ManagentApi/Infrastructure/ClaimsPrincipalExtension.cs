using Convience.Jwtauthentication;

using System.Linq;
using System.Security.Claims;

namespace Convience.ManagentApi.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Name)
                ?.Value ?? string.Empty;
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserName)
                ?.Value ?? string.Empty;
        }

        public static string GetUserRoleIds(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserRoleIds)
                ?.Value ?? string.Empty;
        }
    }
}
