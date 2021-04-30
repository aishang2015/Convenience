using Convience.Model.Models.SystemTool;
using Convience.Service.SystemTool;
using Convience.Util.Extension;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.SystemTool
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginLogController : ControllerBase
    {
        public readonly ILoginLogService _loginLogService;

        public LoginLogController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        [HttpGet("setting")]
        public IActionResult GetSetting()
        {
            var result = _loginLogService.GetSetting();
            return Ok(result);
        }

        [HttpPost("setting")]
        public async Task<IActionResult> UpdateSetting([FromBody] LoginLogSettingViewModel viewModel)
        {
            var isSuccess = await _loginLogService.UpdateLoginLogSettingAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败！");
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetLoginLog([FromQuery] LoginLogQueryModel queryModel)
        {
            var resultModel = _loginLogService.GetLoginLogDetail(queryModel);
            return Ok(resultModel);
        }
    }


}
