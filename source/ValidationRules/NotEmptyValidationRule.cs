using BIMPlugins.Windows.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace BIMPlugins.Windows.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, Resources.Required)
                : ValidationResult.ValidResult;
        }
    }
}
