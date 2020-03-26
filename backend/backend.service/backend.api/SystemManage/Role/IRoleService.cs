using Backend.Model.backend.api.Models.SystemManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.Role
{
    public interface IRoleService
    {
        IEnumerable<RoleResult> GetRoles(int page, int size, string name);

        Task<RoleResult> GetRole(string id);

        IEnumerable<RoleResult> GetRoles();

        Task<string> RemoveRole(string roleName);

        Task<string> AddRole(RoleViewModel model);

        Task<string> Update(RoleViewModel model);

        int Count();

        IEnumerable<string> GetRoleClaimValue(string[] roleIds, string claimType);

        IEnumerable<string> GetRoleClaimValueByName(string[] roleNames, string claimType);
    }
}
