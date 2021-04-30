using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Constants.SystemManage;
using Convience.Model.Models;
using Convience.Model.Models.SystemManage;
using Convience.Util.Extension;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{

    public interface IRoleService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        Task<string> AddRole(RoleViewModel model);

        /// <summary>
        /// 获取角色数据
        /// </summary>
        Task<RoleResultModel> GetRoleAsync(string id);

        /// <summary>
        /// 角色列表(分页)
        /// </summary>
        PagingResultModel<RoleResultModel> GetRoles(int page, int size, string name);

        /// <summary>
        /// 取得全部角色
        /// </summary>
        IEnumerable<RoleResultModel> GetRoles();

        /// <summary>
        /// 删除角色
        /// </summary>
        Task<string> RemoveRoleAsync(string roleName);

        /// <summary>
        /// 更新角色
        /// </summary>
        Task<string> UpdateAsync(RoleViewModel model);

        /// <summary>
        /// 获取角色关联的claim
        /// </summary>
        List<string> GetRoleClaimValue(string[] roleIds, string claimType);
    }

    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<MenuTree> _menuTreeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly SystemIdentityDbContext _systemIdentityDbContext;
        private readonly SystemIdentityDbUnitOfWork _systemIdentityDbUnitOfWork;
        private readonly RoleManager<SystemRole> _roleManager;

        public RoleService(
            IMapper mapper,
            IRepository<MenuTree> menuTreeRepository,
            IRoleRepository roleRepository,
            SystemIdentityDbContext systemIdentityDbContext,
            SystemIdentityDbUnitOfWork systemIdentityDbUnitOfWork,
            RoleManager<SystemRole> roleManager)
        {
            _mapper = mapper;
            _menuTreeRepository = menuTreeRepository;
            _roleRepository = roleRepository;
            _systemIdentityDbContext = systemIdentityDbContext;
            _systemIdentityDbUnitOfWork = systemIdentityDbUnitOfWork;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        public async Task<string> AddRole(RoleViewModel model)
        {
            // 开启事务
            using var trans = await _systemIdentityDbUnitOfWork.StartTransactionAsync();

            // 创建角色
            var createResult = await _roleManager.CreateAsync(_mapper.Map<SystemRole>(model));
            if (!createResult.Succeeded)
            {
                return RoleConstants.ROLE_NAME_SAME;
            }

            // 找到菜单树所有父和子节点
            var menuIdArray = model.Menus.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var menuTree = (from tree in _menuTreeRepository.Get(false)
                            where menuIdArray.Contains(tree.Ancestor.ToString())
                                 || menuIdArray.Contains(tree.Descendant.ToString())
                            select tree).ToList();

            // 所有关联菜单的ID
            var menuIds = menuTree.Select(tree => tree.Descendant).Concat(
                    menuTree.Select(tree => tree.Ancestor)).Distinct();

            // 找到刚创建的角色
            var role = await _roleManager.FindByNameAsync(model.Name);

            // 关联角色和菜单
            var isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                CustomClaimTypes.RoleMenus, string.Join(',', menuIds.Distinct()));
            if (!isSuccess)
            {
                await trans.RollbackAsync();
                return RoleConstants.ROLE_RIGHT_FAIL;
            }

            // 关联角色和菜单 这个主要是为了前端设置界面用
            isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                CustomClaimTypes.RoleMenusFront, string.Join(',', menuIdArray.Distinct()));
            if (!isSuccess)
            {
                await trans.RollbackAsync();
                return RoleConstants.ROLE_RIGHT_FAIL;
            }

            await trans.CommitAsync();
            return string.Empty;
        }

        /// <summary>
        /// 获取角色数据
        /// </summary>
        public async Task<RoleResultModel> GetRoleAsync(string id)
        {
            var role = await _roleRepository.GetRoleById(id);
            var menus = from roleclaim in _systemIdentityDbContext.RoleClaims
                        where roleclaim.RoleId == role.Id
                            && roleclaim.ClaimType == CustomClaimTypes.RoleMenusFront
                        select roleclaim.ClaimValue;

            var roleResult = _mapper.Map<RoleResultModel>(role) with
            {
                Menus = menus.FirstOrDefault()
            };
            return roleResult;
        }

        /// <summary>
        /// 角色列表(分页)
        /// </summary>
        public PagingResultModel<RoleResultModel> GetRoles(int page, int size, string name)
        {
            var query = _roleRepository.GetRoles()
                .AndIfHaveValue(name, r => r.Name.Contains(name));
            var roles = query.Skip(size * (page - 1)).Take(size);
            return new PagingResultModel<RoleResultModel>
            {
                Data = _mapper.Map<List<RoleResultModel>>(roles),
                Count = query.Count()
            };
        }

        /// <summary>
        /// 取得全部角色
        /// </summary>
        public IEnumerable<RoleResultModel> GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            return _mapper.Map<List<RoleResultModel>>(roles.ToArray());
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task<string> RemoveRoleAsync(string roleName)
        {
            var role = await _roleRepository.GetRole(roleName);
            if (role == null)
            {
                return RoleConstants.ROLE_NO_EXIST;
            }
            else if (role.Name == "超级管理员")
            {
                return RoleConstants.ROLE_SYSTEM_CANNOT_MODIFY;
            }
            var count = await _roleRepository.GetMemberCount(roleName);
            if (count > 0)
            {
                return RoleConstants.ROLE_CONTAIN_USER;
            }
            var isSuccess = await _roleRepository.RemoveRole(roleName);
            if (!isSuccess)
            {
                return RoleConstants.ROLE_REMOVE_FAIL;
            }
            return string.Empty;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        public async Task<string> UpdateAsync(RoleViewModel model)
        {
            // 开始事务
            using var trans = await _systemIdentityDbContext.Database.BeginTransactionAsync();

            // 取得角色并更新
            var role = await _roleRepository.GetRoleById(model.Id);
            _mapper.Map(model, role);
            var isSuccess = await _roleRepository.UpdateRole(role);
            if (!isSuccess)
            {
                return RoleConstants.ROLE_UPDATE_NAME_SAME;
            }

            // 找到菜单树所有父和子节点
            var menuIdArray = model.Menus.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var menuTree = (from tree in _menuTreeRepository.Get(false)
                            where menuIdArray.Contains(tree.Ancestor.ToString())
                                 || menuIdArray.Contains(tree.Descendant.ToString())
                            select tree).ToList();

            // 所有关联菜单的ID
            var menuIds = menuTree.Select(tree => tree.Descendant).Concat(
                    menuTree.Select(tree => tree.Ancestor)).Distinct();

            // 关联角色和菜单
            isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                CustomClaimTypes.RoleMenus, string.Join(',', menuIds.Distinct()));
            if (!isSuccess)
            {
                await trans.RollbackAsync();
                return RoleConstants.ROLE_UPDATE_RIGHT_FAIL;
            }

            // 关联角色和菜单 这个主要是为了前端设置界面用
            isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                CustomClaimTypes.RoleMenusFront, string.Join(',', menuIdArray.Distinct()));
            if (!isSuccess)
            {
                await trans.RollbackAsync();
                return RoleConstants.ROLE_UPDATE_RIGHT_FAIL;
            }
            await trans.CommitAsync();
            return string.Empty;
        }

        /// <summary>
        /// 获取角色关联的claim
        /// </summary>
        public List<string> GetRoleClaimValue(string[] roleIds, string claimType)
        {
            return (from rc in _systemIdentityDbContext.RoleClaims
                    where rc.ClaimType == claimType && roleIds.Contains(rc.RoleId.ToString())
                    select rc.ClaimValue).Distinct().ToList();
        }
    }
}
