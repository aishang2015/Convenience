using Convience.Injection;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.SystemManage;
using Convience.Service.SystemManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.SystemManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
#pragma warning disable CS0649

        [Autowired]
        private readonly IUserService _userService;

        [HttpGet("list")]
        [Permission("userList")]
        public IActionResult GetUserList([FromQuery] UserQueryModel userQuery)
        {
            return Ok(_userService.GetUsers(userQuery));
        }

        [HttpGet("dic")]
        [Permission("userDic")]
        public IActionResult GetUserDic([FromQuery] string name)
        {
            return Ok(_userService.GetUserDic(name));
        }

        [HttpGet]
        [Permission("userDetail")]
        public IActionResult GetUser([FromQuery] string id)
        {
            var user = _userService.GetUser(id);
            return Ok(user);
        }

        [HttpPost]
        [Permission("userAdd")]
        [LogFilter("系统管理", "用户管理", "创建用户")]
        public async Task<IActionResult> AddUser([FromBody] UserViewModel model)
        {
            var result = await _userService.AddUserAsync(model);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("userUpdate")]
        [LogFilter("系统管理", "用户管理", "更新用户")]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel model)
        {
            var result = await _userService.UpdateUserAsync(model);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpDelete]
        [Permission("userDelete")]
        [LogFilter("系统管理", "用户管理", "删除用户")]
        public async Task<IActionResult> RemoveUser([FromQuery] string id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpPost("password")]
        [Permission("userPassword")]
        public async Task<IActionResult> SetPassword([FromBody] UserPasswordModel model)
        {
            var result = await _userService.SetPasswordAsync(model);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();

        }
    }
}