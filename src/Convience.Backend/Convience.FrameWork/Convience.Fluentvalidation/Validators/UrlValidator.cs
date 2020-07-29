using FluentValidation.Validators;

using System.Text.RegularExpressions;

namespace Convience.Fluentvalidation.Validators
{
    public class UrlValidator : PropertyValidator
    {

        // url格式
        private readonly string _urlPattern = @"^(https?)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";

        public UrlValidator() : base("URL格式错误！") { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var url = context.PropertyValue?.ToString();

            // 正则验证
            return Regex.IsMatch(url, _urlPattern);
        }
    }
}
