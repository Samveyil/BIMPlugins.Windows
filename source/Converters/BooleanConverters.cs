using System;
using System.Globalization;
using System.Windows.Data;
using Binding = System.Windows.Data.Binding;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// True when the value's text equals <c>ConverterParameter</c>, for any bool property: IsChecked, IsEnabled,
    /// IsReadOnly and so on. Back, true writes the parameter to the source, converted to its type (a string, an enum or
    /// a number), and false leaves the source as it is.
    /// </summary>
    /// <example>
    /// Enable a button in one mode only:
    /// <code>
    /// &lt;Button IsEnabled="{Binding Mode, Converter={StaticResource StringToBool}, ConverterParameter=Edit}" /&gt;
    /// </code>
    /// Choose one of several values, as radio buttons or toggle buttons do:
    /// <code>
    /// &lt;RadioButton Content="Low" IsChecked="{Binding Quality, Converter={StaticResource StringToBool}, ConverterParameter=Low}" /&gt;
    /// &lt;RadioButton Content="High" IsChecked="{Binding Quality, Converter={StaticResource StringToBool}, ConverterParameter=High}" /&gt;
    /// </code>
    /// </example>
    public class StringToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string expected)
            {
                return expected.Equals(value?.ToString());
            }
            return false;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// True writes the parameter; false, and a parameter that does not convert to the source's type, leave the
        /// source as it is.
        /// </remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not true || parameter is not string expected)
                return Binding.DoNothing;

            var type = Nullable.GetUnderlyingType(targetType) ?? targetType;
            try
            {
                if (type == typeof(string) || type == typeof(object))
                    return expected;

                return type.IsEnum
                    ? Enum.Parse(type, expected)
                    : System.Convert.ChangeType(expected, type, culture);
            }
            catch (Exception e) when (e is ArgumentException || e is FormatException || e is InvalidCastException || e is OverflowException)
            {
                return Binding.DoNothing;
            }
        }
    }

    /// <summary>
    /// True when the value is not null, for enabling a part of a window once something is chosen:
    /// <c>IsEnabled="{Binding SelectedGroup, Converter={StaticResource NullableToBool}}"</c>.
    /// </summary>
    public class NullableToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
