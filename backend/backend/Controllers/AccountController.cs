
using backend.fluentvalidation;
using backend.model.backend.api.AccountViewModels;
using backend.service.backend.api.Account;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace backend.api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _loginService.ValidateCredentials(model.UserName, model.Password);
            if (string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult("错误的用户名或密码！");
            }
            return Ok(new LoginResult
            {
                UserName = model.UserName,
                Token = result
            });
        }

        //public async Task<IActionResult> ChangePwdByOldPwd()
        //{
        //    return Ok();
        //}
    }
}