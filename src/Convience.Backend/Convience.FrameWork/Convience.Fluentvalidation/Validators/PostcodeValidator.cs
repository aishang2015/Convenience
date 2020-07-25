using FluentValidation.Validators;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Convience.Fluentvalidation.Validators
{
    public class PostcodeValidator : PropertyValidator
    {
        public PostcodeValidator() : base("邮政编码格式错误！") { }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var postcode = context.PropertyValue?.ToString();

            // 正则验证
            return Regex.IsMatch(postcode, @"\d{6}");
        }
    }
}
