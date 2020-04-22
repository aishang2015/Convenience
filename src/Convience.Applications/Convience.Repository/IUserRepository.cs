using Convience.Entity.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Repository
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

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size);

        public IQueryable<SystemUser> GetUsers();

        public DbSet<IdentityUserClaim<int>> GetUserClaims();

        public DbSet<IdentityUserRole<int>> GetUserRoles();

        public Task<IEnumerable<string>> GetUserRolesAsync(SystemUser user);

        Task<bool> AddUserToRoles(SystemUser user, IEnumerable<string> roleIds);

        Task<int> GetUserCountInRoleAsync(string roleName);

        Task UpdateUserClaimsAsync(SystemUser user, string claimType, IEnumerable<string> values);
    }
}
