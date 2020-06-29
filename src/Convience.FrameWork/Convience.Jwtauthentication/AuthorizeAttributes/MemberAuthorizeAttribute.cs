using Microsoft.AspNetCore.Authorization;

namespace Convience.JwtAuthentication.AuthorizeAttributes
{
    public class MemberAuthorizeAttribute : AuthorizeAttribute
    {
        public MemberAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtAuthenticationSchemeConstants.MemberAuthenticationScheme;
        }
    }
}
