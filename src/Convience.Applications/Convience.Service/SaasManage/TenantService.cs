

using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Infrastructure;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.SaasManage;
using Convience.Util.Extension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.SaasManage
{
    public interface ITenantService
    {
        Task<TenantResultModel> Get(Guid id);

        IEnumerable<TenantResultModel> Get(TenantQueryModel query);

        long Count(TenantQueryModel query);

        Task<Tenant> AddAsync(TenantViewModel model);

        Task UpdateAsync(TenantViewModel model);

        Task RemoveAsync(Guid id);
    }

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

        public long Count(TenantQueryModel query)
        {
            Enum.TryParse(typeof(DataBaseType), query.DataBaseType, out object type);
            Expression<Func<Tenant, bool>> where = ExpressionExtension.TrueExpression<Tenant>()
                .AndIfHaveValue(query.Name, t => t.Name.Contains(query.Name))
                .AndIfHaveValue(query.DataBaseType, t => t.DataBaseType == (DataBaseType)type);

            var queryExpress = _tenantRepository.Get(where);
            return _tenantRepository.CountAsync(queryExpress).Result;
        }

        public IEnumerable<TenantResultModel> Get(TenantQueryModel query)
        {
            Enum.TryParse(typeof(DataBaseType), query.DataBaseType, out object type);
            Expression<Func<Tenant, bool>> where = ExpressionExtension.TrueExpression<Tenant>()
                .AndIfHaveValue(query.Name, t => t.Name.Contains(query.Name))
                .AndIfHaveValue(query.DataBaseType, t => t.DataBaseType == (DataBaseType)type);

            Expression<Func<Tenant, object>> order = query.SortKey switch
            {
                "CreatedTime" => t => t.CreatedTime,
                _ => t => t.CreatedTime,
            };

            var result = _tenantRepository.Get(where, order, query.Page, query.Size, query.isDesc, false);

            return result.Count() > 0 ? _mapper.Map<Tenant[], TenantResultModel[]>(result.ToArray()) :
                new TenantResultModel[] { };
        }

        public async Task<TenantResultModel> Get(Guid id)
        {
            var tenant = await _tenantRepository.GetAsync(id);
            return _mapper.Map<TenantResultModel>(tenant);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _tenantRepository.RemoveAsync(tenant => tenant.Id == id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(TenantViewModel model)
        {
            var tenant = _mapper.Map<Tenant>(model);
            _tenantRepository.Update(tenant);
            await _unitOfWork.SaveAsync();
        }
    }
}
