using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.GroupManage;
using Convience.Service.GroupManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Convience.ManagentApi.Controllers.GroupManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("list")]
        [Permission("employeeList")]
        public IActionResult Get(EmployeeQuery employeeQuery)
        {
            return Ok(_employeeService.GetEmployees(employeeQuery));
        }

        [HttpPatch]
        [Permission("employeeUpdate")]
        public async Task<IActionResult> Update(EmployeeViewModel viewModel)
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