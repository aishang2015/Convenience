using backend.entity.backend.api;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace backend.repository.backend.api
{
    public interface IUserRepository
    {
        public Task<bool> AddUserAsync(SystemUser user);

        public Task<bool> SetPasswordAsync(SystemUser user, string password);

        public Task<bool> CheckPasswordAsync(SystemUser user, string password);

        public Task<bool> ChangePasswordAsync(SystemUser user, string oldPassword, string newPassword);

        public Task<bool> UpdateUserAsync(SystemUser user);

        public Task<bool> RemoveUserByIdAsync(string id);

        public Task<SystemUser> GetUserByIdAsync(string id);

        public Task<SystemUser> GetUserByNameAsync(string name);

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size);

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where);

        public IQueryable<SystemUser> GetUsers(int page, int size);

        public IQueryable<SystemUser> GetUsers();

        public Task<IEnumerable<string>> GetUserRolesAsync(SystemUser user);
    }
}
