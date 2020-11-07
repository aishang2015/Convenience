using Convience.Model.Models.SystemManage;

using FluentValidation;

namespace Convience.Model.Validators.SystemManage
{
    public class MenuViewModelValidator : AbstractValidator<MenuViewModel>
    {
        public MenuViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("菜单名称长度不能超过15！");

            RuleFor(viewmodel => viewmodel.Name).NotNull().NotEmpty()
                .WithMessage("菜单名称不能为空！");

            RuleFor(viewmodel => viewmodel.Identification).MaximumLength(50)
                .WithMessage("前端识别长度不能超过50！");

            RuleFor(viewmodel => viewmodel.Permission).MaximumLength(2000)
                .WithMessage("后端权限长度不能超过2000！");

            RuleFor(viewmodel => viewmodel.Route).MaximumLength(50)
                .WithMessage("路由长度不能超过50！");

            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("菜单排序长度过长！");
        }
    }
}
