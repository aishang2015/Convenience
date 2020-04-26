using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.ContentManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnService _columnService;

        public ColumnController(IColumnService columnService)
        {
            _columnService = columnService;
        }

        [HttpGet]
        [Permission("columnGet")]
        public async Task<IActionResult> GetById(int id)
        {
            var column = await _columnService.GetByIdAsync(id);
            return Ok(column);
        }

        [HttpGet("all")]
        [Permission("allColumn")]
        public IActionResult Get()
        {
            return Ok(_columnService.GetAllColumn());
        }

        [HttpDelete]
        [Permission("columnDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _columnService.DeleteColumnAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("columnAdd")]
        public async Task<IActionResult> Add(ColumnViewModel columnViewModel)
        {
            var isSuccess = await _columnService.AddColumnAsync(columnViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("columnUpdate")]
        public async Task<IActionResult> Update(ColumnViewModel columnViewModel)
        {
            var isSuccess = await _columnService.UpdateColumnAsync(columnViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }

    }
}