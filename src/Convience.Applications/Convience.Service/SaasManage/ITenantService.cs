using Convience.Entity.Entity;
using Convience.Model.Models.SaasManage;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.SaasManage
{
    public interface ITenantService
    {
        Task<TenantResult> Get(Guid id);

        IEnumerable<TenantResult> Get(TenantQuery query);

        long Count(TenantQuery query);

        Task<Tenant> AddAsync(TenantViewModel model);

        Task UpdateAsync(TenantViewModel model);

        Task RemoveAsync(Guid id);
    }
}
