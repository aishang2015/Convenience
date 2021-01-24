
using Convience.Entity.Data;
using Convience.JwtAuthentication;
using Convience.Model.Models.Account;
using Convience.Util.Helpers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.Account
{
    public interface IAccountService
    {
        public CaptchaResultModel GetCaptcha();

        public string ValidateCaptcha(string captchaKey, string captchaValue);

        public Task<bool> IsStopUsing(string userName);

        public Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password);

        public Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMemoryCache _cachingProvider;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(IUserRepository userRepository,
            IMemoryCache cachingProvider,
            IOptionsSnapshot<JwtOption> jwtOptionAccessor)
        {
            var option = jwtOptionAccessor.Get(JwtAuthenticationSchemeConstants.DefaultAuthenticationScheme);
            _userRepository = userRepository;
            _cachingProvider = cachingProvider;
            _jwtFactory = new JwtFactory(option);
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

        public async Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    return (false, "此账号未激活！", null);
                }
                var isValid = await _userRepository.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    var pairs = new List<(string, string)>
                    {
                        (CustomClaimTypes.UserName,user.UserName),
                        (CustomClaimTypes.UserRoleIds,user.RoleIds),
                        (CustomClaimTypes.Name,user.Name)
                    };
                    return (true, _jwtFactory.GenerateJwtToken(pairs), user);
                }
            }
            return (false, "错误的用户名或密码！", null);
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

        public CaptchaResultModel GetCaptcha()
        {
            var randomValue = CaptchaHelper.GetValidateCode(5);
            var imageData = CaptchaHelper.CreateBase64Image(randomValue);
            var key = Guid.NewGuid().ToString();
            _cachingProvider.Set(key, randomValue, TimeSpan.FromMinutes(2));
            return new CaptchaResultModel
            {
                CaptchaKey = key,
                CaptchaData = imageData
            };
        }

        public string ValidateCaptcha(string captchaKey, string captchaValue)
        {
            var value = _cachingProvider.Get(captchaKey);
            if (value != null)
            {
                return captchaValue == value.ToString() ? string.Empty : "验证码错误！";
            }
            return "验证码已过期！";
        }
    }
}
