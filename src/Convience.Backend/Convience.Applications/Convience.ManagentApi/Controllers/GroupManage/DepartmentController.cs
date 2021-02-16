using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.GroupManage;
using Convience.Service.GroupManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.GroupManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet()]
        [Permission("departmentGet")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var result = await _departmentService.GetDepartmentById(id);
            return Ok(result);
        }

        [HttpGet("all")]
        [Permission("allDepartment")]
        public IActionResult Get()
        {
            return Ok(_departmentService.GetAllDepartment());
        }

        [HttpDelete]
        [Permission("departmentDelete")]
        [LogFilter("组织管理", "部门管理", "删除部门")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _departmentService.DeleteDepartmentAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("departmentAdd")]
        [LogFilter("组织管理", "部门管理", "添加部门")]
        public async Task<IActionResult> Add(DepartmentViewModel viewModel)
        {
            var isSuccess = await _departmentService.AddDepartmentAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("departmentUpdate")]
        [LogFilter("组织管理", "部门管理", "更新部门")]
        public async Task<IActionResult> Update(DepartmentViewModel viewModel)
        {
            var isSuccess = await _departmentService.UpdateDepartmentAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }

        [HttpGet("dic")]
        [Permission("departmentDic")]
        public IActionResult GetDepartmentDic([FromQuery] string name)
        {
            return Ok(_departmentService.GetDepartmentDic(name));
        }
    }
}