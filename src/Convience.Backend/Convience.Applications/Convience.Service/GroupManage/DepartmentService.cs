using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.GroupManage;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IDepartmentService
    {
        IQueryable<DepartmentResultModel> GetAllDepartment();

        Task<DepartmentResultModel> GetDepartmentById(int id);

        Task<bool> AddDepartmentAsync(DepartmentViewModel model);

        Task<bool> UpdateDepartmentAsync(DepartmentViewModel model);

        Task<bool> DeleteDepartmentAsync(int id);

        IEnumerable<DicResultModel> GetDepartmentDic(string name);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<DepartmentTree> _departmentTreeRepository;
        private readonly IRepository<SystemUser> _userRepository;
        private readonly IRepository<SystemUserClaim> _userClaimRepository;
        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public DepartmentService(
            ILogger<DepartmentService> logger,
            IRepository<Department> departmentRepository,
            IRepository<DepartmentTree> departmentTreeRepository,
            IRepository<SystemUser> userRepository,
            IRepository<SystemUserClaim> userClaimRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _departmentTreeRepository = departmentTreeRepository;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddDepartmentAsync(DepartmentViewModel model)
        {
            using var tran = await _unitOfWork.StartTransactionAsync();

            var department = _mapper.Map<Department>(model);
            var entity = await _departmentRepository.AddAsync(department);
            await _unitOfWork.SaveAsync();

            if (!string.IsNullOrWhiteSpace(model.UpId))
            {
                var tree = _departmentTreeRepository.Get(dt => dt.Descendant == int.Parse(model.UpId));
                foreach (var node in tree)
                {
                    await _departmentTreeRepository.AddAsync(new DepartmentTree
                    {
                        Ancestor = node.Ancestor,
                        Descendant = entity.Id,
                        Length = node.Length + 1
                    });
                }
            }

            await _departmentTreeRepository.AddAsync(new DepartmentTree
            {
                Ancestor = entity.Id,
                Descendant = entity.Id,
                Length = 0
            });
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync(tran);
            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            using var tran = await _unitOfWork.StartTransactionAsync();

            // 删除用户相关的部门信息
            await _userClaimRepository.RemoveAsync(uc => uc.ClaimType == CustomClaimTypes.UserDepartment &&
                uc.ClaimValue == id.ToString());

            var childId = _departmentTreeRepository.Get(dt => dt.Ancestor == id)
                .Select(dt => dt.Descendant);
            await _departmentRepository.RemoveAsync(d => childId.Contains(d.Id));
            await _departmentTreeRepository.RemoveAsync(
                dt => childId.Contains(dt.Ancestor) || childId.Contains(dt.Descendant));
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync(tran);
            return true;

        }

        public IQueryable<DepartmentResultModel> GetAllDepartment()
        {
            var query = from d in _departmentRepository.Get()

                        join dt in _departmentTreeRepository.Get()
                        on new { id = d.Id, len = 1 } equals new { id = dt.Descendant, len = dt.Length }
                        into newdt
                        from dt in newdt.DefaultIfEmpty()

                        join u in _userRepository.Get()
                        on d.LeaderId equals u.Id into newu
                        from u in newu.DefaultIfEmpty()

                        orderby d.Sort
                        select new DepartmentResultModel
                        {
                            Id = d.Id,
                            UpId = dt.Ancestor.ToString(),
                            Email = d.Email,
                            LeaderId = d.LeaderId,
                            LeaderName = u.Name,
                            Name = d.Name,
                            Sort = d.Sort,
                            Telephone = d.Telephone
                        };
            return query;
        }

        public async Task<DepartmentResultModel> GetDepartmentById(int id)
        {
            var result = await _departmentRepository.GetAsync(id);
            return _mapper.Map<DepartmentResultModel>(result);
        }

        public IEnumerable<DicResultModel> GetDepartmentDic(string name)
        {
            var dic = from department in _departmentRepository.Get()
                      where department.Name.Contains(name)
                      select new DicResultModel
                      {
                          Key = department.Id.ToString(),
                          Value = department.Name,
                      };
            return dic.Take(10);
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentViewModel model)
        {
            var department = _mapper.Map<Department>(model);
            _departmentRepository.Update(department);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
