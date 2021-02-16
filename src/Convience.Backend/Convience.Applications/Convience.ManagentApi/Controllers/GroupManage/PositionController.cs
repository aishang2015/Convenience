using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.GroupManage;
using Convience.Service.GroupManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.GroupManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet()]
        [Permission("positionGet")]
        public async Task<IActionResult> GetPosition([FromQuery] int id)
        {
            var result = await _positionService.GetPositionAsync(id);
            return Ok(result);
        }

        [HttpGet("dic")]
        [Permission("positionDic")]
        public IActionResult GetPositionDic([FromQuery] string name)
        {
            return Ok(_positionService.GetPositionDic(name));
        }

        [HttpGet("all")]
        [Permission("allPosition")]
        public IActionResult GetAllPosition()
        {
            return Ok(_positionService.GetAllPosition());
        }

        [HttpGet("list")]
        [Permission("positionList")]
        public IActionResult Get([FromQuery] PositionQueryModel positionQuery)
        {
            return Ok(new
            {
                Data = _positionService.GetPositions(positionQuery),
                Count = _positionService.Count()
            });
        }

        [HttpDelete]
        [Permission("positionDelete")]
        [LogFilter("组织管理", "职位管理", "删除职位")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var isSuccess = await _positionService.DeletePositionAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("positionAdd")]
        [LogFilter("组织管理", "职位管理", "创建职位")]
        public async Task<IActionResult> Add(PositionViewModel viewModel)
        {
            var isSuccess = await _positionService.AddPositionAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("positionUpdate")]
        [LogFilter("组织管理", "职位管理", "更新职位")]
        public async Task<IActionResult> Update(PositionViewModel viewModel)
        {
            var isSuccess = await _positionService.UpdatePositionAsync(viewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}