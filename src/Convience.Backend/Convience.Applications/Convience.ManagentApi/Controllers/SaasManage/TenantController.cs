﻿using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.OperateLog;
using Convience.Model.Models.SaasManage;
using Convience.Service.SaasManage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.SaasManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("list")]
        [Permission("tenantList")]
        public IActionResult Get([FromQuery] TenantQueryModel query)
        {
            return Ok(new
            {
                Data = _tenantService.Get(query),
                Count = _tenantService.Count(query)
            });
        }

        [HttpGet]
        [Permission("tenantGet")]
        public async Task<IActionResult> GetDetail([FromQuery] Guid id)
        {
            var result = await _tenantService.Get(id);
            return Ok(result);
        }

        [HttpPost]
        [Permission("tenantAdd")]
        [LogFilter("", "租户管理", "创建租户")]
        public async Task<IActionResult> Add([FromBody] TenantViewModel model)
        {
            await _tenantService.AddAsync(model);
            return Ok();
        }

        [HttpPatch]
        [Permission("tenantUpdate")]
        [LogFilter("", "租户管理", "更新租户")]
        public async Task<IActionResult> Update([FromBody] TenantViewModel model)
        {
            await _tenantService.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete]
        [Permission("tenantDelete")]
        [LogFilter("", "租户管理", "删除租户")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _tenantService.RemoveAsync(id);
            return Ok();
        }
    }
}