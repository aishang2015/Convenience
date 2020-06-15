using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
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
        public async Task<IActionResult> AddOrUpdate(WorkFlowFormViewModel viewModel)
        {
            var isSuccess = await _workFlowFormService.AddOrUpdateWorkFlowForm(viewModel, HttpContext.User.GetUserName());
            if (!isSuccess)
            {
                return this.BadRequestResult("操作失败!");
            }
            return Ok();
        }
    }
}