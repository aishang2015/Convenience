using AutoMapper;

using Backend.Model.backend.api.Models.SystemManage;
using Backend.Repository.backend.api;
using Backend.Repository.backend.api.Data;

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
            role.Number = 0;
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

        public async Task<bool> RemoveRole(string roleName)
        {
            var role = await _roleRepository.GetRole(roleName);
            if (role != null && role.Number > 0)
            {
                return false;
            }
            return await _roleRepository.RemoveRole(roleName);
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
