using Convience.Model.Models.GroupManage;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Model.Validators.GroupManage
{
    public class EmployeeQueryValidator : AbstractValidator<EmployeeQuery>
    {
        public EmployeeQueryValidator()
        {
            RuleFor(viewmodel => viewmodel.Size).Must(size => size == 10 || size == 20 || size == 30 || size == 40).WithMessage("错误的长度！");

            RuleFor(viewmodel => viewmodel.UserName).MaximumLength(15).WithMessage("检索内容过长！");
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(10).WithMessage("检索内容过长！");
            RuleFor(viewmodel => viewmodel.PhoneNumber).MaximumLength(11).WithMessage("检索内容过长！");
        }
    }
}
