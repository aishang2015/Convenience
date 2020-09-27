using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.WorkFlowManage;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowGroupService
    {
        IQueryable<WorkFlowGroupResultModel> GetAllWorkFlowGroup();

        Task<WorkFlowGroupResultModel> GetWorkFlowGroupById(int id);

        Task<bool> AddWorkFlowGroupAsync(WorkFlowGroupViewModel model);

        Task<bool> UpdateWorkFlowGroupAsync(WorkFlowGroupViewModel model);

        Task<bool> DeleteWorkFlowGroupAsync(int id);
    }

    public class WorkFlowGroupService : IWorkFlowGroupService
    {
        private readonly ILogger<WorkFlowGroupService> _logger;

        private readonly IRepository<WorkFlowGroup> _workFlowGroupRepository;

        private readonly IRepository<WorkFlowGroupTree> _workFlowGroupTreeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public WorkFlowGroupService(
            ILogger<WorkFlowGroupService> logger,
            IRepository<WorkFlowGroup> workFlowGroupRepository,
            IRepository<WorkFlowGroupTree> workFlowGroupTreeRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _workFlowGroupRepository = workFlowGroupRepository;
            _workFlowGroupTreeRepository = workFlowGroupTreeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddWorkFlowGroupAsync(WorkFlowGroupViewModel model)
        {
            using (var tran = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    var workflowGroup = _mapper.Map<WorkFlowGroup>(model);
                    var entity = await _workFlowGroupRepository.AddAsync(workflowGroup);
                    await _unitOfWork.SaveAsync();
                    if (!string.IsNullOrWhiteSpace(model.UpId))
                    {
                        var upid = int.Parse(model.UpId);
                        var tree = _workFlowGroupTreeRepository.Get(dt => dt.Descendant == upid);
                        foreach (var node in tree)
                        {
                            await _workFlowGroupTreeRepository.AddAsync(new WorkFlowGroupTree
                            {
                                Ancestor = node.Ancestor,
                                Descendant = entity.Id,
                                Length = node.Length + 1
                            });
                        }
                    }
                    await _workFlowGroupTreeRepository.AddAsync(new WorkFlowGroupTree
                    {
                        Ancestor = entity.Id,
                        Descendant = entity.Id,
                        Length = 0
                    });
                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CommitAsync(tran);
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                }
                return false;
            }
        }

        public async Task<bool> DeleteWorkFlowGroupAsync(int id)
        {
            using (var tran = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    // todo 校验是否包含工作流，如果包含则不能删除
                    var childId = _workFlowGroupTreeRepository.Get(dt => dt.Ancestor == id)
                        .Select(dt => dt.Descendant);
                    await _workFlowGroupRepository.RemoveAsync(d => childId.Contains(d.Id));
                    await _workFlowGroupTreeRepository.RemoveAsync(dt => childId.Contains(dt.Ancestor) || childId.Contains(dt.Descendant));
                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CommitAsync(tran);
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await _unitOfWork.RollBackAsync(tran);
                    return false;
                }
            }
        }

        public IQueryable<WorkFlowGroupResultModel> GetAllWorkFlowGroup()
        {
            var query = from wfg in _workFlowGroupRepository.Get()
                        join tree in _workFlowGroupTreeRepository.Get()
                        on new { id = wfg.Id, length = 1 } equals new { id = tree.Descendant, length = tree.Length }
                        into e
                        from rtree in e.DefaultIfEmpty()
                        orderby wfg.Sort
                        select new WorkFlowGroupResultModel
                        {
                            Id = wfg.Id,
                            UpId = rtree.Ancestor.ToString(),
                            Name = wfg.Name,
                            Sort = wfg.Sort,
                        };
            return query;
        }

        public async Task<WorkFlowGroupResultModel> GetWorkFlowGroupById(int id)
        {
            var column = await _workFlowGroupRepository.GetAsync(id);
            return _mapper.Map<WorkFlowGroupResultModel>(column);
        }

        public async Task<bool> UpdateWorkFlowGroupAsync(WorkFlowGroupViewModel model)
        {
            try
            {
                var column = _mapper.Map<WorkFlowGroup>(model);
                _workFlowGroupRepository.Update(column);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return false;
            }
        }
    }


}
