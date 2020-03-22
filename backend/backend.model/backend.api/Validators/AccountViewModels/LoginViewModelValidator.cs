
using Backend.Model.backend.api.Models.AccountViewModels;

using FluentValidation;

namespace backend.model.backend.api.Validators.AccountViewModels
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.UserName).NotNull().NotEmpty()
                .WithMessage("用户名不能为空！");

            RuleFor(viewmodel => viewmodel.Password).NotNull().NotEmpty()
                .WithMessage("密码不能为空！");

        }
    }
}
