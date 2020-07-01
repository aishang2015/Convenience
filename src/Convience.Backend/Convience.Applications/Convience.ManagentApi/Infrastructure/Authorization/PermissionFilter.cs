using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.Authorization
{
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly string _name;

        public PermissionFilter(IAuthorizationService authorizationService, string name)
        {
            _authorizationService = authorizationService;
            _name = name;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var result = await _authorizationService.AuthorizeAsync(context.HttpContext.User, null,
                new PermissionAuthorizationRequirement(_name));
            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
