using backend.fluentvalidation;
using Backend.Api.Infrastructure.Authorization;
using Backend.Model.backend.api.Models.SystemManage;
using Backend.Service.backend.api.SystemManage.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Backend.Api.Controllers.SystemManage
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
        public IActionResult GetUserList([FromQuery]UserQuery userQuery)
        {
            return Ok(new
            {
                data = _userService.GetUsers(userQuery),
                count = _userService.Count()
            });
        }

        [HttpGet]
        [Permission("userDetail")]
        public async Task<IActionResult> GetUser([FromQuery]string id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost]
        [Permission("userAdd")]
        public async Task<IActionResult> AddUser([FromBody]UserViewModel model)
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
        public async Task<IActionResult> UpdateUser([FromBody]UserViewModel model)
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
        public async Task<IActionResult> RemoveUser([FromQuery]string id)
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