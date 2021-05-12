using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models.Account;
using Convience.Util.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.Account
{
    public interface IAccountService
    {
        public CaptchaResultModel GetCaptcha();

        public string ValidateCaptcha(string captchaKey, string captchaValue);

        public bool IsStopUsing(string userName);

        public Task<ValidateCredentialsResultModel> ValidateCredentialsAsync(string userName, string password);

        public Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<SystemUser> _userManager;

        private readonly IRepository<SystemUserRole> _userRoleRepository;

        private readonly IMemoryCache _cachingProvider;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(
            UserManager<SystemUser> userManager,
            IRepository<SystemUserRole> userRoleRepository,
            IMemoryCache cachingProvider,
            IOptionsSnapshot<JwtOption> jwtOptionAccessor)
        {
            var option = jwtOptionAccessor.Get(JwtAuthenticationSchemeConstants.DefaultAuthenticationScheme);
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _cachingProvider = cachingProvider;
            _jwtFactory = new JwtFactory(option);
        }

        public bool IsStopUsing(string userName)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);
            return user != null;
        }

        public async Task<ValidateCredentialsResultModel> ValidateCredentialsAsync(string userName, string password)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);
            var roleIds = string.Join(',',
                _userRoleRepository.Get(ur => ur.UserId == user.Id).Select(ur => ur.RoleId));
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, password);
                if (isValid)
                {
                    var pairs = new List<(string, string)>
                    {
                        (CustomClaimTypes.UserName,user.UserName),
                        (CustomClaimTypes.UserRoleIds,roleIds),
                        (CustomClaimTypes.Name,user.Name)
                    };
                    return new ValidateCredentialsResultModel(_jwtFactory.GenerateJwtToken(pairs),
                        user.Name, user.Avatar, roleIds);
                }
            }
            return null;
        }

        public async Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == userName);
            return user == null ? false :
                (await _userManager.ChangePasswordAsync(user, oldPassword, newPassword)).Succeeded;

        }

        public CaptchaResultModel GetCaptcha()
        {
            var randomValue = CaptchaHelper.GetValidateCode(5);
            var imageData = CaptchaHelper.CreateBase64Image(randomValue);
            var key = Guid.NewGuid().ToString();
            _cachingProvider.Set(key, randomValue, TimeSpan.FromMinutes(2));
            return new CaptchaResultModel(key, imageData);
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
