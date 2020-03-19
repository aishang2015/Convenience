using backend.entity.backend.api;

using Microsoft.AspNetCore.Identity;

using System;
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

        public async Task<bool> AddUser(SystemUser user)
        {
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded;
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

        public async Task<SystemUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<SystemUser> GetUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size)
        {
            var skip = size * (page - 1);
            return GetUsers(where).Skip(skip).Take(size);
        }

        public async Task RemoveUserById(string id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task UpdateUser(SystemUser user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}
