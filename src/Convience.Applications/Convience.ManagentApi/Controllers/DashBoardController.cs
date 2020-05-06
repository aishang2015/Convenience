using Convience.Service.DashBoard;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashBoardService;

        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _dashBoardService.GetAsync();
            return Ok(result);
        }
    }
}