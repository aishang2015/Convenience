using Convience.Entity.Data;
using Convience.EntityFrameWork.Repositories;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Convience.Repository
{
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

        public async Task<bool> AddUserAsync(SystemUser user)
        {
            user.CreatedTime = DateTime.Now;
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SetPasswordAsync(SystemUser user, string password)
        {
            var result = await _userManager.AddPasswordAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(SystemUser user, string password)
        {
            var result = await _userManager.RemovePasswordAsync(user);
            return result.Succeeded ? await SetPasswordAsync(user, password) : false;
        }

        public async Task<bool> ChangePasswordAsync(SystemUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(SystemUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
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

        public IQueryable<IdentityUserClaim<int>> GetUserClaims()
        {
            return _systemIdentityDbContext.UserClaims;
        }

        public IQueryable<IdentityUserRole<int>> GetUserRoles()
        {
            return _systemIdentityDbContext.UserRoles;
        }


        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size)
        {
            var skip = size * (page - 1);
            return GetUsers().Where(where).Skip(skip).Take(size);
        }

        public async Task<int> GetUserCountInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Count(user => user.IsActive);
        }

        public async Task<bool> RemoveUserByIdAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return true;
        }

        public async Task<bool> UpdateUserAsync(SystemUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(SystemUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveUserByNameAsync(string name)
        {
            var user = await GetUserByNameAsync(name);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return true;
        }

        public async Task<bool> AddUserToRoles(SystemUser user, IEnumerable<string> roleIds)
        {
            // remove all old roles,add some news
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
