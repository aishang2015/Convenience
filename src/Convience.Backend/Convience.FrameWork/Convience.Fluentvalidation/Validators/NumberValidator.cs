using FluentValidation.Validators;

using System.Text.RegularExpressions;

namespace Convience.Fluentvalidation.Validators
{
    public class NumberValidator : PropertyValidator
    {
        // 数字格式
        private readonly string _numberPattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";

        protected override string GetDefaultMessageTemplate()
            => "数字格式错误！";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var number = context.PropertyValue?.ToString();

            // 正则验证
            return Regex.IsMatch(number, _numberPattern);
        }
    }
}
