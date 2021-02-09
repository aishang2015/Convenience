using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Injection;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.SystemManage;
using Convience.Util.Extension;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{
    public interface IUserService : IBaseService
    {
        public IEnumerable<DicResultModel> GetUserDic(string name);

        public PagingResultModel<UserResultModel> GetUsers(UserQueryModel query);

        public Task<UserResultModel> GetUserAsync(string Id);

        public Task<string> AddUserAsync(UserViewModel model);

        public Task<string> UpdateUserAsync(UserViewModel model);

        public Task<string> RemoveUserAsync(string Id);

        public Task<string> RemoveUserByNameAsync(string Name);

        public Task<bool> SetUserPassword(string userName, string password);

        public Task<bool> ResetUserPassword(string userName, string password);
    }

    public class UserService : BaseService, IUserService
    {

#pragma warning disable CS0649

        [Autowired]
        private readonly ILogger<UserService> _logger;

        [Autowired]
        private readonly IUserRepository _userRepository;

        [Autowired]
        private readonly IRoleRepository _roleRepository;

        [Autowired]
        private readonly IRepository<Position> _positionRepository;

        [Autowired]
        private readonly IRepository<Department> _departmentRepository;

        [Autowired]
        private readonly IMapper _mapper;

        [Autowired]
        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        public async Task<string> AddUserAsync(UserViewModel model)
        {
            using (var tran = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    var user = _mapper.Map<SystemUser>(model);
                    var isSuccess = await _userRepository.AddUserAsync(user);
                    if (!isSuccess)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "无法创建用户，请检查用户名是否相同！";
                    }
                    isSuccess = await _userRepository.SetPasswordAsync(user, model.Password);
                    if (!isSuccess)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "无法创建用户，初始密码创建失败！";
                    }
                    isSuccess = await _userRepository.AddUserToRoles(user,
                        model.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries));
                    if (!isSuccess)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "无法创建用户，设置角色失败！";
                    }

                    // 更新部门信息
                    await _userRepository.UpdateUserClaimsAsync(user, CustomClaimTypes.UserDepartment,
                        new List<string> { model.DepartmentId });

                    // 更新职位信息
                    await _userRepository.UpdateUserClaimsAsync(user, CustomClaimTypes.UserPosition,
                        model.PositionIds.Split(',', StringSplitOptions.RemoveEmptyEntries));

                    await _unitOfWork.CommitAsync(tran);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                }

                return string.Empty;
            }
        }

        public async Task<UserResultModel> GetUserAsync(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            return new UserResultModel
            {
                Avatar = user.Avatar,
                Name = user.Name,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                IsActive = user.IsActive,
                Sex = (int)user.Sex,
                CreatedTime = user.CreatedTime,

                RoleIds = user.RoleIds,
                DepartmentId = (from uc in _userRepository.GetUserClaims()
                                where user.Id == uc.UserId && uc.ClaimType == CustomClaimTypes.UserDepartment
                                select uc.ClaimValue.ToString()).FirstOrDefault(),
                PositionIds = string.Join(',', (from uc in _userRepository.GetUserClaims()
                                                where user.Id == uc.UserId && uc.ClaimType == CustomClaimTypes.UserPosition
                                                select uc.ClaimValue).ToArray()),
            };
        }

        public IEnumerable<DicResultModel> GetUserDic(string name)
        {
            var dic = from user in _userRepository.GetUsers()
                      where user.Name.Contains(name)
                      select new DicResultModel
                      {
                          Key = user.Id.ToString(),
                          Value = user.Name,
                      };
            return dic.Take(10);
        }

        public PagingResultModel<UserResultModel> GetUsers(UserQueryModel query)
        {
            var where = ExpressionExtension.TrueExpression<UserResultModel>()
                .AndIfHaveValue(query.UserName, u => u.UserName.Contains(query.UserName))
                .AndIfHaveValue(query.Name, u => u.Name.Contains(query.Name))
                .AndIfHaveValue(query.PhoneNumber, u => u.PhoneNumber.Contains(query.PhoneNumber));

            var userQuery = _userRepository.GetUsers();
            if (!string.IsNullOrEmpty(query.RoleId))
            {
                var roleid = int.Parse(query.RoleId);
                userQuery = from u in userQuery
                            join ur in _userRepository.GetUserRoles() on u.Id equals ur.UserId
                            where ur.RoleId == roleid
                            select u;
            }

            if (query.Department != null)
            {
                userQuery = from u in userQuery
                            join uc in _userRepository.GetUserClaims() on u.Id equals uc.UserId
                            where uc.ClaimType == CustomClaimTypes.UserDepartment &&
                                    uc.ClaimValue == query.Department.ToString()
                            select u;
            }

            if (query.Position != null)
            {
                userQuery = from u in userQuery
                            join uc in _userRepository.GetUserClaims() on u.Id equals uc.UserId
                            where uc.ClaimType == CustomClaimTypes.UserPosition &&
                                    uc.ClaimValue == query.Position.ToString()
                            select u;
            }

            var resultQuery = from u in userQuery
                              let rquery = from ur in _userRepository.GetUserRoles()
                                           join r in _roleRepository.GetRoles() on ur.RoleId equals r.Id
                                           where ur.UserId == u.Id
                                           select r.Name

                              let pquery = from uc in _userRepository.GetUserClaims()
                                           from pinfo in _positionRepository.Get(false)
                                           where u.Id == uc.UserId && uc.ClaimType == CustomClaimTypes.UserPosition &&
                                                uc.ClaimValue == pinfo.Id.ToString()
                                           select pinfo.Name

                              let dquery = from uc in _userRepository.GetUserClaims()
                                           from dinfo in _departmentRepository.Get(false)
                                           where u.Id == uc.UserId && uc.ClaimType == CustomClaimTypes.UserDepartment &&
                                                uc.ClaimValue == dinfo.Id.ToString()
                                           select dinfo.Name

                              orderby u.Id descending
                              select new UserResultModel
                              {
                                  Avatar = u.Avatar,
                                  Name = u.Name,
                                  UserName = u.UserName,
                                  PhoneNumber = u.PhoneNumber,
                                  Id = u.Id,
                                  IsActive = u.IsActive,
                                  Sex = (int)u.Sex,
                                  CreatedTime = u.CreatedTime,

                                  RoleName = string.Join(',', rquery.ToArray()),
                                  DepartmentName = dquery.FirstOrDefault(),
                                  PositionName = string.Join(',', pquery.ToArray()),
                              };

            var skip = query.Size * (query.Page - 1);
            var users = resultQuery.Where(where).Skip(skip).Take(query.Size).ToArray();
            return new PagingResultModel<UserResultModel>
            {
                Data = users,
                Count = resultQuery.Where(where).Count()
            };
        }

        public async Task<string> RemoveUserAsync(string Id)
        {
            using (var tran = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    var isSuccess = await _userRepository.RemoveUserByIdAsync(Id);
                    if (!isSuccess)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "删除失败！";
                    }
                    var count = await _userRepository.GetUserCountInRoleAsync("超级管理员");
                    if (count == 0)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "可用的超级管理员数量不能为0！";
                    }
                    await _unitOfWork.CommitAsync(tran);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                }
                return string.Empty;
            }
        }

        public async Task<string> RemoveUserByNameAsync(string Name)
        {
            using (var tran = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    var isSuccess = await _userRepository.RemoveUserByNameAsync(Name);
                    if (!isSuccess)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "删除失败！";
                    }
                    var count = await _userRepository.GetUserCountInRoleAsync("超级管理员");
                    if (count == 0)
                    {
                        await _unitOfWork.RollBackAsync(tran);
                        return "可用的超级管理员数量不能为0！";
                    }
                    await _unitOfWork.CommitAsync(tran);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                }

                return string.Empty;
            }
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
            using (var tran = await _unitOfWork.StartTransactionAsync())
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
                            await _unitOfWork.RollBackAsync(tran);
                            return "无法更新用户，请检查用户名是否相同！";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Password))
                            {
                                isSuccess = await _userRepository.ResetPasswordAsync(user, model.Password);
                                if (!isSuccess)
                                {
                                    await _unitOfWork.RollBackAsync(tran);
                                    return "更新密码失败！";
                                }
                            }
                        }

                        isSuccess = await _userRepository.AddUserToRoles(user,
                            model.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries));
                        if (!isSuccess)
                        {
                            await _unitOfWork.RollBackAsync(tran);
                            return "无法更新用户，更新角色失败！";
                        }

                        var count = await _userRepository.GetUserCountInRoleAsync("超级管理员");
                        if (count == 0)
                        {
                            await _unitOfWork.RollBackAsync(tran);
                            return "可用的超级管理员数量不能为0！";
                        }

                        // 更新部门信息
                        await _userRepository.UpdateUserClaimsAsync(user, CustomClaimTypes.UserDepartment,
                            new List<string> { model.DepartmentId });

                        // 更新职位信息
                        await _userRepository.UpdateUserClaimsAsync(user, CustomClaimTypes.UserPosition,
                            model.PositionIds.Split(',', StringSplitOptions.RemoveEmptyEntries));

                        await _unitOfWork.CommitAsync(tran);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                    return "用户更新失败，请练习管理员！";
                }
            }
            return string.Empty;
        }
    }
}
