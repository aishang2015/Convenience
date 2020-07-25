using FluentValidation.Validators;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Convience.Fluentvalidation.Validators
{
    public class EmailValidator : PropertyValidator
    {
        private readonly string _emailPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public EmailValidator() : base("邮箱格式错误！") { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var email = context.PropertyValue?.ToString();

            // 正则验证
            return Regex.IsMatch(email, _emailPattern);
        }
    }
}
