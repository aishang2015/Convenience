using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Jwtauthentication;
using Convience.Model.Models.SystemManage;
using Convience.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<MenuTree> _menuTreeRepository;
        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        public RoleService(IRoleRepository roleRepository, IMapper mapper,
            IRepository<MenuTree> menuTreeRepository,
            SystemIdentityDbContext systemIdentityDbContext)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _menuTreeRepository = menuTreeRepository;
            _systemIdentityDbContext = systemIdentityDbContext;
        }

        public async Task<string> AddRole(RoleViewModel model)
        {
            using (var trans = await _systemIdentityDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var role = _mapper.Map<SystemRole>(model);
                    var isSuccess = await _roleRepository.AddRole(role);
                    if (!isSuccess)
                    {
                        return "无法创建角色，请检查角色名是否相同！";
                    }

                    // 找到所有父和子节点
                    var menuIdArray = model.Menus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var menuTree = from tree in _menuTreeRepository.Get(false)
                                   where menuIdArray.Contains(tree.Ancestor.ToString())
                                        || menuIdArray.Contains(tree.Descendant.ToString())
                                   select tree;

                    var menuIds = menuTree.Select(tree => tree.Descendant).Concat(
                            menuTree.Select(tree => tree.Ancestor)).Distinct();

                    role = await _roleRepository.GetRole(model.Name);
                    isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                        CustomClaimTypes.RoleMenus, string.Join(',', menuIds.Distinct()));
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法创建角色，菜单权限添加失败！";
                    }

                    // 存储原始选中节点前端用
                    isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                        CustomClaimTypes.RoleMenusFront, string.Join(',', menuIdArray.Distinct()));
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法更新角色，菜单权限添加失败！";
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
            return _roleRepository.GetRoles().Count();
        }

        public async Task<RoleResult> GetRole(string id)
        {
            var role = await _roleRepository.GetRoleById(id);
            var roleResult = _mapper.Map<RoleResult>(role);
            var menus = from roleclaim in _systemIdentityDbContext.RoleClaims
                        where roleclaim.RoleId == role.Id
                            && roleclaim.ClaimType == CustomClaimTypes.RoleMenusFront
                        select roleclaim.ClaimValue;
            roleResult.Menus = menus.FirstOrDefault();
            return roleResult;
        }

        public IEnumerable<RoleResult> GetRoles(int page, int size, string name)
        {
            var roles = string.IsNullOrEmpty(name) ?
                _roleRepository.GetRoles(page, size).ToArray() :
                _roleRepository.GetRoles(role => role.Name.Contains(name), page, size).ToArray();
            return _mapper.Map<SystemRole[], IEnumerable<RoleResult>>(roles);
        }

        public IEnumerable<RoleResult> GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            return _mapper.Map<SystemRole[], IEnumerable<RoleResult>>(roles.ToArray());
        }

        public async Task<string> RemoveRole(string roleName)
        {
            var role = await _roleRepository.GetRole(roleName);
            if (role == null)
            {
                return "无法删除角色，角色不存在！";
            }
            else if (role.Name == "超级管理员")
            {
                return "系统超级管理员,不可删除修改!";
            }
            var count = await _roleRepository.GetMemberCount(roleName);
            if (count > 0)
            {
                return "无法删除角色，角色中包含用户！";
            }
            var isSuccess = await _roleRepository.RemoveRole(roleName);
            if (!isSuccess)
            {
                return "角色删除失败！";
            }
            return string.Empty;
        }

        public async Task<string> Update(RoleViewModel model)
        {
            using (var trans = await _systemIdentityDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var role = await _roleRepository.GetRoleById(model.Id);
                    _mapper.Map(model, role);
                    var isSuccess = await _roleRepository.UpdateRole(role);
                    if (!isSuccess)
                    {
                        return "无法更新角色，请检查角色名是否相同！";
                    }

                    // 找到所有父和子节点
                    var menuIdArray = model.Menus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var menuTree = from tree in _menuTreeRepository.Get(false)
                                   where menuIdArray.Contains(tree.Ancestor.ToString())
                                        || menuIdArray.Contains(tree.Descendant.ToString())
                                   select tree;

                    var menuIds = menuTree.Select(tree => tree.Descendant).Concat(
                            menuTree.Select(tree => tree.Ancestor)).Distinct();

                    role = await _roleRepository.GetRole(model.Name);
                    isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                        CustomClaimTypes.RoleMenus, string.Join(',', menuIds.Distinct()));
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法更新角色，菜单权限添加失败！";
                    }

                    // 存储原始选中节点前端用
                    isSuccess = await _roleRepository.AddOrUpdateRoleClaim(role,
                        CustomClaimTypes.RoleMenusFront, string.Join(',', menuIdArray.Distinct()));
                    if (!isSuccess)
                    {
                        await trans.RollbackAsync();
                        return "无法更新角色，菜单权限添加失败！";
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

        public IEnumerable<string> GetRoleClaimValue(string[] roleIds, string claimType)
        {
            var result = from rc in _systemIdentityDbContext.RoleClaims
                         where rc.ClaimType == claimType && roleIds.Contains(rc.RoleId.ToString())
                         select rc.ClaimValue;
            return string.Join(',', result).Split(',');
        }

    }
}
