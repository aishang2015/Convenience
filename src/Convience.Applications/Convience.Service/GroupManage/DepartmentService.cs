using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.GroupManage;
using Convience.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;

        private readonly IRepository<DepartmentTree> _departmentTreeRepository;

        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public DepartmentService(IRepository<Department> departmentRepository,
            IRepository<DepartmentTree> departmentTreeRepository,
            IUserRepository userRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _departmentTreeRepository = departmentTreeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddDepartmentAsync(DepartmentViewModel model)
        {
            await _unitOfWork.StartTransactionAsync();
            try
            {
                var department = _mapper.Map<Department>(model);
                var entity = await _departmentRepository.AddAsync(department);
                await _unitOfWork.SaveAsync();
                if (!string.IsNullOrWhiteSpace(model.UpId))
                {
                    var upid = int.Parse(model.UpId);
                    var tree = _departmentTreeRepository.Get(dt => dt.Descendant == upid);
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
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
            }
            return false;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var childId = _departmentTreeRepository.Get(dt => dt.Ancestor == id)
                    .Select(dt => dt.Descendant);
                await _departmentRepository.RemoveAsync(d => childId.Contains(d.Id));
                await _departmentTreeRepository.RemoveAsync(
                    dt => childId.Contains(dt.Ancestor) || childId.Contains(dt.Descendant));
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<DepartmentResult> GetAllDepartment()
        {
            var query = from d in _departmentRepository.Get()

                        join dt in _departmentTreeRepository.Get()
                        on new { id = d.Id, len = 1 } equals new { id = dt.Descendant, len = dt.Length }
                        into newdt
                        from dt in newdt.DefaultIfEmpty()

                        join u in _userRepository.GetUsers()
                        on d.LeaderId equals u.Id into newu
                        from u in newu.DefaultIfEmpty()

                        orderby d.Sort
                        select new DepartmentResult
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

        public async Task<DepartmentResult> GetDepartmentById(int id)
        {
            var result =  await _departmentRepository.GetAsync(id);
            return _mapper.Map<DepartmentResult>(result);
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentViewModel model)
        {
            try
            {
                var department = await _departmentRepository.GetAsync(model.Id);
                _mapper.Map(model, department);
                _departmentRepository.Update(department);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
