using Convience.Model.Models.ContentManage;

using FluentValidation;

namespace Convience.Model.Validators.ContentManage
{
    public class DicDataViewModelValidator : AbstractValidator<DicDataViewModel>
    {
        public DicDataViewModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Name).NotEmpty().NotNull()
                .WithMessage("�ֵ�����������Ϊ�գ�");
            RuleFor(viewmodel => viewmodel.Name).MaximumLength(15)
                .WithMessage("�ֵ����������Ȳ��ܳ���15��");
            RuleFor(viewmodel => viewmodel.Sort).LessThan(999999999)
                .WithMessage("�ֵ����������򳤶ȹ�����");
        }
    }
}