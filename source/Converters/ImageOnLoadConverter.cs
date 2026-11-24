using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BIMPlugins.Windows.Converters
{
    /// <summary>
    /// An image from a full file path, read at once so that the file is not kept open: it can then be replaced or deleted
    /// while the window shows it, as a plain <c>Source="{Binding Path}"</c> would not allow.
    /// <c>Source="{Binding ImagePath, Converter={StaticResource ImageOnLoad}}"</c>.
    /// </summary>
    /// <remarks>
    /// An empty path, a relative one, and a file that is missing, locked or not an image show nothing.
    /// </remarks>
    public class ImageOnLoadConverter : IValueConverter
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}