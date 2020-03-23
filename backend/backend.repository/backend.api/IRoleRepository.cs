using Backend.Repository.backend.api.Data;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Repository.backend.api
{
    public interface IRoleRepository
    {
        public Task<bool> AddRole(SystemRole role);

        public Task<bool> RemoveRole(string roleName);

        public Task<bool> UpdateRole(SystemRole role);

        public Task<SystemRole> GetRole(string roleName);

        public IQueryable<SystemRole> GetRoles();

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where);

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where, int page, int size);

        public IQueryable<SystemRole> GetRoles(int page, int size);

        Task<int> GetMemberCount(string roleName);
    }
}
