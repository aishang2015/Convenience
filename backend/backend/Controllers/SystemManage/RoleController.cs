using backend.fluentvalidation;
using Backend.Model.backend.api.Models.SystemManage;
using Backend.Service.backend.api.SystemManage.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Api.Controllers.SystemManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetRoles([FromQuery]string name, [FromQuery]int page, [FromQuery]int size)
        {
            return Ok(new
            {
                data = _roleService.GetRoles(page, size, name),
                count = _roleService.Count()
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole([FromQuery]string name)
        {
            var isSuccess = await _roleService.RemoveRole(name);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法删除角色，角色中包含用户！");
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody]RoleViewModel viewModel)
        {
            var isSuccess = await _roleService.AddRole(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法创建角色，请检查角色名是否相同！");
            }
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateRole([FromBody]RoleViewModel viewModel)
        {
            var isSuccess = await _roleService.Update(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("无法更新角色，请检查角色名是否相同！");
            }
            return Ok();
        }

        [HttpGet("list")]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleService.GetRoles());
        }
    }
}