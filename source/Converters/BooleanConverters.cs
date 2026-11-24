using System;
using System.Globalization;
using System.Windows.Data;
using Binding = System.Windows.Data.Binding;

namespace BIMPlugins.Windows.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string radioValue)
            {
                return radioValue.Equals(value?.ToString());
            }
            return false;
        }

        // The checked radio button writes its value to the source; an unchecked one leaves it as it is.
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

    public class NullableToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}