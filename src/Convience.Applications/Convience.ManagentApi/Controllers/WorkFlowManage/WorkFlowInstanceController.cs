using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.WorkFlowManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkFlowInstanceController : ControllerBase
    {
        private readonly IWorkFlowInstanceService _workFlowInstanceService;

        public WorkFlowInstanceController(IWorkFlowInstanceService workFlowInstanceService)
        {
            _workFlowInstanceService = workFlowInstanceService;
        }

        [HttpPost]
        [Permission("workFlowInstancePost")]
        public async Task<IActionResult> CreateWorkFlowInstance([FromBody]WorkFlowInstanceViewModel vm)
        {
             var isSuccess = await _workFlowInstanceService
                .CreateWorkFlowInstance(vm.WorkFlowId, User.GetUserName(), User.GetName());
            if (!isSuccess)
            {
                return this.BadRequestResult("创建工作流失败!");
            }
            return Ok();
        }

        [HttpGet]
        [Permission("workFlowInstanceGet")]
        public IActionResult GetWorkFlowInstances([FromQuery]PageQuery query)
        {
            var result = _workFlowInstanceService.GetInstanceList(User.GetUserName(), query.Page, query.Size);
            return Ok(new
            {
                data = result.Item1,
                count = result.Item2,
            });
        }
    }
}