using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models.WorkFlowManage;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowFlowService
    {
        WorkFlowFlowResultModel GetWorkFlowFlow(int workflowId);

        Task<bool> AddOrUpdateWorkFlowFlow(WorkFlowFlowViewModel viewModel);
    }

    public class WorkFlowFlowService : IWorkFlowFlowService
    {
        private readonly ILogger<WorkFlowFlowService> _logger;

        private readonly IRepository<WorkFlow> _workflowRepository;

        private readonly IRepository<WorkFlowLink> _linkRepository;

        private readonly IRepository<WorkFlowNode> _nodeRepository;

        private readonly IRepository<WorkFlowCondition> _conditionRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private IMapper _mapper;

        public WorkFlowFlowService(ILogger<WorkFlowFlowService> logger,
            IRepository<WorkFlow> workflowRepository,
            IRepository<WorkFlowLink> linkRepository,
            IRepository<WorkFlowNode> nodeRepository,
            IRepository<WorkFlowCondition> conditionRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _logger = logger;
            _workflowRepository = workflowRepository;
            _linkRepository = linkRepository;
            _nodeRepository = nodeRepository;
            _conditionRepository = conditionRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<bool> AddOrUpdateWorkFlowFlow(WorkFlowFlowViewModel viewModel)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User?.GetUserName();

                foreach (var link in viewModel.WorkFlowLinkViewModels)
                {
                    link.WorkFlowId = viewModel.WorkFlowId;
                }
                foreach (var node in viewModel.WorkFlowNodeViewModels)
                {
                    node.WorkFlowId = viewModel.WorkFlowId;
                }
                foreach (var condition in viewModel.WorkFlowConditionViewModels)
                {
                    condition.WorkFlowId = viewModel.WorkFlowId;
                }
                var workflow = await _workflowRepository.GetAsync(viewModel.WorkFlowId);
                workflow.UpdatedTime = DateTime.Now;
                workflow.UpdatedUser = userName;

                await _linkRepository.RemoveAsync(f => f.WorkFlowId == viewModel.WorkFlowId);
                await _linkRepository.AddAsync(_mapper.Map<IEnumerable<WorkFlowLink>>(viewModel.WorkFlowLinkViewModels));
                await _nodeRepository.RemoveAsync(f => f.WorkFlowId == viewModel.WorkFlowId);
                await _nodeRepository.AddAsync(_mapper.Map<IEnumerable<WorkFlowNode>>(viewModel.WorkFlowNodeViewModels));
                await _conditionRepository.RemoveAsync(f => f.WorkFlowId == viewModel.WorkFlowId);
                await _conditionRepository.AddAsync(_mapper.Map<IEnumerable<WorkFlowCondition>>(viewModel.WorkFlowConditionViewModels));
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return false;
            }
        }

        public WorkFlowFlowResultModel GetWorkFlowFlow(int workflowId)
        {
            var links = _linkRepository.Get(f => f.WorkFlowId == workflowId).ToArray();
            var nodes = _nodeRepository.Get(f => f.WorkFlowId == workflowId).ToArray();
            var conditions = _conditionRepository.Get(f => f.WorkFlowId == workflowId).ToArray();
            return new WorkFlowFlowResultModel
            {
                WorkFlowLinkResults = _mapper.Map<IEnumerable<WorkFlowLinkResultModel>>(links),
                WorkFlowNodeResults = _mapper.Map<IEnumerable<WorkFlowNodeResultModel>>(nodes),
                WorkFlowConditionResults = _mapper.Map<IEnumerable<WorkFlowConditionResultModel>>(conditions)
            };
        }
    }
}
