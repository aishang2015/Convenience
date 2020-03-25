using Backend.Model.backend.api.Models.SystemManage;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model.backend.api.Validators.SystemManage
{
    public class MenuViewModelValidator : AbstractValidator<MenuViewModel>
    {
        public MenuViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("菜单名称长度不能超过15！");

            RuleFor(viewmodel => viewmodel.Name).NotNull().NotEmpty()
                .WithMessage("菜单名称不能为空！");
        }
    }
}
