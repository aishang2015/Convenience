using backend.entity.backend.api;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace backend.repository.backend.api
{
    public interface IUserRepository
    {
        public Task<bool> AddUser(SystemUser user);

        public Task<bool> CheckPasswordAsync(SystemUser user, string password);

        public Task<bool> ChangePasswordAsync(SystemUser user, string oldPassword, string newPassword);

        public Task UpdateUser(SystemUser user);

        public Task RemoveUserById(string id);

        public Task<SystemUser> GetUserById(string id);

        public Task<SystemUser> GetUserByName(string name);

        public IQueryable<SystemUser> GetUsersByPage(IQueryable<SystemUser> users, int page, int size);

        public IQueryable<SystemUser> GetUserByCondition(Expression<Func<SystemUser, bool>> where);
    }
}
