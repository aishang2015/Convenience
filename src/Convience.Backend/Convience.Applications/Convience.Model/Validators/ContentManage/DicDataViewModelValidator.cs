using Convience.Model.Models.ContentManage;

using FluentValidation;

namespace Convience.Model.Validators.ContentManage
{
    public class DicDataViewModelValidator : AbstractValidator<DicDataViewModel>
    {
        public DicDataViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).NotEmpty().NotNull()
                .WithMessage("字典数据名不能为空！");
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("字典数据名长度不能超过15！");
            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("字典数据名排序长度过长！");
        }
    }
}