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
    public class FolderController : ControllerBase
    {
        private readonly IFileManageService _fileManageService;

        public FolderController(IFileManageService fileManageService)
        {
            _fileManageService = fileManageService;
        }

        [HttpPost]
        [Permission("fileAdd")]
        [LogFilter("内容管理", "文件管理", "创建文件夹")]
        public async Task<IActionResult> MakeDirectory([FromBody] FileViewModel vm)
        {
            var result = await _fileManageService.MakeDirectoryAsync(vm);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpDelete]
        [Permission("fileDelete")]
        [LogFilter("内容管理", "文件管理", "删除文件夹")]
        public async Task<IActionResult> DeleteDirectory([FromQuery] FileViewModel vm)
        {
            var isSuccess = await _fileManageService.DeleteDirectoryAsync(vm);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败！");
            }
            return Ok();
        }
    }
}