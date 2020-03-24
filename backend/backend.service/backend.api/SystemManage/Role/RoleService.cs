using AutoMapper;
using Backend.Entity.backend.api.Data;
using Backend.Model.backend.api.Models.SystemManage;
using Backend.Repository.backend.api;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddRole(RoleViewModel model)
        {
            var role = new SystemRole();
            role.Name = model.Name;
            role.Remark = model.Remark;
            return await _roleRepository.AddRole(role);
        }

        public int Count()
        {
            return _roleRepository.GetRoles().Count();
        }

        public IEnumerable<RoleResult> GetRoles(int page, int size, string name)
        {

            var roles = string.IsNullOrEmpty(name) ?
                _roleRepository.GetRoles(page, size).ToArray() :
                _roleRepository.GetRoles(role => role.Name.Contains(name), page, size).ToArray();
            return _mapper.Map<SystemRole[], IEnumerable<RoleResult>>(roles);
        }

        public IEnumerable<string> GetRoles()
        {
            return _roleRepository.GetRoles().Select(r => r.Name);
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

        public async Task<bool> Update(RoleViewModel model)
        {
            var role = await _roleRepository.GetRole(model.Name);
            role.Name = model.Name;
            role.Remark = model.Remark;
            return await _roleRepository.UpdateRole(role);
        }


    }
}
