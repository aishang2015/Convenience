using backend.fluentvalidation;
using Backend.Api.Infrastructure.Authorization;
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

        [HttpGet()]
        [Permission("roleGet")]
        public async Task<IActionResult> GetRoles([FromQuery]string id)
        {
            return Ok(await _roleService.GetRole(id));
        }

        [HttpGet("list")]
        [Permission("roleList")]
        public IActionResult GetRoles([FromQuery]string name, [FromQuery]int page, [FromQuery]int size)
        {
            return Ok(new
            {
                data = _roleService.GetRoles(page, size, name),
                count = _roleService.Count()
            });
        }

        [HttpDelete]
        [Permission("roleDelete")]
        public async Task<IActionResult> DeleteRole([FromQuery]string name)
        {
            var result = await _roleService.RemoveRole(name);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpPost]
        [Permission("roleAdd")]
        public async Task<IActionResult> AddRole([FromBody]RoleViewModel viewModel)
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
        public async Task<IActionResult> UpdateRole([FromBody]RoleViewModel viewModel)
        {
            var result = await _roleService.Update(viewModel);
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