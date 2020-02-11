using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.model.AccountViewModels;
using backend.service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _loginService.ValidateCredentials(model.UserName, model.Password);
            if (string.IsNullOrEmpty(result))
            {
                ModelState.AddModelError(string.Empty, "错误的用户名或密码！");
                return BadRequest(ModelState);
            }
            return Ok(new LoginResult
            {
                UserName = model.UserName,
                Token = result
            });
        }
    }
}