using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.WorkFlowManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowFlowController : ControllerBase
    {
        private readonly IWorkFlowFlowService _workFlowFlowService;

        public WorkFlowFlowController(IWorkFlowFlowService workFlowFlowService)
        {
            _workFlowFlowService = workFlowFlowService;
        }

        [HttpGet]
        [Permission("workflowFlowGet")]
        public IActionResult GetById(int workflowId)
        {
            return Ok(_workFlowFlowService.GetWorkFlowFlow(workflowId));
        }

        [HttpPost]
        [Permission("workflowFlowAddUpdate")]
        [LogFilter("工作流", "工作流流程管理", "更新工作流流程")]
        public async Task<IActionResult> AddOrUpdate(WorkFlowFlowViewModel viewModel)
        {
            var isSuccess = await _workFlowFlowService.AddOrUpdateWorkFlowFlow(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("操作失败!");
            }
            return Ok();
        }
    }
}