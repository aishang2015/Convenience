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
    public class DicTypeController : ControllerBase
    {
        private readonly IDicTypeService _dicTypeService;

        public DicTypeController(IDicTypeService dicTypeService)
        {
            _dicTypeService = dicTypeService;
        }

        [HttpGet]
        [Permission("dicTypeGet")]
        public async Task<IActionResult> GetById(int id)
        {
            var dictype = await _dicTypeService.GetByIdAsync(id);
            return Ok(dictype);
        }

        [HttpGet("list")]
        [Permission("dicTypeList")]
        public IActionResult GetDicTypeList()
        {
            var dicdata = _dicTypeService.GetDicTypes();
            return Ok(dicdata);
        }

        [HttpDelete]
        [Permission("dicTypeDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _dicTypeService.DeleteDicTypeAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("dicTypeAdd")]
        public async Task<IActionResult> Add(DicTypeViewModel dictypeViewModel)
        {
            var codeExist = _dicTypeService.HasSameCode(dictypeViewModel.Code);
            if (codeExist)
            {
                return this.BadRequestResult("字典类型编码已经存在，请修改!");
            }

            var isSuccess = await _dicTypeService.AddDicTypeAsync(dictypeViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("dicTypeUpdate")]
        public async Task<IActionResult> Update(DicTypeViewModel dictypeViewModel)
        {
            var codeExist = _dicTypeService.HasSameCode(dictypeViewModel.Id, dictypeViewModel.Code);
            if (codeExist)
            {
                return this.BadRequestResult("字典类型编码已经存在，请修改!");
            }

            var isSuccess = await _dicTypeService.UpdateDicTypeAsync(dictypeViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}