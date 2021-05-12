using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.Entity.Entity.Identity;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.WorkFlowManage;

using Microsoft.AspNetCore.Http;
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
        Task<bool> CreateWorkFlowInstance(int workflowId);

        /// <summary>
        /// 用户发起的工作流实例
        /// </summary>
        PagingResultModel<WorkFlowInstanceResultModel> GetInstanceList(int page, int size);

        /// <summary>
        /// 需要用户处理的的工作流实例
        /// </summary>
        PagingResultModel<WorkFlowInstanceResultModel> GetHandledInstanceList(int page, int size);

        /// <summary>
        /// 取得工作流内容
        /// </summary>
        IEnumerable<WorkFlowInstanceValueResultModel> GetWorkFlowInstanceValues(int workFlowInstanceId);

        /// <summary>
        /// 取得工作流处理情况
        /// </summary>
        /// <param name="workFlowInstanceId"></param>
        /// <returns></returns>
        IEnumerable<WorkflowinstanceRouteResultModel> GetWorkFlowInstanceRoutes(int workFlowInstanceId);

        /// <summary>
        /// 保存工作流内容
        /// </summary>
        Task<bool> SaveWorkFlowInstanceValues(int workFlowInstanceId, IEnumerable<WorkFlowInstanceValueViewModel> values);

        /// <summary>
        /// 提交
        /// </summary>
        Task<bool> SubmitWorkFlowInstance(WorkFlowInstanceHandleViewModel viewModel);

        /// <summary>
        /// 取消工作流
        /// </summary>
        Task<bool> CancelFlowInstance(int workFlowInstanceId);

        /// <summary>
        /// 删除工作流ID
        /// </summary>
        Task<bool> DeleteWorkFlowInstance(int workFlowInstanceId);

        /// <summary>
        /// 审核
        /// </summary>
        Task<bool> ApproveOrDisApproveNode(WorkFlowInstanceHandleViewModel viewModel);

    }

    public class WorkFlowInstanceService : IWorkFlowInstanceService
    {
        private readonly ILogger<WorkFlowInstanceService> _logger;

        private readonly IRepository<SystemUser> _userRepository;

        private readonly IRepository<SystemUserClaim> _userClaimRepository;

        private readonly IRepository<Department> _departmentRepository;

        private readonly IRepository<WorkFlow> _workflowRepository;

        private readonly IRepository<WorkFlowLink> _linkRepository;

        private readonly IRepository<WorkFlowNode> _nodeRepository;

        private readonly IRepository<WorkFlowFormControl> _controlRepository;

        private readonly IRepository<WorkFlowInstance> _instanceRepository;

        private readonly IRepository<WorkFlowInstanceRoute> _instanceRouteRepository;

        private readonly IRepository<WorkFlowInstanceValue> _instanceValueRepository;

        private readonly IRepository<WorkFlowCondition> _conditionRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        public WorkFlowInstanceService(
            ILogger<WorkFlowInstanceService> logger,
            IRepository<SystemUser> userRepository,
            IRepository<SystemUserClaim> userClaimRepository,
            IRepository<Department> departmentRepository,
            IRepository<WorkFlow> workflowRepository,
            IRepository<WorkFlowLink> linkRepository,
            IRepository<WorkFlowNode> nodeRepository,
            IRepository<WorkFlowFormControl> controlRepository,
            IRepository<WorkFlowInstance> instanceRepository,
            IRepository<WorkFlowInstanceRoute> instanceRouteRepository,
            IRepository<WorkFlowInstanceValue> instanceValueRepository,
            IRepository<WorkFlowCondition> conditionRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _departmentRepository = departmentRepository;
            _workflowRepository = workflowRepository;
            _linkRepository = linkRepository;
            _nodeRepository = nodeRepository;
            _controlRepository = controlRepository;
            _instanceRepository = instanceRepository;
            _instanceRouteRepository = instanceRouteRepository;
            _instanceValueRepository = instanceValueRepository;
            _conditionRepository = conditionRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<bool> CreateWorkFlowInstance(int workflowId)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();
            var userName = _httpContextAccessor.HttpContext?.User?.GetName();

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
                HandlePeopleName = userName,
                HandlePepleAccount = account,
                HandleState = HandleStateEnum.未处理,
                HandleTime = DateTime.Now,
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

        public PagingResultModel<WorkFlowInstanceResultModel> GetInstanceList(int page, int size)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();
            var query = _instanceRepository.Get(i => i.CreatedUserAccount == account);
            var result = query.OrderByDescending(i => i.CreatedTime).Skip((page - 1) * size).Take(size);
            return new PagingResultModel<WorkFlowInstanceResultModel>
            {
                Data = _mapper.Map<IList<WorkFlowInstanceResultModel>>(result.ToArray()),
                Count = query.Count()
            };
        }

        public PagingResultModel<WorkFlowInstanceResultModel> GetHandledInstanceList(int page, int size)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();

            // 根据用户信息查找需要处理的实例
            var query = (from route in _instanceRouteRepository.Get()
                         join i in _instanceRepository.Get() on route.WorkFlowInstanceId equals i.Id
                         where route.HandlePepleAccount == account &&
                         route.NodeName != "开始节点" &&
                         i.WorkFlowInstanceState != WorkFlowInstanceStateEnum.NoCommitted
                         select i).Distinct();

            // 取得结果
            var result = query.OrderByDescending(i => i.CreatedTime).Skip((page - 1) * size).Take(size);
            return new PagingResultModel<WorkFlowInstanceResultModel>
            {
                Data = _mapper.Map<IList<WorkFlowInstanceResultModel>>(result.ToArray()),
                Count = query.Count()
            };

        }

        public IEnumerable<WorkFlowInstanceValueResultModel> GetWorkFlowInstanceValues(int workFlowInstanceId)
        {
            var result = _instanceValueRepository.Get(v => v.WorkFlowInstanceId == workFlowInstanceId).ToList();
            return _mapper.Map<IEnumerable<WorkFlowInstanceValueResultModel>>(result);
        }

        public async Task<bool> SaveWorkFlowInstanceValues(int workFlowInstanceId, IEnumerable<WorkFlowInstanceValueViewModel> values)
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

        /// <summary>
        /// 用户发起工作流进行提交
        /// </summary>
        public async Task<bool> SubmitWorkFlowInstance(WorkFlowInstanceHandleViewModel viewModel)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();
            var instance = _instanceRepository.Get(i =>
                i.Id == viewModel.WorkFlowInstanceId &&
                i.CreatedUserAccount == account &&
                (i.WorkFlowInstanceState == WorkFlowInstanceStateEnum.NoCommitted ||
                i.WorkFlowInstanceState == WorkFlowInstanceStateEnum.ReturnBack)).FirstOrDefault();
            if (instance != null)
            {
                try
                {
                    instance.WorkFlowInstanceState = WorkFlowInstanceStateEnum.CirCulation;

                    var currentNode = await _nodeRepository.GetAsync(instance.CurrentNodeId);

                    // 目标节点
                    var targetNodes = from n in _nodeRepository.Get(false)
                                      where (
                                          from link in _linkRepository.Get(false)
                                          where link.SourceId == currentNode.DomId &&
                                              link.WorkFlowId == instance.WorkFlowId
                                          select link.TargetId).Contains(n.DomId)
                                      select n;

                    // 开始节点只有一个后代节点，因此不用判断直接进入
                    var node = targetNodes.First();
                    instance.CurrentNodeId = node.Id;

                    // 获取处理人员
                    var users = GetHandleUsers(node, account);

                    foreach (var user in users.ToList())
                    {
                        // 添加节点处理记录
                        var routeInfo = new WorkFlowInstanceRoute
                        {
                            NodeId = node.Id,
                            NodeName = node.Name,
                            HandlePeopleName = user.Name,
                            HandlePepleAccount = user.UserName,
                            HandleState = HandleStateEnum.未处理,
                            HandleTime = DateTime.Now,
                            WorkFlowInstanceId = viewModel.WorkFlowInstanceId
                        };
                        await _instanceRouteRepository.AddAsync(routeInfo);
                    }

                    var route = _instanceRouteRepository.Get(route =>
                        route.WorkFlowInstanceId == viewModel.WorkFlowInstanceId &&
                        route.HandleState == HandleStateEnum.未处理).FirstOrDefault();
                    route.HandleState = HandleStateEnum.通过;
                    route.HandleTime = DateTime.Now;
                    _instanceRouteRepository.Update(route);

                    _instanceRepository.Update(instance);
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
            return false;
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public async Task<bool> CancelFlowInstance(int workFlowInstanceId)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();

            var instance = _instanceRepository.Get(i =>
                i.Id == workFlowInstanceId &&
                i.CreatedUserAccount == account).FirstOrDefault();
            if (instance != null)
            {
                try
                {
                    instance.WorkFlowInstanceState = WorkFlowInstanceStateEnum.Cancel;
                    _instanceRepository.Update(instance);
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
            return false;
        }

        /// <summary>
        /// 人员审核流程
        /// </summary>
        public async Task<bool> ApproveOrDisApproveNode(WorkFlowInstanceHandleViewModel viewModel)
        {
            var account = _httpContextAccessor.HttpContext?.User?.GetUserName();

            // 取得当前用户待处理的节点
            var route = _instanceRouteRepository.Get(route =>
                route.WorkFlowInstanceId == viewModel.WorkFlowInstanceId &&
                route.HandlePepleAccount == account &&
                route.HandleState == HandleStateEnum.未处理).FirstOrDefault();

            if (route != null)
            {
                // 取得节点对应的流转中的工作流实例
                var instance = _instanceRepository.Get(i => i.Id == route.WorkFlowInstanceId &&
                    i.WorkFlowInstanceState == WorkFlowInstanceStateEnum.CirCulation).FirstOrDefault();
                if (instance != null)
                {
                    try
                    {
                        if (viewModel.IsPass)
                        {
                            route.HandleState = HandleStateEnum.通过;
                            route.HandleComment = viewModel.HandleComment;

                            var currentNode = await _nodeRepository.GetAsync(instance.CurrentNodeId);

                            // 取得下一个节点
                            var targetIds = from link in _linkRepository.Get()
                                            where link.SourceId == currentNode.DomId &&
                                                link.WorkFlowId == instance.WorkFlowId
                                            select link.TargetId;

                            // 判断是否能够进入下一个节点
                            var haveNext = false;

                            // 循环所有目标节点
                            foreach (var targetId in targetIds.ToList())
                            {
                                // 找出流转到该节点的条件组
                                var conditions = from c in _conditionRepository.Get()
                                                 where c.WorkFlowId == instance.WorkFlowId &&
                                                        c.SourceId == currentNode.DomId &&
                                                        c.TargetId == targetId
                                                 select c;

                                // 比较
                                var isCompare = true;

                                // 无条件，直接进入
                                if (conditions.Count() == 0)
                                {
                                    isCompare = true;
                                    haveNext = true;
                                }
                                else
                                {
                                    // 有条件，判断条件
                                    foreach (var condition in conditions.ToList())
                                    {
                                        // 取得实例中表单的值
                                        var formValue = (from value in _instanceValueRepository.Get()
                                                         join control in _controlRepository.Get() on value.FormControlDomId equals control.DomId
                                                         where control.Id == condition.FormControlId &&
                                                             value.WorkFlowInstanceId == instance.Id
                                                         select value.Value).FirstOrDefault();

                                        // 对所有条件进行and判断
                                        switch (condition.CompareMode)
                                        {
                                            case CompareModeEnum.Equal:
                                                isCompare = isCompare && (formValue == condition.CompareValue);
                                                break;
                                            case CompareModeEnum.EqualOrGreater:
                                                isCompare = isCompare && (string.Compare(formValue, condition.CompareValue) >= 0);
                                                break;
                                            case CompareModeEnum.EqualOrSmaller:
                                                isCompare = isCompare && (string.Compare(formValue, condition.CompareValue) <= 0);
                                                break;
                                            case CompareModeEnum.Greater:
                                                isCompare = isCompare && (string.Compare(formValue, condition.CompareValue) > 0);
                                                break;
                                            case CompareModeEnum.Smaller:
                                                isCompare = isCompare && (string.Compare(formValue, condition.CompareValue) < 0);
                                                break;
                                        }

                                        // 出现条件不满足的情况直接退出判断
                                        if (!isCompare)
                                        {
                                            break;
                                        }
                                    }
                                }

                                // 条件全部满足则转入到该节点
                                if (isCompare)
                                {
                                    haveNext = true;
                                    var nextNode = _nodeRepository.Get(n => n.DomId == targetId).FirstOrDefault();

                                    instance.CurrentNodeId = nextNode.Id;

                                    if (nextNode.NodeType == NodeTypeEnum.EndNode)
                                    {
                                        instance.WorkFlowInstanceState = WorkFlowInstanceStateEnum.End;

                                        // 添加节点处理记录
                                        var routeInfo = new WorkFlowInstanceRoute
                                        {
                                            NodeId = nextNode.Id,
                                            NodeName = nextNode.Name,
                                            HandlePeopleName = "系统",
                                            HandleState = HandleStateEnum.通过,
                                            HandleTime = DateTime.Now,
                                            WorkFlowInstanceId = instance.Id
                                        };
                                        await _instanceRouteRepository.AddAsync(routeInfo);
                                    }
                                    else
                                    {

                                        // 获取处理人员
                                        var users = GetHandleUsers(nextNode, account);

                                        foreach (var u in users.ToList())
                                        {
                                            // 添加节点处理记录
                                            var routeInfo = new WorkFlowInstanceRoute
                                            {
                                                NodeId = nextNode.Id,
                                                NodeName = nextNode.Name,
                                                HandlePeopleName = u.Name,
                                                HandlePepleAccount = u.UserName,
                                                HandleState = HandleStateEnum.未处理,
                                                HandleTime = DateTime.Now,
                                                WorkFlowInstanceId = instance.Id
                                            };
                                            await _instanceRouteRepository.AddAsync(routeInfo);
                                        }
                                    }
                                    _instanceRepository.Update(instance);
                                    break;
                                }
                            }

                            if (!haveNext)
                            {
                                // 无法进行,异常结束
                                instance.WorkFlowInstanceState = WorkFlowInstanceStateEnum.BadEnd;
                            }
                        }
                        else
                        {
                            route.HandleState = HandleStateEnum.拒绝;
                            route.HandleComment = viewModel.HandleComment;

                            // 获取开始节点id
                            var startNode = await _nodeRepository.Get(n => n.WorkFlowId == instance.WorkFlowId &&
                                n.NodeType == NodeTypeEnum.StartNode).FirstOrDefaultAsync();

                            instance.CurrentNodeId = startNode.Id;
                            instance.WorkFlowInstanceState = WorkFlowInstanceStateEnum.ReturnBack;
                        }
                        _instanceRouteRepository.Update(route);
                        _instanceRepository.Update(instance);
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

            return false;
        }


        private List<SystemUser> GetHandleUsers(WorkFlowNode node, string account)
        {
            IQueryable<SystemUser> users = null;
            switch (node.HandleMode)
            {
                case HandleModeEnum.Personnel:

                    // 指定人员模式，找出所有人员生成route记录
                    var userids = node.Handlers.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    users = from u in _userRepository.Get()
                            where userids.Contains(u.Id.ToString())
                            select u;
                    break;
                case HandleModeEnum.Position:

                    // 指定职位模式，找出当前职位的人员生成route记录
                    users = from u in _userRepository.Get()
                            join uc in _userClaimRepository.Get() on u.Id equals uc.UserId
                            where uc.ClaimType == CustomClaimTypes.UserPosition &&
                                    uc.ClaimValue == node.Position.ToString()
                            select u;
                    break;
                case HandleModeEnum.Leader:

                    // 指定部门负责人模式，找出指定部门的负责人生成route记录
                    var leaderId = from d in _departmentRepository.Get()
                                   where d.Id.ToString() == node.Department
                                   select d.LeaderId;

                    users = from u in _userRepository.Get()
                            where leaderId.FirstOrDefault() == u.Id
                            select u;

                    break;
                case HandleModeEnum.UserLeader:

                    // 指定发起人部门负责人模式，找出流程发起人所在部门的负责人
                    // 查找用户所在部门
                    var department = from u in _userRepository.Get()
                                     join uc in _userClaimRepository.Get() on u.Id equals uc.UserId
                                     where u.UserName == account &&
                                         uc.ClaimType == CustomClaimTypes.UserDepartment
                                     select uc.ClaimValue;

                    // 根据部门去查找部门负责人
                    leaderId = from d in _departmentRepository.Get()
                               where d.Id.ToString() == department.FirstOrDefault()
                               select d.LeaderId;

                    users = from u in _userRepository.Get()
                            where leaderId.FirstOrDefault() == u.Id
                            select u;
                    break;
                    //case HandleModeEnum.UpLeader:
                    //    break;
            }
            return users.ToList();
        }


        /// <summary>
        /// 删除工作流ID
        /// </summary>
        public async Task<bool> DeleteWorkFlowInstance(int workFlowInstanceId)
        {
            try
            {
                var account = _httpContextAccessor.HttpContext?.User?.GetUserName();
                await _instanceRepository.RemoveAsync(i =>
                    i.Id == workFlowInstanceId &&
                    i.CreatedUserAccount == account &&
                    i.WorkFlowInstanceState != WorkFlowInstanceStateEnum.CirCulation);
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

        /// <summary>
        /// 取得各个节点情况
        /// </summary>
        public IEnumerable<WorkflowinstanceRouteResultModel> GetWorkFlowInstanceRoutes(int workFlowInstanceId)
        {
            return _mapper.Map<IEnumerable<WorkflowinstanceRouteResultModel>>(
                _instanceRouteRepository.Get(ir => ir.WorkFlowInstanceId == workFlowInstanceId).ToList());
        }
    }
}
