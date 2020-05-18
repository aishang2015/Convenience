using System.Globalization;
using System.Windows.Controls;

namespace Convience.WPFClient.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入内容！")
                : ValidationResult.ValidResult;
        }
    }
}
