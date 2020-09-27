using Convience.JwtAuthentication.AuthorizeAttributes;
using Convience.Service.TenantService;
using Microsoft.AspNetCore.Mvc;

namespace Convience.ManagentApi.TenantControllers
{
    [Route("api/tenant/[controller]")]
    [ApiController]
    [MemberAuthorize]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_memberService.GetAllMemeber());
        }
    }
}
