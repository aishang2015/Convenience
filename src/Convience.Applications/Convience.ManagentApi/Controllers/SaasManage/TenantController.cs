using Convience.Model.Models.SaasManage;
using Convience.Service.SaasManage;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.SaasManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("list")]
        public IActionResult Get([FromQuery]TenantQuery query)
        {
            return Ok(new
            {
                Data = _tenantService.Get(query),
                Count = _tenantService.Count(query)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail([FromQuery]Guid id)
        {
            var result = await _tenantService.Get(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]TenantViewModel model)
        {
            await _tenantService.AddAsync(model);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody]TenantViewModel model)
        {
            await _tenantService.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Guid id)
        {
            await _tenantService.RemoveAsync(id);
            return Ok();
        }
    }
}