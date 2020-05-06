using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.GroupManage;
using Convience.Service.GroupManage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.GroupManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet()]
        [Permission("employeeGet")]
        public IActionResult Get(int id)
        {
            return Ok(_employeeService.GetEmployeeById(id));
        }

        [HttpGet("list")]
        [Permission("employeeList")]
        public IActionResult Get([FromQuery]EmployeeQuery employeeQuery)
        {
            var result = _employeeService.GetEmployees(employeeQuery);
            return Ok(new
            {
                data = result.Item1,
                count = result.Item2
            });
        }

        [HttpPatch]
        [Permission("employeeUpdate")]
        public async Task<IActionResult> Update([FromBody]EmployeeViewModel viewModel)
        {
            var isSuccess = await _employeeService.UpdateEmplyeeAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }


    }
}