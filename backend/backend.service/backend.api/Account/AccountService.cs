using backend.jwtauthentication;
using backend.repository.backend.api;

using Backend.Jwtauthentication;

using System.Collections.Generic;
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

        public async Task<bool> IsStopUsing(string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null && user.IsActive)
            {
                return true;
            }
            return false;
        }

        public async Task<(bool, string)> ValidateCredentials(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    return (false, "此账号未激活！");
                }
                var isValid = await _userRepository.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    var pairs = new List<(string, string)>
                    {
                        (CustomClaimTypes.UserName,user.UserName)
                    };
                    return (true, _jwtFactory.GenerateJwtToken(pairs));
                }
            }
            return (false, "错误的用户名或密码！");
        }

        public async Task<bool> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
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
