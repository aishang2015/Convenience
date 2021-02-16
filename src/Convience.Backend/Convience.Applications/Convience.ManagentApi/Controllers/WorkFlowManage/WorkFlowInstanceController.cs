using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
using Convience.Util.Extension;

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
        [Permission("workFlowInstanceAdd")]
        [LogFilter("工作流", "工作流实例管理", "创建工作流实例")]
        public async Task<IActionResult> CreateWorkFlowInstance([FromBody] WorkFlowInstanceViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.CreateWorkFlowInstance(vm.WorkFlowId);
            if (!isSuccess)
            {
                return this.BadRequestResult("创建工作流失败!");
            }
            return Ok();
        }

        /// <summary>
        /// 删除工作流
        /// </summary>
        [HttpDelete]
        [Permission("workFlowInstanceDelete")]
        [LogFilter("工作流", "工作流实例管理", "删除工作流实例")]
        public async Task<IActionResult> DeleteWorkFlowInstance([FromQuery] int id)
        {
            var isSuccess = await _workFlowInstanceService.DeleteWorkFlowInstance(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除工作流失败!");
            }
            return Ok();
        }


        /// <summary>
        /// 取得工作流列表
        /// </summary>
        [HttpGet]
        [Permission("workFlowInstanceList")]
        public IActionResult GetWorkFlowInstances([FromQuery] PageQueryModel query)
        {
            return Ok(_workFlowInstanceService.GetInstanceList(query.Page, query.Size));
        }

        /// <summary>
        /// 取得表单值
        /// </summary>
        [HttpGet("values")]
        [Permission("workFlowInstanceValuesGet")]
        public IActionResult GetWorkFlowInstanceValues([FromQuery] int workFlowInstanceId)
        {
            return Ok(_workFlowInstanceService.GetWorkFlowInstanceValues(workFlowInstanceId));
        }

        /// <summary>
        /// 保存表单值
        /// </summary>
        [HttpPut("values")]
        [Permission("workFlowInstanceValuesUpdate")]
        [LogFilter("工作流", "工作流实例管理", "保存工作流实例")]
        public async Task<IActionResult> SaveWorkFlowInstanceValues([FromBody] InstanceValuesViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.SaveWorkFlowInstanceValues(vm.WorkFlowInstanceId, vm.Values);
            if (!isSuccess)
            {
                return this.BadRequestResult("创建工作流失败!");
            }
            return Ok();
        }

        [HttpGet("routes")]
        [Permission("workFlowInstanceRouteList")]
        public IActionResult GetWorkFlowInstanceRoutes([FromQuery] int workFlowInstanceId)
        {
            return Ok(_workFlowInstanceService.GetWorkFlowInstanceRoutes(workFlowInstanceId));
        }

        /// <summary>
        /// 提交工作流开始流转
        /// </summary>
        [HttpPut]
        [Permission("workFlowInstanceSubmit")]
        [LogFilter("工作流", "工作流实例管理", "提交工作流实例")]
        public async Task<IActionResult> SubmitWorkFlowInstance(WorkFlowInstanceHandleViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.SubmitWorkFlowInstance(vm);
            if (!isSuccess)
            {
                return this.BadRequestResult("提交工作流失败!");
            }
            return Ok();
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        [HttpPatch]
        [Permission("workFlowInstanceCancel")]
        [LogFilter("工作流", "工作流实例管理", "取消工作流实例")]
        public async Task<IActionResult> CancelFlowInstance([FromBody] WorkFlowInstanceHandleViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.CancelFlowInstance(vm.WorkFlowInstanceId);
            if (!isSuccess)
            {
                return this.BadRequestResult("取消工作流失败!");
            }
            return Ok();
        }

        /// <summary>
        /// 取得工作流列表
        /// </summary>
        [HttpGet("handle")]
        [Permission("handledWorkFlowInstanceList")]
        public IActionResult GetHandledWorkFlowInstances([FromQuery] PageQueryModel query)
        {
            return Ok(_workFlowInstanceService.GetHandledInstanceList(query.Page, query.Size));
        }

        [HttpPost("handle")]
        [Permission("handleWorkFlowInstanceApprove")]
        [LogFilter("工作流", "工作流实例管理", "审核工作流实例")]
        public async Task<IActionResult> ApproveOrDisApproveNode([FromBody] WorkFlowInstanceHandleViewModel vm)
        {
            var isSuccess = await _workFlowInstanceService.ApproveOrDisApproveNode(vm);
            if (!isSuccess)
            {
                return this.BadRequestResult("审批失败!");
            }
            return Ok();

        }
    }
}