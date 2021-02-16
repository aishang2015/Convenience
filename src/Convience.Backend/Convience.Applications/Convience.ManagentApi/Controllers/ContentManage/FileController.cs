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
    public class FileController : ControllerBase
    {
        private readonly IFileManageService _fileManageService;

        public FileController(IFileManageService fileManageService)
        {
            _fileManageService = fileManageService;
        }

        [HttpPost]
        [Permission("fileAdd")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadViewModel fileUploadModel)
        {
            var result = await _fileManageService.UploadAsync(fileUploadModel);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpGet("list")]
        [Permission("fileList")]
        public async Task<IActionResult> GetContent([FromQuery] FileQueryModel query)
        {
            var result = await _fileManageService.GetContentsAsync(query);
            return Ok(result);
        }

        [HttpDelete]
        [Permission("fileDelete")]
        [LogFilter("内容管理", "文件管理", "删除文件")]
        public async Task<IActionResult> DeleteFile([FromQuery] FileViewModel viewModel)
        {
            var isSuccess = await _fileManageService.DeleteFileAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败！");
            }
            return Ok();
        }

        [HttpGet]
        [Permission("fileGet")]
        [LogFilter("内容管理", "文件管理", "下载文件")]
        public async Task<IActionResult> DownloadFile([FromQuery] FileViewModel viewModel)
        {
            var stream = await _fileManageService.DownloadAsync(viewModel);
            return File(stream, "application/octet-stream", viewModel.FileName);
        }
    }
}