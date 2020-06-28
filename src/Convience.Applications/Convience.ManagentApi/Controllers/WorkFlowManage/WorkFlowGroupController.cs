using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.WorkFlowManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowGroupController : ControllerBase
    {
        private readonly IWorkFlowGroupService _workFlowGroupService;

        public WorkFlowGroupController(IWorkFlowGroupService workFlowGroupService)
        {
            _workFlowGroupService = workFlowGroupService;
        }

        [HttpGet()]
        [Permission("workflowGroupGet")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var result = await _workFlowGroupService.GetWorkFlowGroupById(id);
            return Ok(result);
        }

        [HttpGet("all")]
        [Permission("allworkflowGroup")]
        public IActionResult Get()
        {
            return Ok(_workFlowGroupService.GetAllWorkFlowGroup());
        }

        [HttpDelete]
        [Permission("workflowGroupDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _workFlowGroupService.DeleteWorkFlowGroupAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("workflowGroupAdd")]
        public async Task<IActionResult> Add(WorkFlowGroupViewModel viewModel)
        {
            var isSuccess = await _workFlowGroupService.AddWorkFlowGroupAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("workflowGroupUpdate")]
        public async Task<IActionResult> Update(WorkFlowGroupViewModel viewModel)
        {
            var isSuccess = await _workFlowGroupService.UpdateWorkFlowGroupAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}