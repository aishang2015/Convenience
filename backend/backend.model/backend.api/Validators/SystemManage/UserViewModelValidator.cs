using Backend.Model.backend.api.Models.SystemManage;

using FluentValidation;

namespace Backend.Model.backend.api.Validators.SystemManage
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.UserName).MaximumLength(15).WithMessage("用户名过长！");
            RuleFor(viewmodel => viewmodel.UserName).NotEmpty().NotNull()
                .WithMessage("用户名不能为空！");

            RuleFor(viewmodel => viewmodel.Password).Must((vm, pwd) => (vm.Id == 0 && !string.IsNullOrEmpty(pwd)) || (vm.Id != 0))
                .WithMessage("密码不能为空！");

            RuleFor(viewmodel => viewmodel.Name).MaximumLength(10).WithMessage("人名过长！");
            RuleFor(viewmodel => viewmodel.Name).NotEmpty().NotNull()
                .WithMessage("人名不能为空！");

            RuleFor(viewmodel => viewmodel.PhoneNumber).MaximumLength(11).WithMessage("电话号码过长！");
            RuleFor(viewmodel => viewmodel.Avatar).MaximumLength(5).WithMessage("头像内容过长！");
        }
    }
}
