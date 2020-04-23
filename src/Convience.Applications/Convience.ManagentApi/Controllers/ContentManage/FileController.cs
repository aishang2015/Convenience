using Convience.Fluentvalidation;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.ContentManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileManageService _fileManageService;

        public FileController(IFileManageService fileManageService)
        {
            _fileManageService = fileManageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileUploadModel fileUploadModel)
        {
            var result = await _fileManageService.UploadAsync(fileUploadModel);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetContent([FromQuery]FileQuery query)
        {
            var result = await _fileManageService.GetContentsAsync(query);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromQuery]FileViewModel viewModel)
        {
            var isSuccess = await _fileManageService.DeleteFileAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败！");
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile([FromQuery]FileViewModel viewModel)
        {
            var result = await _fileManageService.DownloadAsync(viewModel);
            return Ok(result);
        }
    }
}