
using Backend.Model.backend.api.Models.AccountViewModels;

using FluentValidation;

namespace backend.model.backend.api.Validators.AccountViewModels
{
    public class ChangePwdViewModelValidator : AbstractValidator<ChangePwdViewModel>
    {
        public ChangePwdViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.OldPassword).NotEmpty().NotNull()
                .WithMessage("旧密码不能为空");

            RuleFor(viewmodel => viewmodel.NewPassword).NotEmpty().NotNull()
                .WithMessage("新密码不能为空");

            RuleFor(viewmodel => viewmodel.NewPassword).NotEqual(viewmodel => viewmodel.OldPassword)
                .WithMessage("旧密码不能和新密码相同");
        }
    }
}
