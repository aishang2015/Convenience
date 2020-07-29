using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.SystemManage;
using Convience.Service.SystemManage;

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

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

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
        public async Task<IActionResult> GetUser([FromQuery] string id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost]
        [Permission("userAdd")]
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
        public async Task<IActionResult> RemoveUser([FromQuery] string id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }
    }
}