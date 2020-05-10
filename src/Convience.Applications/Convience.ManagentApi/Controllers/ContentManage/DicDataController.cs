using Convience.Fluentvalidation;
using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DicDataController : ControllerBase
    {
        private readonly IDicDataService _dicDataService;

        public DicDataController(IDicDataService dicDataService)
        {
            _dicDataService = dicDataService;
        }

        [HttpGet]
        [Permission("dicDataGet")]
        public async Task<IActionResult> GetById(int id)
        {
            var dicdata = await _dicDataService.GetByIdAsync(id);
            return Ok(dicdata);
        }

        [HttpGet("list")]
        [Permission("dicDataList")]
        public IActionResult GetDicDataList(int dicTypeId)
        {
            var dicdata = _dicDataService.GetByDicTypeIdAsync(dicTypeId);
            return Ok(dicdata);
        }

        [HttpDelete]
        [Permission("dicDataDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _dicDataService.DeleteDicDataAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("dicDataAdd")]
        public async Task<IActionResult> Add(DicDataViewModel dicdataViewModel)
        {
            var isSuccess = await _dicDataService.AddDicDataAsync(dicdataViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("dicDataUpdate")]
        public async Task<IActionResult> Update(DicDataViewModel dicdataViewModel)
        {
            var isSuccess = await _dicDataService.UpdateDicDataAsync(dicdataViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}