using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Binding = System.Windows.Data.Binding;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// <see cref="TrueValue"/> when the value equals the parameter, else <see cref="FalseValue"/>. Both are compared as text,
    /// ignoring case, so that the string parameter from XAML matches a number, an enum value or a bool:
    /// <c>Visibility="{Binding ModeIndex, Converter={StaticResource EqualsToVisibility}, ConverterParameter=2}"</c>.
    /// </summary>
    /// <remarks>
    /// Numbers are written in the invariant culture: a fractional parameter takes a point, as in <c>1.5</c>.
    /// A null value equals a missing parameter.
    /// </remarks>
    public class EqualsToVisibilityConverter : IValueConverter
    {
        /// <summary>The visibility when the value equals the parameter; Visible by default.</summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;

        /// <summary>The visibility when it does not; Collapsed by default.</summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            string.Equals(System.Convert.ToString(value, CultureInfo.InvariantCulture),
                System.Convert.ToString(parameter, CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase)
                ? TrueValue
                : FalseValue;

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
