

using AutoMapper;
using backend.data.Infrastructure;
using backend.data.Repositories;
using backend.util.helpers;
using Backend.Entity.backend.api.Data;
using Backend.Entity.backend.api.Entity;
using Backend.Model.backend.api.Models.SaasManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SaasManage
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public TenantService(IRepository<Tenant> tenantRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Tenant> AddAsync(TenantViewModel model)
        {
            var tenant = _mapper.Map<Tenant>(model);
            tenant.CreatedTime = DateTime.Now;
            var result = await _tenantRepository.AddAsync(tenant);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public long Count(TenantQuery query)
        {
            Expression<Func<Tenant, bool>> where = t => (
                   (string.IsNullOrEmpty(query.Name) || t.Name.Contains(query.Name))
                   && (string.IsNullOrEmpty(query.DataBaseType) || t.DataBaseType == (DataBaseType)Enum.Parse(typeof(DataBaseType), query.DataBaseType))
               );
            var queryExpress = _tenantRepository.Get(where);
            return _tenantRepository.CountAsync(queryExpress).Result;
        }

        public IEnumerable<TenantResult> Get(TenantQuery query)
        {
            Expression<Func<Tenant, bool>> where = t => (
                   (string.IsNullOrEmpty(query.Name) || t.Name.Contains(query.Name))
                   && (string.IsNullOrEmpty(query.DataBaseType) || t.DataBaseType == (DataBaseType)Enum.Parse(typeof(DataBaseType), query.DataBaseType))
               );

            Expression<Func<Tenant, object>> order = query.SortKey switch
            {
                "CreatedTime" => t => t.CreatedTime,
                _ => t => t.CreatedTime,
            };

            var result = _tenantRepository.Get(where, order, query.Page, query.Size, query.isDesc, false);

            return result.Count() > 0 ? _mapper.Map<Tenant[], TenantResult[]>(result.ToArray()) :
                new TenantResult[] { };
        }

        public async Task<TenantResult> Get(Guid id)
        {
            var tenant = await _tenantRepository.GetAsync(id);
            return _mapper.Map<TenantResult>(tenant);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _tenantRepository.RemoveAsync(tenant => tenant.Id == id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(TenantViewModel model)
        {
            var tenant = await _tenantRepository.GetAsync(model.Id);
            _mapper.Map(model, tenant);
            _tenantRepository.Update(tenant);
            await _unitOfWork.SaveAsync();
        }
    }
}
