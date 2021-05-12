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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet()]
        [Permission("roleGet")]
        public IActionResult GetRoles([FromQuery] string id)
        {
            return Ok(_roleService.GetRole(id));
        }

        [HttpGet("list")]
        [Permission("roleList")]
        public IActionResult GetRoles([FromQuery] string name, [FromQuery] int page, [FromQuery] int size)
        {
            return Ok(_roleService.GetRoles(page, size, name));
        }

        [HttpDelete]
        [Permission("roleDelete")]
        [LogFilter("系统管理", "角色管理", "删除角色")]
        public async Task<IActionResult> DeleteRole([FromQuery] string name)
        {
            var result = await _roleService.RemoveRoleAsync(name);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpPost]
        [Permission("roleAdd")]
        [LogFilter("系统管理", "角色管理", "创建角色")]
        public async Task<IActionResult> AddRole([FromBody] RoleViewModel viewModel)
        {
            var result = await _roleService.AddRole(viewModel);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("roleUpdate")]
        [LogFilter("系统管理", "角色管理", "更新角色")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleViewModel viewModel)
        {
            var result = await _roleService.UpdateAsync(viewModel);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpGet("namelist")]
        [Permission("roleNameList")]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleService.GetRoles());
        }
    }
}