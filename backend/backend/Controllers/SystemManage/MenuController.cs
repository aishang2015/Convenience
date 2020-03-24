using backend.fluentvalidation;
using Backend.Model.backend.api.Models.SystemManage;
using Backend.Service.backend.api.SystemManage.Menu;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Api.Controllers.SystemManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_menuService.GetAllMenu());
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var isSuccess = await _menuService.DeleteMenuAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MenuViewModel menuViewModel)
        {
            var isSuccess = await _menuService.AddMenuAsync(menuViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update(MenuViewModel menuViewModel)
        {
            var isSuccess = await _menuService.UpdateMenuAsync(menuViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }

    }
}