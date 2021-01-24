using Convience.JwtAuthentication;
using Convience.Model.Models.Account;
using Convience.Service.Account;
using Convience.Service.SystemManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // 验证验证码
            var validResult = _loginService.ValidateCaptcha(model.CaptchaKey, model.CaptchaValue);
            if (!string.IsNullOrEmpty(validResult))
            {
                return this.BadRequestResult(validResult);
            }

            // 取得用户信息
            var result = await _loginService.ValidateCredentials(model.UserName, model.Password);
            if (!result.Item1)
            {
                return this.BadRequestResult(result.Item2);
            }

            // 取得权限信息
            var menuIds = _roleService.GetRoleClaimValue(result.Item3.RoleIds.Split(',',
                System.StringSplitOptions.RemoveEmptyEntries), CustomClaimTypes.RoleMenus);

            var irs = _menuService.GetIdentificationRoutes(menuIds.ToArray());
            return Ok(new LoginResultModel
            {
                Name = result.Item3.Name,
                Avatar = result.Item3.Avatar,
                Identification = irs.Item1,
                Routes = irs.Item2,
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