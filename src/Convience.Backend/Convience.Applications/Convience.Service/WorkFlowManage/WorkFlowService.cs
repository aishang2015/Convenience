using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.WorkFlowManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowService
    {
        Task<WorkFlowResultModel> GetByIdAsync(int id);

        PagingResultModel<WorkFlowResultModel> GetWorkFlows(WorkFlowQueryModel query);

        Task<bool> AddWorkFlowAsync(WorkFlowViewModel model);

        Task<bool> UpdateWorkFlowAsync(WorkFlowViewModel model);

        Task<bool> DeleteWorkFlowAsync(int id);

        Task<(bool, string)> PublishWorkFlow(int id, bool isPublish);
    }


    public class WorkFlowService : IWorkFlowService
    {
        private readonly ILogger<WorkFlowService> _logger;

        private readonly IRepository<WorkFlow> _workFlowRepository;

        private readonly IRepository<WorkFlowLink> _linkRepository;

        private readonly IRepository<WorkFlowNode> _nodeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private IMapper _mapper;

        public WorkFlowService(
            ILogger<WorkFlowService> logger,
            IRepository<WorkFlow> workFlowRepository,
            IRepository<WorkFlowLink> linkRepository,
            IRepository<WorkFlowNode> nodeRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _logger = logger;
            _workFlowRepository = workFlowRepository;
            _linkRepository = linkRepository;
            _nodeRepository = nodeRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<bool> AddWorkFlowAsync(WorkFlowViewModel model)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User?.GetUserName();

                var workflow = _mapper.Map<WorkFlow>(model);
                workflow.CreatedTime = DateTime.Now;
                workflow.UpdatedTime = DateTime.Now;
                workflow.CreatedUser = userName;
                workflow.UpdatedUser = userName;

                await _workFlowRepository.AddAsync(workflow);
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

        public async Task<bool> DeleteWorkFlowAsync(int id)
        {
            try
            {
                await _workFlowRepository.RemoveAsync(id);
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

        public async Task<WorkFlowResultModel> GetByIdAsync(int id)
        {
            var result = await _workFlowRepository.GetAsync(id);
            return _mapper.Map<WorkFlowResultModel>(result);
        }

        public PagingResultModel<WorkFlowResultModel> GetWorkFlows(WorkFlowQueryModel query)
        {
            Expression<Func<WorkFlow, bool>> where = ExpressionExtension.TrueExpression<WorkFlow>()
                .And(wf => wf.WorkFlowGroupId == query.WorkFlowGroupId)
                .AndIfHaveValue(query.IsPublish, wf => wf.IsPublish == query.IsPublish);

            var workFlowQuery = _workFlowRepository.Get(where)
                .OrderByDescending(w => w.CreatedTime)
                .Skip((query.Page - 1) * query.Size).Take(query.Size).ToArray();

            return new PagingResultModel<WorkFlowResultModel>
            {
                Data = _mapper.Map<IList<WorkFlowResultModel>>(workFlowQuery),
                Count = workFlowQuery.Count()
            };
        }

        public async Task<(bool, string)> PublishWorkFlow(int id, bool isPublish)
        {
            try
            {
                var wf = await _workFlowRepository.GetAsync(id);

                if (isPublish)
                {
                    var nodes = _nodeRepository.Get(n => n.WorkFlowId == id).ToList();
                    var links = _linkRepository.Get(l => l.WorkFlowId == id).ToList();

                    // 检查节点种类
                    var nodeTypeCount = nodes.Select(n => n.NodeType).Distinct().Count();
                    if (nodeTypeCount < 3)
                    {
                        return (false, "流程不完整！");
                    }

                    // 检查流程完整
                    var startNode = nodes.FirstOrDefault(n => n.NodeType == NodeTypeEnum.StartNode);
                    WorkFlowNode endNode = null;
                    while (true)
                    {
                        var link = links.FirstOrDefault(l => l.SourceId == startNode.DomId);
                        if (link != null)
                        {
                            startNode = nodes.FirstOrDefault(n => n.DomId == link.TargetId);
                            if (startNode == null)
                            {
                                endNode = startNode;
                                break;
                            }
                        }
                        else
                        {
                            endNode = startNode;
                            break;
                        }
                    }
                    if (endNode.NodeType != NodeTypeEnum.EndNode)
                    {
                        return (false, "流程不完整！");
                    }

                    // 检查处理人员模式
                    if (nodes.Any(n => n.HandleMode == 0 && n.NodeType == NodeTypeEnum.WorkNode))
                    {
                        return (false, "处理人员信息不完整！");
                    }
                }

                wf.IsPublish = isPublish;
                await _unitOfWork.SaveAsync();
                return (true, string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return (false, "操作失败！");
            }
        }

        public async Task<bool> UpdateWorkFlowAsync(WorkFlowViewModel model)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User?.GetUserName();

                var wf = _workFlowRepository.Get(wf => wf.Id == model.Id).FirstOrDefault();
                _mapper.Map(model, wf);
                wf.UpdatedTime = DateTime.Now;
                wf.UpdatedUser = userName;
                _workFlowRepository.Update(wf);
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
