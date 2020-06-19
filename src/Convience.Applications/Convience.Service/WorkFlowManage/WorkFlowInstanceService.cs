using AutoMapper;
using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.WorkFlowManage;
using DnsClient.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowInstanceService
    {
        /// <summary>
        /// 创建工作流实例
        /// </summary>
        Task<bool> CreateWorkFlowInstance(int workflowId, string account, string userName);

        /// <summary>
        /// 用户发起的工作流实例
        /// </summary>
        (IEnumerable<WorkFlowInstanceResult>, int) GetInstanceList(string account, int page, int size);

        /// <summary>
        /// 需要用户处理的的工作流实例
        /// </summary>
        (IEnumerable<WorkFlowInstanceResult>, int) GetHandledInstanceList();

        /// <summary>
        /// 取得工作流内容
        /// </summary>
        IEnumerable<WorkFlowInstanceValueResult> GetWorkFlowInstance(int workFlowInstanceId);

        /// <summary>
        /// 保存工作流内容
        /// </summary>
        Task<bool> SaveWorkFlowInstance(int workFlowInstanceId, IEnumerable<WorkFlowInstanceValueViewModel> values);

        /// <summary>
        /// 提交，流转
        /// </summary>
        Task SubmitWorkFlowInstance(int WorkFlowInstanceId, WorkFlowInstanceHandleViewModel viewModel);
    }

    public class WorkFlowInstanceService : IWorkFlowInstanceService
    {
        private readonly ILogger<WorkFlowInstanceService> _logger;

        private readonly IRepository<WorkFlow> _workflowRepository;

        private readonly IRepository<WorkFlowNode> _nodeRepository;

        private readonly IRepository<WorkFlowInstance> _instanceRepository;

        private readonly IRepository<WorkFlowInstanceRoute> _instanceRouteRepository;

        private readonly IRepository<WorkFlowInstanceValue> _instanceValueRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public WorkFlowInstanceService(
            ILogger<WorkFlowInstanceService> logger,
            IRepository<WorkFlow> workflowRepository,
            IRepository<WorkFlowNode> nodeRepository,
            IRepository<WorkFlowInstance> instanceRepository,
            IRepository<WorkFlowInstanceRoute> instanceRouteRepository,
            IRepository<WorkFlowInstanceValue> instanceValueRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _workflowRepository = workflowRepository;
            _nodeRepository = nodeRepository;
            _instanceRepository = instanceRepository;
            _instanceRouteRepository = instanceRouteRepository;
            _instanceValueRepository = instanceValueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateWorkFlowInstance(int workflowId, string account, string userName)
        {
            // 获取开始节点id
            var startNode = await _nodeRepository.Get(n => n.WorkFlowId == workflowId && n.NodeType == NodeTypeEnum.StartNode)
                .FirstOrDefaultAsync();

            // 工作流定义
            var workflow = await _workflowRepository.GetAsync(workflowId);

            // 创建工作流实例
            var instance = new WorkFlowInstance()
            {
                WorkFlowInstanceState = WorkFlowInstanceStateEnum.NoCommitted,
                CurrentNodeId = startNode.Id,
                CreatedTime = DateTime.Now,
                CreatedUserAccount = account,
                CreatedUserName = userName,
                WorkFlowName = workflow.Name,
                WorkFlowId = workflowId,
            };

            // 添加节点处理记录
            var routeInfo = new WorkFlowInstanceRoute
            {
                NodeId = startNode.Id,
                NodeName = startNode.Name,
                HandlePeople = userName,
                IsPass = true,
                HandleInfo = "发起工作流",
                HandleTime = DateTime.Now
            };

            try
            {
                var entity = await _instanceRepository.AddAsync(instance);
                await _unitOfWork.SaveAsync();

                routeInfo.WorkFlowInstanceId = entity.Id;
                await _instanceRouteRepository.AddAsync(routeInfo);
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

        public (IEnumerable<WorkFlowInstanceResult>, int) GetInstanceList(string account, int page, int size)
        {
            var query = _instanceRepository.Get(i => i.CreatedUserAccount == account);
            var result = query.OrderByDescending(i => i.CreatedTime).Skip((page - 1) * size).Take(size);
            return (_mapper.Map<IEnumerable<WorkFlowInstanceResult>>(result.ToArray()), query.Count());
        }

        public (IEnumerable<WorkFlowInstanceResult>, int) GetHandledInstanceList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkFlowInstanceValueResult> GetWorkFlowInstance(int workFlowInstanceId)
        {
            var result = _instanceValueRepository.Get(v => v.WorkFlowInstanceId == workFlowInstanceId);
            return _mapper.Map<IEnumerable<WorkFlowInstanceValueResult>>(result);
        }

        public async Task<bool> SaveWorkFlowInstance(int workFlowInstanceId, IEnumerable<WorkFlowInstanceValueViewModel> values)
        {
            foreach (var model in values)
            {
                model.WorkFlowInstanceId = workFlowInstanceId;
            }

            try
            {
                await _instanceValueRepository.RemoveAsync(v => v.WorkFlowInstanceId == workFlowInstanceId);
                _mapper.Map<List<WorkFlowInstanceValue>>(values).ForEach(async value =>
                {
                    await _instanceValueRepository.AddAsync(value);
                });
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

        public Task SubmitWorkFlowInstance(int WorkFlowInstanceId, WorkFlowInstanceHandleViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
