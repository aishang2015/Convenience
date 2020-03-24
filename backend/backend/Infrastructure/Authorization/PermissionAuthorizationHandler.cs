using Backend.Service.backend.api.SystemManage.User;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IUserService _userService;

        public PermissionAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            var userRoles = context.User.GetUserRoles()
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (userRoles.Contains("超级管理员"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
