using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.fluentvalidation;
using Backend.Model.backend.api.Models.SystemManage;
using Backend.Service.backend.api.SystemManage.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetUserList([FromQuery]UserQuery userQuery)
        {
            return Ok(new
            {
                data = _userService.GetUsers(userQuery),
                count = _userService.Count()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery]string id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]UserViewModel model)
        {
            var isSuccess = await _userService.AddUserAsync(model);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法创建用户，请检查用户名是否相同！");
            }
            isSuccess = await _userService.SetUserPassword(model.UserName, model.Password);
            if (!isSuccess)
            {
                return this.BadRequestResult("初始密码创建失败！");
            }
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody]UserViewModel model)
        {
            var isSuccess = await _userService.UpdateUserAsync(model);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法更新用户，请检查用户名是否相同！");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromQuery]string id)
        {
            var isSuccess = await _userService.RemoveUserAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法删除角色！");
            }
            return Ok();
        }
    }
}