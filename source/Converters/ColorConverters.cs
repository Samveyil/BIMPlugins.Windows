using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BIMPlugins.Windows.Converters
{
    public class RevitColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // An invalid Revit colour (InvalidColorValue) throws on Red, Green and Blue.
            if (value is Autodesk.Revit.DB.Color { IsValid: true } revitColor)
            {
                var wpfColor = Color.FromRgb(
                    revitColor.Red,
                    revitColor.Green,
                    revitColor.Blue
                );

                var brush = new SolidColorBrush(wpfColor);
                brush.Freeze();
                return brush;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}