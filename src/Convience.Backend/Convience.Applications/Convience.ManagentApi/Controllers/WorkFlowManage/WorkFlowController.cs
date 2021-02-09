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
    public class WorkFlowController : ControllerBase
    {
        private readonly IWorkFlowService _workflowService;

        public WorkFlowController(IWorkFlowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpGet]
        [Permission("workflowGet")]
        public async Task<IActionResult> GetById(int id)
        {
            var workflow = await _workflowService.GetByIdAsync(id);
            return Ok(workflow);
        }

        [HttpGet("list")]
        [Permission("workflowList")]
        public IActionResult Get([FromQuery] WorkFlowQueryModel workflowQuery)
        {
            return Ok(_workflowService.GetWorkFlows(workflowQuery));
        }

        [HttpDelete]
        [Permission("workflowDelete")]
        [LogFilter("工作流", "工作流定义管理", "删除工作流定义")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _workflowService.DeleteWorkFlowAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("workflowAdd")]
        [LogFilter("工作流", "工作流定义管理", "创建工作流定义")]
        public async Task<IActionResult> Add(WorkFlowViewModel workflowViewModel)
        {
            var isSuccess = await _workflowService.AddWorkFlowAsync(workflowViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("workflowUpdate")]
        [LogFilter("工作流", "工作流定义管理", "更新工作流定义")]
        public async Task<IActionResult> Update(WorkFlowViewModel workflowViewModel)
        {
            var isSuccess = await _workflowService.UpdateWorkFlowAsync(workflowViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }

        [HttpPut]
        [Permission("workflowPublish")]
        [LogFilter("工作流", "工作流定义管理", "发布工作流定义")]
        public async Task<IActionResult> Publish(WorkFlowViewModel workflowViewModel)
        {
            var result = await _workflowService.PublishWorkFlow(workflowViewModel.Id, workflowViewModel.IsPublish);
            if (!result.Item1)
            {
                return this.BadRequestResult(result.Item2);
            }
            return Ok();
        }
    }
}