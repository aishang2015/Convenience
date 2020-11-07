using Convience.Model.Models.ContentManage;

using FluentValidation;

namespace Convience.Model.Validators.ContentManage
{
    public class ColumnViewModelValidator : AbstractValidator<ColumnViewModel>
    {
        public ColumnViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("文章栏目长度不能超过15！");
            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("文章栏目排序长度过长！");
        }
    }
}
