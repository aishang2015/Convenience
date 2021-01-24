using Convience.Model.Models.SystemTool;
using Convience.Service.SystemTool;

using Microsoft.AspNetCore.Mvc;

namespace Convience.ManagentApi.Controllers.SystemTool
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperateLogDetailController : ControllerBase
    {
        private readonly IOperateLogService _operateLogService;

        public OperateLogDetailController(IOperateLogService operateLogService)
        {
            _operateLogService = operateLogService;
        }

        [HttpGet]
        public IActionResult GetDetails([FromQuery] OperateLogDetailQueryModel queryModel)
        {
            var result = _operateLogService.GetPagingOperateDetail(queryModel);
            return Ok(result);
        }
    }
}
