using Convience.Model.Models.GroupManage;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Model.Validators.GroupManage
{
    public class PositionQueryValidator : AbstractValidator<PositionQuery>
    {
        public PositionQueryValidator()
        {
            RuleFor(viewmodel => viewmodel.Size).Must(size => size == 10 || size == 20 || size == 30 || size == 40)
                .WithMessage("错误的长度！");
        }
    }
}
