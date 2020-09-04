using Convience.Fluentvalidation;
using Convience.Model.Models.Tenant;
using Convience.Service.TenantService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.TenantControllers
{
    [Route("api/tenant")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] TenantLoginViewModel viewModel)
        {
            var result = _accountService.Login(viewModel);
            if (result.Item1)
            {
                return Ok(new { Token = result.Item2 });
            }
            else
            {
                return this.BadRequestResult(result.Item2);
            }
        }

        [HttpPost("regist")]
        public async Task<IActionResult> RegistMemeber([FromBody] RegisterViewModel registerViewModel)
        {
            var result = await _accountService.Regist(registerViewModel);
            if (result)
            {
                return Ok();
            }
            else
            {
                return this.BadRequestResult("租户名已存在！");
            }
        }
    }
}
