using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.ContentManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [LogFilter("内容管理", "文章管理", "删除文章栏目")]
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
        [LogFilter("内容管理", "文章管理", "创建文章栏目")]
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
        [LogFilter("内容管理", "文章管理", "更新文章栏目")]
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