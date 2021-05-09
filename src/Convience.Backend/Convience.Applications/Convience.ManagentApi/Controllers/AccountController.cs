using Convience.JwtAuthentication;
using Convience.ManagentApi.Infrastructure.Logs.LoginLog;
using Convience.Model.Constants;
using Convience.Model.Models.Account;
using Convience.Service.Account;
using Convience.Service.SystemManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _loginService;

        private readonly IMenuService _menuService;

        private readonly IRoleService _roleService;

        public AccountController(IAccountService loginService,
            IMenuService menuService,
            IRoleService roleService)
        {
            _loginService = loginService;
            _menuService = menuService;
            _roleService = roleService;
        }

        [HttpGet("captcha")]
        public IActionResult GetCaptcha()
        {
            var result = _loginService.GetCaptcha();
            return Ok(result);
        }

        [HttpPost("login")]
        [LoginLogFilter]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // 验证验证码
            var validResult = _loginService.ValidateCaptcha(model.CaptchaKey, model.CaptchaValue);
            if (!string.IsNullOrEmpty(validResult))
            {
                return this.BadRequestResult(validResult);
            }

            // 验证用户是否可以使用
            var isActive = _loginService.IsStopUsing(model.UserName);
            if (!isActive)
            {
                return this.BadRequestResult(AccountConstants.ACCOUNT_NOT_ACTIVE);
            }

            // 取得用户信息
            var validateResult = await _loginService.ValidateCredentialsAsync(model.UserName, model.Password);
            if (validateResult is null)
            {
                return this.BadRequestResult(AccountConstants.ACCOUNT_WRONG_INPUT);
            }

            // 取得权限信息
            var menuIds = _roleService.GetRoleClaimValue(validateResult.RoleIds.Split(',',
                StringSplitOptions.RemoveEmptyEntries), CustomClaimTypes.RoleMenus);

            // 获取菜单权限对应的前端标识
            var irs = _menuService.GetIdentificationRoutes(menuIds.ToArray());

            return Ok(new LoginResultModel(
                validateResult.Name,
                validateResult.Avatar,
                validateResult.Token,
                irs.Item1,
                irs.Item2));
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ChangePwdByOldPwd(ChangePwdViewModel viewmodel)
        {
            var result = await _loginService.ChangePasswordAsync(User.GetUserName(), viewmodel.OldPassword, viewmodel.NewPassword);
            if (!result)
            {
                return this.BadRequestResult(AccountConstants.ACCOUNT_MODIFY_PASSWORD_FAIL);
            }
            return Ok();
        }
    }
}