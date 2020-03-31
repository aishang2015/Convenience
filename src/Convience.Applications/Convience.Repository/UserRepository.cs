using Convience.Entity.Data;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var users = from u in _systemIdentityDbContext.Users
                        orderby u.Id descending
                        let q =
                             from ur in _systemIdentityDbContext.UserRoles
                             where ur.UserId == u.Id
                             join r in _systemIdentityDbContext.Roles on ur.RoleId equals r.Id
                             select r.Id
                        select new SystemUser
                        {
                            Avatar = u.Avatar,
                            Name = u.Name,
                            UserName = u.UserName,
                            PhoneNumber = u.PhoneNumber,
                            Id = u.Id,
                            IsActive = u.IsActive,
                            Sex = u.Sex,
                            CreatedTime = u.CreatedTime,
                            RoleIds = string.Join(',', q.ToArray())
                        };
            return users;
        }

        public IQueryable<SystemUser> GetUsers(int page, int size)
        {
            var skip = size * (page - 1);
            return GetUsers().Skip(skip).Take(size);
        }

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where)
        {
            return GetUsers().Where(where);
        }

        public IQueryable<SystemUser> GetUsers(Expression<Func<SystemUser, bool>> where, int page, int size)
        {
            var skip = size * (page - 1);
            return GetUsers(where).Skip(skip).Take(size);
        }

        public async Task<List<SystemUser>> GetUsers(string userName, string Name, string phoneNumber, string roleId, int page, int size)
        {
            var role = await _roleManger.FindByIdAsync(roleId);
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            var result = from u in users
                         where u.UserName.Contains(userName ?? string.Empty)
                            && u.Name.Contains(Name ?? string.Empty)
                            && u.PhoneNumber.Contains(phoneNumber ?? string.Empty)
                         orderby u.Id descending
                         let q =
                              from ur in _systemIdentityDbContext.UserRoles
                              where ur.UserId == u.Id
                              join r in _systemIdentityDbContext.Roles on ur.RoleId equals r.Id
                              select r.Id
                         select new SystemUser
                         {
                             Avatar = u.Avatar,
                             Name = u.Name,
                             UserName = u.UserName,
                             PhoneNumber = u.PhoneNumber,
                             Id = u.Id,
                             IsActive = u.IsActive,
                             Sex = u.Sex,
                             CreatedTime = u.CreatedTime,
                             RoleIds = string.Join(',', q.ToArray())
                         };

            var skip = size * (page - 1);
            return result.Skip(skip).Take(size).ToList();
        }

        public async Task<int> GetSuperManagerUserCount()
        {
            var users = await _userManager.GetUsersInRoleAsync("超级管理员");
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

            var result = await _userManager.AddToRolesAsync(user, roleaArray);
            return result.Succeeded;
        }
    }
}
