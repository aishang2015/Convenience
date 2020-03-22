using Backend.Model.backend.api.Models.SystemManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.Role
{
    public interface IRoleService
    {
        IEnumerable<RoleResult> GetRoles(int page, int size, string name);

        IEnumerable<string> GetRoles();

        Task<bool> RemoveRole(string roleName);

        Task<bool> AddRole(RoleViewModel model);

        Task<bool> Update(RoleViewModel model);

        int Count();
    }
}
