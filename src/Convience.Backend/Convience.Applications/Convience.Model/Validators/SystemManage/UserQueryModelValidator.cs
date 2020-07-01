using Convience.Model.Models.SystemManage;

using FluentValidation;

namespace Convience.Model.Validators.SystemManage
{
    public class UserQueryModelValidator : AbstractValidator<UserQueryModel>
    {
        public UserQueryModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Size).Must(size => size == 10 || size == 20 || size == 30 || size == 40)
                .WithMessage("错误的长度！");

            RuleFor(viewmodel => viewmodel.UserName).MaximumLength(15).WithMessage("检索内容过长！");
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(10).WithMessage("检索内容过长！");
            RuleFor(viewmodel => viewmodel.PhoneNumber).MaximumLength(11).WithMessage("检索内容过长！");
            RuleFor(viewmodel => viewmodel.RoleId).MaximumLength(15).WithMessage("检索内容过长！");
        }
    }
}
