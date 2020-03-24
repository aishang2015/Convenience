using AutoMapper;

using backend.repository.backend.api;
using Backend.Entity.backend.api.Data;
using Backend.Model.backend.api.Models.SystemManage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        public UserService(IUserRepository userRepository,
            IMapper mapper, SystemIdentityDbContext systemIdentityDbContext)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _systemIdentityDbContext = systemIdentityDbContext;
        }

        public async Task<string> AddUserAsync(UserViewModel model)
        {
            using (var trans = _systemIdentityDbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = _mapper.Map<SystemUser>(model);
                    var isSuccess = await _userRepository.AddUserAsync(user);
                    if (!isSuccess)
                    {
                        return "无法创建用户，请检查用户名是否相同！";
                    }
                    isSuccess = await _userRepository.SetPasswordAsync(user, model.Password);
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法创建用户，初始密码创建失败！";
                    }
                    isSuccess = await _userRepository.AddUserToRoles(user,
                        model.RoleNames.Split(',', StringSplitOptions.RemoveEmptyEntries));
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法创建用户，设置角色失败！";
                    }
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    throw e;
                }
            }
            return string.Empty;
        }

        public int Count()
        {
            return _userRepository.GetUsers().Count();
        }

        public async Task<UserResult> GetUserAsync(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            return _mapper.Map<UserResult>(user);
        }

        public IEnumerable<UserResult> GetUsers(UserQuery query)
        {
            if (!string.IsNullOrEmpty(query.RoleName))
            {
                var users = _userRepository.GetUsers(query.UserName, query.Name,
                    query.PhoneNumber, query.RoleName, query.Page, query.Size).Result;
                return _mapper.Map<SystemUser[], IEnumerable<UserResult>>(users.ToArray());
            }
            else
            {
                Expression<Func<SystemUser, bool>> where = u => (
                    (string.IsNullOrEmpty(query.UserName) || u.UserName.Contains(query.UserName)) &&
                    (string.IsNullOrEmpty(query.Name) || u.Name.Contains(query.Name)) &&
                    (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber))
                );
                var users = _userRepository.GetUsers(where, query.Page, query.Size);
                return _mapper.Map<SystemUser[], IEnumerable<UserResult>>(users.ToArray());
            }
        }

        public async Task<string> RemoveUserAsync(string Id)
        {
            using (var trans = _systemIdentityDbContext.Database.BeginTransaction())
            {
                try
                {
                    var isSuccess = await _userRepository.RemoveUserByIdAsync(Id);
                    if (!isSuccess)
                    {
                        return "删除失败！";
                    }
                    var count = await _userRepository.GetSuperManagerUserCount();
                    if (count == 0)
                    {
                        await trans.RollbackAsync();
                        return "可用的超级管理员数量不能为0！";
                    }
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
            return string.Empty;
        }

        public async Task<string> RemoveUserByNameAsync(string Name)
        {
            using (var trans = _systemIdentityDbContext.Database.BeginTransaction())
            {
                try
                {
                    var isSuccess = await _userRepository.RemoveUserByNameAsync(Name);
                    if (!isSuccess)
                    {
                        return "删除失败！";
                    }
                    var count = await _userRepository.GetSuperManagerUserCount();
                    if (count == 0)
                    {
                        await trans.RollbackAsync();
                        return "可用的超级管理员数量不能为0！";
                    }
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }

            return string.Empty;
        }

        public async Task<bool> ResetUserPassword(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            return user != null ?
                await _userRepository.ResetPasswordAsync(user, password) : false;
        }

        public async Task<bool> SetUserPassword(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            return user != null ?
                await _userRepository.SetPasswordAsync(user, password) : false;
        }

        public async Task<string> UpdateUserAsync(UserViewModel model)
        {
            using (var trans = _systemIdentityDbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await _userRepository.GetUserByIdAsync(model.Id.ToString());
                    if (user != null)
                    {
                        _mapper.Map(model, user);
                        var isSuccess = await _userRepository.UpdateUserAsync(user);
                        if (!isSuccess)
                        {
                            return "无法更新用户，请检查用户名是否相同！";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Password))
                            {
                                isSuccess = await _userRepository.ResetPasswordAsync(user, model.Password);
                                if (!isSuccess)
                                {
                                    await trans.RollbackAsync();
                                    return "更新密码失败！";
                                }
                            }
                        }

                        isSuccess = await _userRepository.AddUserToRoles(user,
                            model.RoleNames.Split(',', StringSplitOptions.RemoveEmptyEntries));
                        if (!isSuccess)
                        {
                            await trans.RollbackAsync();
                            return "无法更新用户，更新角色失败！";
                        }

                        var count = await _userRepository.GetSuperManagerUserCount();
                        if (count == 0)
                        {
                            await trans.RollbackAsync();
                            return "可用的超级管理员数量不能为0！";
                        }
                        await trans.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }

            return string.Empty;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            return user.RoleNames.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
