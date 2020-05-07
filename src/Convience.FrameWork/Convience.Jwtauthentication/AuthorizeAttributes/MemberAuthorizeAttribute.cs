using Microsoft.AspNetCore.Authorization;

namespace Convience.Jwtauthentication.AuthorizeAttributes
{
    public class MemberAuthorizeAttribute : AuthorizeAttribute
    {
        public MemberAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtAuthenticationSchemeConstants.MemberAuthenticationScheme;
        }
    }
}
