using Microsoft.AspNetCore.Authorization;

namespace Convience.Jwtauthentication.AuthorizeAttributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtAuthenticationSchemeConstants.AdminAuthenticationScheme;
        }
    }
}
