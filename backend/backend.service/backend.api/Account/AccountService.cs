using backend.jwtauthentication;
using backend.repository.backend.api;
using Backend.Jwtauthentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.service.backend.api.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(IUserRepository userRepository, IJwtFactory jwtFactory)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
        }

        public async Task<string> ValidateCredentials(string userName, string password)
        {
            var user = await _userRepository.GetUserByName(userName);
            if (user != null)
            {
                var isValid = await _userRepository.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    var pairs = new List<(string, string)>
                    {
                        (CustomClaimTypes.UserName,user.UserName)
                    };
                    return _jwtFactory.GenerateJwtToken(pairs);
                }
            }
            return string.Empty;
        }

        public async Task<bool> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByName(userName);
            if (user != null)
            {
                var isValid = await _userRepository.ChangePasswordAsync(user, oldPassword, newPassword);
                if (isValid)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
