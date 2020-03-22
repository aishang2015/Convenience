using AutoMapper;

using backend.repository.backend.api;

using Backend.Model.backend.api.Models.SystemManage;
using Backend.Repository.backend.api.Data;

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

        public UserService(IUserRepository userRepository, IMapper mapper, SystemIdentityDbContext systemIdentityDbContext)
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
                    isSuccess = await SetUserPassword(model.UserName, model.Password);
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法创建用户，初始密码创建失败！";
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
            Expression<Func<SystemUser, bool>> where = u => (
                (string.IsNullOrEmpty(query.UserName) || u.UserName.Contains(query.UserName)) &&
                (string.IsNullOrEmpty(query.Name) || u.Name.Contains(query.Name)) &&
                (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber)) &&
                (string.IsNullOrEmpty(query.RoleName) || u.Name.Contains(query.RoleName))
            );
            var users = _userRepository.GetUsers(where, query.Page, query.Size).ToArray();
            return _mapper.Map<SystemUser[], IEnumerable<UserResult>>(users);
        }

        public async Task<bool> RemoveUserAsync(string Id)
        {
            return await _userRepository.RemoveUserByIdAsync(Id);
        }

        public async Task<bool> RemoveUserByNameAsync(string Name)
        {
            return await _userRepository.RemoveUserByNameAsync(Name);
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
    }
}
