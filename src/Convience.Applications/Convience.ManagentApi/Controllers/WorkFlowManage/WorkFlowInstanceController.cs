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

        /// <summary>
        /// 创建工作流
        /// </summary>
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

        /// <summary>
        /// 取得工作流列表
        /// </summary>
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

        /// <summary>
        /// 取得表单值
        /// </summary>
        [HttpGet("values")]
        [Permission("workFlowInstanceValuesGet")]
        public IActionResult GetWorkFlowInstanceValues([FromQuery]int workFlowInstanceId)
        {
            return Ok(_workFlowInstanceService.GetWorkFlowInstance(workFlowInstanceId));
        }

        /// <summary>
        /// 保存表单值
        /// </summary>
        [HttpPut("values")]
        [Permission("workFlowInstanceValuesPut")]
        public async Task<IActionResult> SaveWorkFlowInstanceValues([FromBody]InstanceValuesViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.SaveWorkFlowInstance(vm.WorkFlowInstanceId, vm.Values);
            if (!isSuccess)
            {
                return this.BadRequestResult("创建工作流失败!");
            }
            return Ok();
        }
    }
}