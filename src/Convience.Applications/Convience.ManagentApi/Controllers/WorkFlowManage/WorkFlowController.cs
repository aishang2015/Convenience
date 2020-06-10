using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.WorkFlowManage;
using Convience.Service.WorkFlowManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get([FromQuery]WorkFlowQuery workflowQuery)
        {
            var result = _workflowService.GetWorkFlows(workflowQuery);
            return Ok(new
            {
                data = result.Item1,
                count = result.Item2
            });
        }

        [HttpDelete]
        [Permission("workflowDelete")]
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
        public async Task<IActionResult> Add(WorkFlowViewModel workflowViewModel)
        {
            var isSuccess = await _workflowService.AddWorkFlowAsync(workflowViewModel, User.GetUserName());
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("workflowUpdate")]
        public async Task<IActionResult> Update(WorkFlowViewModel workflowViewModel)
        {
            var isSuccess = await _workflowService.UpdateWorkFlowAsync(workflowViewModel, User.GetUserName());
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}