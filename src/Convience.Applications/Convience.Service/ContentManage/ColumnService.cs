using AutoMapper;
using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.ContentManage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public class ColumnService : IColumnService
    {
        private readonly IRepository<Column> _columnRepository;

        private readonly IRepository<ColumnTree> _columnTreeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public ColumnService(IRepository<Column> columnRepository,
            IRepository<ColumnTree> columnTreeRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _columnRepository = columnRepository;
            _columnTreeRepository = columnTreeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddColumnAsync(ColumnViewModel model)
        {
            await _unitOfWork.StartTransactionAsync();
            try
            {
                var column = _mapper.Map<Column>(model);
                var entity = await _columnRepository.AddAsync(column);
                await _unitOfWork.SaveAsync();

                if (!string.IsNullOrEmpty(model.UpId))
                {
                    var upid = int.Parse(model.UpId);
                    var tree = _columnTreeRepository.Get(t => t.Descendant == upid);

                    // 做成树数据
                    foreach (var m in tree)
                    {
                        await _columnTreeRepository.AddAsync(new ColumnTree
                        {
                            Ancestor = m.Ancestor,
                            Descendant = entity.Id,
                            Length = m.Length + 1
                        });
                    }
                }
                await _columnTreeRepository.AddAsync(new ColumnTree
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

        public async Task<bool> DeleteColumnAsync(int id)
        {
            try
            {
                var descendantIds = _columnTreeRepository.Get(tree => tree.Ancestor == id)
                    .Select(tree => tree.Descendant);
                await _columnRepository.RemoveAsync(column => descendantIds.Contains(column.Id));
                await _columnTreeRepository.RemoveAsync(tree => descendantIds.Contains(tree.Ancestor) || descendantIds.Contains(tree.Descendant));
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<ColumnResult> GetAllColumn()
        {
            var query = from column in _columnRepository.Get()
                        join tree in _columnTreeRepository.Get()
                        on new { id = column.Id, length = 1 } equals new { id = tree.Descendant, length = tree.Length }
                        into e
                        from rtree in e.DefaultIfEmpty()
                        orderby column.Sort
                        select new ColumnResult
                        {
                            Id = column.Id,
                            UpId = rtree.Ancestor.ToString(),
                            Name = column.Name,
                            Sort = column.Sort,
                        };
            return query;
        }

        public async Task<ColumnResult> GetByIdAsync(int id)
        {
            var column = await _columnRepository.GetAsync(id);
            return _mapper.Map<ColumnResult>(column);
        }

        public async Task<bool> UpdateColumnAsync(ColumnViewModel model)
        {
            try
            {
                var column = await _columnRepository.GetAsync(model.Id);
                _mapper.Map(model, column);
                _columnRepository.Update(column);
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
