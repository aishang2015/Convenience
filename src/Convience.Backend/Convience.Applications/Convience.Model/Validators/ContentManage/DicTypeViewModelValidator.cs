using Convience.Model.Models.ContentManage;

using FluentValidation;

namespace Convience.Model.Validators.ContentManage
{
    public class DicTypeViewModelValidator : AbstractValidator<DicTypeViewModel>
    {
        public DicTypeViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).NotEmpty().NotNull()
                .WithMessage("字典类型名不能为空！");
            RuleFor(viewmodel => viewmodel.Code).NotEmpty().NotNull()
                .WithMessage("字典类型编码不能为空！");
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("字典类型名长度不能超过15！");
            RuleFor(viewmodel => viewmodel.Code).MaximumLength(15)
                .WithMessage("字典类型编码长度不能超过15！");
            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("字典类型编码排序长度过长！");
        }
    }
}