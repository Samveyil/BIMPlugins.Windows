using BIMPlugins.Windows.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace BIMPlugins.Windows.ValidationRules
{
    /// <summary>
    /// Fails on an empty value or one of spaces alone, with "Required field." in the language of the UI.
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;TextBox&gt;
    ///     &lt;TextBox.Text&gt;
    ///         &lt;Binding Path="Name" UpdateSourceTrigger="PropertyChanged"&gt;
    ///             &lt;Binding.ValidationRules&gt;
    ///                 &lt;rules:NotEmptyValidationRule ValidatesOnTargetUpdated="True" /&gt;
    ///             &lt;/Binding.ValidationRules&gt;
    ///         &lt;/Binding&gt;
    ///     &lt;/TextBox.Text&gt;
    /// &lt;/TextBox&gt;
    /// </code>
    /// </example>
    public class NotEmptyValidationRule : ValidationRule
    {
        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, Resources.Required)
                : ValidationResult.ValidResult;
        }
    }
}
