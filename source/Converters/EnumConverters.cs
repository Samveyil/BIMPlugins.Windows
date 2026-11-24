using BIMPlugins.Windows.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// Shows an enum value as its text from the resources, by the key <c>EnumType_Value</c>, in the language of the UI;
    /// a value with no text shows its name. Back, it takes the text in the UI language, in Russian or in English, or the
    /// value's name.
    /// </summary>
    /// <example>
    /// Better with the values themselves in the list, so that nothing is converted back:
    /// <code>
    /// &lt;ComboBox ItemsSource="{Binding Options}" SelectedItem="{Binding SelectedOption}"&gt;
    ///     &lt;ComboBox.ItemTemplate&gt;
    ///         &lt;DataTemplate&gt;
    ///             &lt;TextBlock Text="{Binding Converter={StaticResource EnumToLocalizedStringConverter}}" /&gt;
    ///         &lt;/DataTemplate&gt;
    ///     &lt;/ComboBox.ItemTemplate&gt;
    /// &lt;/ComboBox&gt;
    /// </code>
    /// </example>
    public class EnumToLocalizedStringConverter : IValueConverter
    {
        private static readonly CultureInfo[] BackCultures = [new CultureInfo("ru-RU"), CultureInfo.InvariantCulture];

        /// <summary>The enum to convert text back to, when the binding's source type does not tell it.</summary>
        public Type EnumType { get; set; }

        /// <summary>The text of an enum value in the language of the UI, or its name when the resources have none.</summary>
        public static string GetText(Enum value) => GetText(value, Resources.Culture);

        private static string GetText(Enum value, CultureInfo culture) =>
            Resources.ResourceManager.GetString($"{value.GetType().Name}_{value}", culture) ?? value.ToString();

        // The culture WPF passes is the element's language (en-US unless set), not the UI language, so it is not used.
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Enum enumValue ? GetText(enumValue) : value;

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = EnumType ?? Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (value is not string text || type is not { IsEnum: true })
                return Binding.DoNothing;

            foreach (Enum enumValue in Enum.GetValues(type))
            {
                if (text == GetText(enumValue) || text == enumValue.ToString() ||
                    BackCultures.Any(c => text == GetText(enumValue, c)))
                    return enumValue;
            }

            return Binding.DoNothing;
        }
    }
}
