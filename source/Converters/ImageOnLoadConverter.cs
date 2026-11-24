using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BIMPlugins.Windows.Converters
{
    public class ImageOnLoadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = value as string;

            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            // An image that cannot be loaded (a relative path, a missing, locked or broken file) shows nothing instead of
            // failing the window.
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch (Exception e) when (e is UriFormatException || e is IOException || e is UnauthorizedAccessException ||
                                      e is NotSupportedException || e is FormatException || e is ArgumentException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}