using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Model.backend.api.Models.SaasManage;
using Backend.Service.backend.api.SaasManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.SaasManage
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