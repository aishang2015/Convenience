using Backend.Model.backend.api.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.Role
{
    public interface IRoleService
    {
        IEnumerable<RoleResult> GetRoles(int page, int size, string name);

        Task<bool> RemoveRole(string roleName);

        Task<bool> AddRole(RoleViewModel model);

        Task Update(RoleViewModel model);

        int Count();
    }
}
