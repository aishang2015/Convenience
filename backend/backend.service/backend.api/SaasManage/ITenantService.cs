using Backend.Entity.backend.api.Entity;
using Backend.Model.backend.api.Models.SaasManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SaasManage
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
