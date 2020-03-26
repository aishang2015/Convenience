using Backend.Jwtauthentication;
using Backend.Service.backend.api.SystemManage.Menu;
using Backend.Service.backend.api.SystemManage.Role;
using Backend.Service.backend.api.SystemManage.User;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;

        public PermissionAuthorizationHandler(IRoleService roleService, IMenuService menuService)
        {
            _roleService = roleService;
            _menuService = menuService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            var userRoleIds = context.User.GetUserRoleIds()
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (userRoleIds.Contains("1"))
            {
                context.Succeed(requirement);
            }
            var menuIds = _roleService.GetRoleClaimValue(userRoleIds, CustomClaimTypes.RoleMenus);
            if (_menuService.HavePermission(menuIds.ToArray(), requirement.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
