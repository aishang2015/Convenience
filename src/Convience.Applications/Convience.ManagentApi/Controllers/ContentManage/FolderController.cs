using Convience.Fluentvalidation;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.ContentManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFileManageService _fileManageService;

        public FolderController(IFileManageService fileManageService)
        {
            _fileManageService = fileManageService;
        }

        [HttpPost]
        public async Task<IActionResult> MakeDirectory([FromBody]FileViewModel vm)
        {
            var result = await _fileManageService.MakeDirectoryAsync(vm.Directory);
            if (!string.IsNullOrEmpty(result))
            {
                return this.BadRequestResult(result);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDirectory([FromQuery]string directory)
        {
            var isSuccess = await _fileManageService.DeleteDirectoryAsync(new FileViewModel
            {
                FileName = string.Empty,
                Directory = directory
            });
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败！");
            }
            return Ok();
        }
    }
}