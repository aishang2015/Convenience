
using Convience.Entity.Data;
using Convience.JwtAuthentication;
using Convience.Model.Models.Account;
using Convience.Util.Helpers;

using EasyCaching.Core;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.Account
{
    public interface IAccountService
    {
        public Task<CaptchaResultModel> GetCaptcha();

        public Task<string> ValidateCaptcha(string captchaKey, string captchaValue);

        public Task<bool> IsStopUsing(string userName);

        public Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password);

        public Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        private readonly IEasyCachingProvider _cachingProvider;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(IUserRepository userRepository,
            IEasyCachingProvider cachingProvider,
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

        public async Task<CaptchaResultModel> GetCaptcha()
        {
            var randomValue = CaptchaHelper.GetValidateCode(5);
            var imageData = CaptchaHelper.CreateBase64Image(randomValue);
            var key = Guid.NewGuid().ToString();
            await _cachingProvider.SetAsync(key, randomValue, TimeSpan.FromMinutes(2));
            return new CaptchaResultModel
            {
                CaptchaKey = key,
                CaptchaData = imageData
            };
        }

        public async Task<string> ValidateCaptcha(string captchaKey, string captchaValue)
        {
            var value = await _cachingProvider.GetAsync(captchaKey, typeof(string));
            if (!string.IsNullOrEmpty(value?.ToString()))
            {
                return captchaValue == value.ToString() ? string.Empty : "验证码错误！";
            }
            return "验证码已过期！";
        }
    }
}
