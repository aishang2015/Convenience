using Backend.Repository.backend.api.Data;

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

        public Task<bool> ResetPasswordAsync(SystemUser user, string password);

        public Task<bool> CheckPasswordAsync(SystemUser user, string password);

        public Task<bool> ChangePasswordAsync(SystemUser user, string oldPassword, string newPassword);

        public Task<bool> UpdateUserAsync(SystemUser user);

        public Task<bool> RemoveUserByIdAsync(string id);

        public Task<bool> RemoveUserByNameAsync(string name);

        public Task<SystemUser> GetUserByIdAsync(string id);

        public Task<SystemUser> GetUserByNameAsync(string name);

        Task<List<SystemUser>> GetUsers(string userName, string Name, string phoneNumber, string roleName, int page, int size);

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size);

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where);

        public IQueryable<SystemUser> GetUsers(int page, int size);

        public IQueryable<SystemUser> GetUsers();

        public Task<IEnumerable<string>> GetUserRolesAsync(SystemUser user);

        Task<bool> AddUserToRoles(SystemUser user, IEnumerable<string> roles);
    }
}
