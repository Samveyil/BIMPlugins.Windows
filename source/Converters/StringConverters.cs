using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BIMPlugins.Windows.Converters
{
    public class EscapeUnderscoreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("_", "__");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("__", "_");
            return value;
        }
    }

    public class NumberToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0%";

            // A string is IConvertible too, so it is parsed first: Convert.ToDouble throws on text that is not a number.
            if (value is string strValue)
            {
                return double.TryParse(strValue, out double parsedNumber) ? $"{parsedNumber}%" : "0%";
            }

            if (value is IConvertible numericValue)
            {
                try
                {
                    return $"{System.Convert.ToDouble(numericValue)}%";
                }
                catch (Exception e) when (e is InvalidCastException || e is FormatException || e is OverflowException)
                {
                    return "0%";
                }
            }

            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    public class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Path.GetFileName(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}