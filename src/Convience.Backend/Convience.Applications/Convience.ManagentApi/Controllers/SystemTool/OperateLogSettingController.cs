using Convience.Model.Models.SystemTool;
using Convience.Service.SystemTool;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.SystemTool
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperateLogSettingController : ControllerBase
    {
        private readonly IOperateLogService _operateLogService;

        public OperateLogSettingController(IOperateLogService operateLogService)
        {
            _operateLogService = operateLogService;
        }

        [HttpGet]
        public IActionResult GetSettings([FromQuery] OperateLogSettingQueryModel queryModel)
        {
            var result = _operateLogService.GetPagingOperateLogSetting(queryModel);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateSetting([FromBody] OperateLogSettingViewModel viewModel)
        {
            var isSuccess = await _operateLogService.UpdateOpreateLogSettingAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败！");
            }
            return Ok();
        }
    }
}
