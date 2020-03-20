using Backend.Model.backend.api.Models.SystemManage;
using FluentValidation;

namespace Backend.Model.backend.api.Validators.SystemManage
{
    public class RoleViewModelValidator : AbstractValidator<RoleViewModel>
    {
        public RoleViewModelValidator()
        {

            RuleFor(viewmodel => viewmodel.Name).NotNull().NotEmpty()
                .WithMessage("角色名不能为空！");

            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("角色名长度不能超过15！");

            RuleFor(viewmodel => viewmodel.Remark).MaximumLength(30)
                .WithMessage("备注长度不能超过30！");
        }
    }
}
