using FluentValidation;

using System.Collections.Generic;

namespace Convience.Fluentvalidation.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>
            (this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num)
                .WithMessage("此列表包含过多项目！");
        }

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainMoreThan<T, TElement>
            (this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count >= num)
                .WithMessage("此列表包含过少项目！");
        }

        #region custom property validator

        public static IRuleBuilderOptions<T, string> IsEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new EmailValidator());
        }
        public static IRuleBuilderOptions<T, string> IsIpAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IPValidator());
        }
        public static IRuleBuilderOptions<T, string> IsNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NumberValidator());
        }
        public static IRuleBuilderOptions<T, string> IsPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhoneNumberValidator());
        }
        public static IRuleBuilderOptions<T, string> IsPostCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PostcodeValidator());
        }
        public static IRuleBuilderOptions<T, string> IsUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlValidator());
        }

        #endregion
    }
}
