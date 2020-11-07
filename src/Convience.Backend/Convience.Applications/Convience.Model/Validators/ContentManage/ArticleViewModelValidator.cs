using Convience.Model.Models.ContentManage;

using FluentValidation;

namespace Convience.Model.Validators.ContentManage
{
    public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
    {
        public ArticleViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Title).NotEmpty().NotNull()
                .WithMessage("文章标题不能为空！");
            RuleFor(viewmodel => viewmodel.Title).MaximumLength(50)
                .WithMessage("文章标题长度不能超过50！");
            RuleFor(viewmodel => viewmodel.SubTitle).MaximumLength(200)
                .WithMessage("文章副标题长度不能超过200！");
            RuleFor(viewmodel => viewmodel.Source).MaximumLength(200)
                .WithMessage("文章出处长度不能超过200！");
            RuleFor(viewmodel => viewmodel.Tags).MaximumLength(200)
                .WithMessage("文章关键字长度不能超过200！");
            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("文章排序长度过长！");
        }
    }
}
