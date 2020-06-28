using Convience.Model.Models.Account;

using FluentValidation;

namespace Convience.Model.Validators.Account
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.UserName).NotNull().NotEmpty()
                .WithMessage("用户名不能为空！");

            RuleFor(viewmodel => viewmodel.Password).NotNull().NotEmpty()
                .WithMessage("密码不能为空！");

            RuleFor(viewmodel => viewmodel.CaptchaValue).NotNull().NotEmpty()
                .WithMessage("验证码不能为空！");

        }
    }
}
