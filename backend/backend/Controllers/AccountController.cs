
using backend.fluentvalidation;
using backend.service.backend.api.Account;

using Backend.Api.Infrastructure;
using Backend.Model.backend.api.Models.AccountViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace backend.api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _loginService;

        public AccountController(IAccountService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var result = await _loginService.ValidateCredentials(model.UserName, model.Password);
            if (!result.Item1)
            {
                return this.BadRequestResult(result.Item2);
            }
            return Ok(new LoginResult
            {
                Name = result.Item3.Name,
                Avatar = result.Item3.Avatar,
                Token = result.Item2
            });
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ChangePwdByOldPwd(ChangePwdViewModel viewmodel)
        {
            var result = await _loginService.ChangePassword(User.GetUserName(), viewmodel.OldPassword, viewmodel.NewPassword);
            if (!result)
            {
                return this.BadRequestResult("密码修改失败,请确认旧密码是否正确！");
            }
            return Ok();
        }
    }
}