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
    public class WorkFlowFormController : ControllerBase
    {
        private readonly IWorkFlowFormService _workFlowFormService;

        public WorkFlowFormController(IWorkFlowFormService workFlowFormService)
        {
            _workFlowFormService = workFlowFormService;
        }

        [HttpGet]
        [Permission("workflowFormGet")]
        public IActionResult GetById(int workflowId)
        {
            var workflow = _workFlowFormService.GetWorkFlowForm(workflowId);
            return Ok(workflow);
        }

        [HttpPost]
        [Permission("workflowFormAddUpdate")]
        [LogFilter("工作流", "工作流表单管理", "更新工作流表单")]
        public async Task<IActionResult> AddOrUpdate(WorkFlowFormViewModel viewModel)
        {
            var isSuccess = await _workFlowFormService.AddOrUpdateWorkFlowForm(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("保存失败!");
            }
            return Ok();
        }

        [HttpGet("dic")]
        [Permission("workflowFormControlDic")]
        public IActionResult GetWorkFlowFormControlDic([FromQuery] int WorkFlowId)
        {
            return Ok(_workFlowFormService.GetWorkFlowFormControlDic(WorkFlowId));
        }
    }
}