
using Backend.Entity.backend.api.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Repository.backend.api
{
    public interface IRoleRepository
    {
        Task<bool> AddRole(SystemRole role);

        Task<bool> RemoveRole(string roleName);

        Task<bool> UpdateRole(SystemRole role);

        Task<SystemRole> GetRole(string roleName);

        Task<SystemRole> GetRoleById(string id);

        IQueryable<SystemRole> GetRoles();

        IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where);

        IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where, int page, int size);

        IQueryable<SystemRole> GetRoles(int page, int size);

        Task<int> GetMemberCount(string roleName);

        Task<bool> AddOrUpdateRoleClaim(SystemRole role, string claimType, string claimValue);
    }
}
