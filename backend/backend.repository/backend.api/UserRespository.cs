using backend.entity.backend.api;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace backend.repository.backend.api
{
    public class UserRespository : IUserRepository
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<SystemRole> _roleManger;

        public UserRespository(UserManager<SystemUser> userManager,
            RoleManager<SystemRole> roleManger)
        {
            _userManager = userManager;
            _roleManger = roleManger;
        }

        public async Task<bool> AddUserAsync(SystemUser user)
        {
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SetPasswordAsync(SystemUser user, string password)
        {
            var result = await _userManager.AddPasswordAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(SystemUser user,string password)
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

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where)
        {
            return _userManager.Users.Where(where);
        }

        public async Task<SystemUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<SystemUser> GetUserByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size)
        {
            var skip = size * (page - 1);
            return GetUsers(where).Skip(skip).Take(size);
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

        public IQueryable<SystemUser> GetUsers(int page, int size)
        {
            var skip = size * (page - 1);
            return _userManager.Users.Skip(skip).Take(size);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(SystemUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public IQueryable<SystemUser> GetUsers()
        {
            return _userManager.Users;
        }
    }
}
