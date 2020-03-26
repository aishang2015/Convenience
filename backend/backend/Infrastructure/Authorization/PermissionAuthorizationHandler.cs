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
            var userRoles = context.User.GetUserRoles()
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (userRoles.Contains("超级管理员"))
            {
                context.Succeed(requirement);
            }
            var menuIds = _roleService.GetRoleClaimsByName(userRoles, CustomClaimTypes.RoleMenus);
            if (_menuService.HavePermission(menuIds.ToArray(), requirement.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
