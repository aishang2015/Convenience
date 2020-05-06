using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;

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
        public async Task<IActionResult> MakeDirectory([FromBody]FileViewModel vm)
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
        public async Task<IActionResult> DeleteDirectory([FromQuery]FileViewModel vm)
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