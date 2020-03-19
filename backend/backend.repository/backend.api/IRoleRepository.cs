using backend.entity.backend.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository.backend.api
{
    public interface IRoleRepository
    {
        public Task<bool> AddRole(SystemRole role);

        public Task<bool> RemoveRole(string roleName);

        public Task UpdateRole(SystemRole role);

        public Task<SystemRole> GetRole(string roleName);

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where);

        public IQueryable<SystemRole> GetRoles(Expression<Func<SystemRole, bool>> where, int page, int size);

        public IQueryable<SystemRole> GetRoles(int page, int size);
    }
}
