
using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Convience.Entity.Data
{
    public interface IRoleRepository
    {
        Task<bool> RemoveRole(string roleName);

        Task<bool> UpdateRole(SystemRole role);

        Task<SystemRole> GetRole(string roleName);

        Task<SystemRole> GetRoleById(string id);

        IQueryable<SystemRole> GetRoles();

        Task<int> GetMemberCount(string roleName);

        Task<bool> AddOrUpdateRoleClaim(SystemRole role, string claimType, string claimValue);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<SystemRole> _roleManager;
        private readonly UserManager<SystemUser> _userManager;

        public RoleRepository(RoleManager<SystemRole> roleManager, UserManager<SystemUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IQueryable<SystemRole> GetRoles()
        {
            return _roleManager.Roles.OrderBy(r => r.Id);
        }

        public async Task<bool> RemoveRole(string roleName)
        {
            var role = await GetRole(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            return true;
        }

        public async Task<SystemRole> GetRole(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<SystemRole> GetRoleById(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateRole(SystemRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<int> GetMemberCount(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Count;
        }

        public async Task<bool> AddOrUpdateRoleClaim(SystemRole role, string claimType, string claimValue)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var values = from claim in claims
                         where claim.Type == claimType
                         select claim;
            if (values.Count() > 0)
            {
                await _roleManager.RemoveClaimAsync(role, values.FirstOrDefault());
            }
            var result = await _roleManager.AddClaimAsync(role, new Claim(type: claimType, value: claimValue));

            return result.Succeeded;
        }
    }
}
