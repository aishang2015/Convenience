using backend.core.Authentication;
using backend.data.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.service
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IJwtFactory _jwtFactory;

        public LoginService(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        public async Task<string> ValidateCredentials(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    return _jwtFactory.GenerateJwtToken();
                }
            }
            return string.Empty;
        }
    }
}
