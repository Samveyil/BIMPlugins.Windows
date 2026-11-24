using System;
using System.Globalization;
using System.Windows.Data;
using Binding = System.Windows.Data.Binding;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// For a group of radio buttons bound to one property: a button is checked when the property's text equals its
    /// parameter, and checking it writes the parameter to the property.
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;RadioButton Content="Low" IsChecked="{Binding Quality, Converter={StaticResource StringToBool}, ConverterParameter=Low}" /&gt;
    /// &lt;RadioButton Content="High" IsChecked="{Binding Quality, Converter={StaticResource StringToBool}, ConverterParameter=High}" /&gt;
    /// </code>
    /// The property can be a string, an enum or a number: the parameter is converted to its type.
    /// </example>
    public class StringToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string radioValue)
            {
                return radioValue.Equals(value?.ToString());
            }
            return false;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The checked button writes its parameter; an unchecked one, and a parameter that does not convert to the
        /// property's type, leave the property as it is.
        /// </remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not true || parameter is not string radioValue)
                return Binding.DoNothing;

            var type = Nullable.GetUnderlyingType(targetType) ?? targetType;
            try
            {
                if (type == typeof(string) || type == typeof(object))
                    return radioValue;

                return type.IsEnum
                    ? Enum.Parse(type, radioValue)
                    : System.Convert.ChangeType(radioValue, type, culture);
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
