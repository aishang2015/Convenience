using Convience.Model.Models.SaasManage;

using FluentValidation;

namespace Convience.Model.Validators.SaasManage
{
    public class TenantViewModelValidator : AbstractValidator<TenantViewModel>
    {
        public TenantViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("名称长度不能超过15!");

            RuleFor(viewmodel => viewmodel.UrlPrefix).MaximumLength(20)
                .WithMessage("url前缀长度不能超过20!");

            RuleFor(viewmodel => viewmodel.ConnectionString).MaximumLength(150)
                .WithMessage("连接字符串长度不能超过150!");
        }
    }
}
