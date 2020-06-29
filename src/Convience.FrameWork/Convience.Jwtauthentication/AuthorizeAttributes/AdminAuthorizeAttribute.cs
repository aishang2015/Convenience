using Microsoft.AspNetCore.Authorization;

namespace Convience.JwtAuthentication.AuthorizeAttributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtAuthenticationSchemeConstants.AdminAuthenticationScheme;
        }
    }
}
