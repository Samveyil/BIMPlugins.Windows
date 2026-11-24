using Autodesk.Revit.DB;

namespace BIMPlugins.Windows.Converters
{
    /// <summary><see cref="RasterQualityType"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class RasterQualityTypeToStringConverter : EnumToLocalizedStringConverter
    {
        public RasterQualityTypeToStringConverter() => EnumType = typeof(RasterQualityType);
    }

    /// <summary><see cref="ColorDepthType"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class ColorDepthTypeToStringConverter : EnumToLocalizedStringConverter
    {
        public ColorDepthTypeToStringConverter() => EnumType = typeof(ColorDepthType);
    }
}
