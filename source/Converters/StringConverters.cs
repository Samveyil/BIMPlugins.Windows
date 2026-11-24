using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// Doubles underscores, so that a Label, CheckBox or Button shows a name such as <c>A_101</c> as it is: their content
    /// takes a single underscore as the mark of an access key and hides it.
    /// <c>Content="{Binding Name, Converter={StaticResource EscapeUnderscore}}"</c>.
    /// </summary>
    public class EscapeUnderscoreConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("_", "__");
            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("__", "_");
            return value;
        }
    }

    /// <summary>
    /// A number, or text that is one, followed by a percent sign: <c>42</c> as <c>42%</c>. Anything else, null included,
    /// shows <c>0%</c>. The number is not multiplied by 100, and it is written and read in the current culture.
    /// </summary>
    public class NumberToPercentConverter : IValueConverter
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    /// <summary>The file's name with its extension from a full path: <c>C:\Clashes\report.xml</c> as <c>report.xml</c>.</summary>
    public class FilePathToFileNameConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Path.GetFileName(value as string);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
