
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Convience.Entity.Data
{

    public interface IUserRepository
    {

        public Task<SystemUser> GetUserByIdAsync(string id);

        public Task<SystemUser> GetUserByNameAsync(string name);

        public IQueryable<SystemUser> GetUsers();

        public DbSet<IdentityUserClaim<int>> GetUserClaims();

        public DbSet<IdentityUserRole<int>> GetUserRoles();

        Task<bool> AddUserToRoles(SystemUser user, IEnumerable<string> roleIds);

        Task<int> GetUserCountInRoleAsync(string roleName);

        Task UpdateUserClaimsAsync(SystemUser user, string claimType, IEnumerable<string> values);
    }

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<SystemRole> _roleManger;
        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        public UserRepository(UserManager<SystemUser> userManager,
            RoleManager<SystemRole> roleManger,
            SystemIdentityDbContext systemIdentityDbContext)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _systemIdentityDbContext = systemIdentityDbContext;
        }

        public async Task<SystemUser> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var roleIds = from ur in _systemIdentityDbContext.UserRoles
                              where ur.UserId == user.Id
                              select ur.RoleId;
                user.RoleIds = string.Join(',', roleIds);
            }
            return user;
        }

        public async Task<SystemUser> GetUserByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                var roleIds = from ur in _systemIdentityDbContext.UserRoles
                              where ur.UserId == user.Id
                              select ur.RoleId;
                user.RoleIds = string.Join(',', roleIds);
            }
            return user;
        }

        public IQueryable<SystemUser> GetUsers()
        {
            return _userManager.Users;
        }

        public DbSet<IdentityUserClaim<int>> GetUserClaims()
        {
            return _systemIdentityDbContext.UserClaims;
        }

        public DbSet<IdentityUserRole<int>> GetUserRoles()
        {
            return _systemIdentityDbContext.UserRoles;
        }

        public async Task<int> GetUserCountInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Count(user => user.IsActive);
        }

        public async Task<bool> AddUserToRoles(SystemUser user, IEnumerable<string> roleIds)
        {
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            var roleaArray = from role in _roleManger.Roles
                             where roleIds.Contains(role.Id.ToString())
                             select role.Name;

            var result = await _userManager.AddToRolesAsync(user, roleaArray.ToArray());
            return result.Succeeded;
        }


        public async Task UpdateUserClaimsAsync(SystemUser user, string claimType, IEnumerable<string> values)
        {
            var ucs = from uc in _systemIdentityDbContext.UserClaims
                      where uc.ClaimType == claimType && uc.UserId == user.Id
                      select new Claim(uc.ClaimType, uc.ClaimValue);
            await _userManager.RemoveClaimsAsync(user, ucs.ToArray());

            var newucs = from v in values
                         select new Claim(claimType, v ?? string.Empty);
            await _userManager.AddClaimsAsync(user, newucs.ToArray());
        }
    }
}
