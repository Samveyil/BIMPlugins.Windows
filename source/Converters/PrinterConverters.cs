using Autodesk.Revit.DB;

namespace BIMPlugins.Windows.Converters
{
    /// <summary><see cref="RasterQualityType"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class RasterQualityTypeToStringConverter : EnumToLocalizedStringConverter
    {
        /// <summary>Creates the converter for <see cref="RasterQualityType"/>.</summary>
        public RasterQualityTypeToStringConverter() => EnumType = typeof(RasterQualityType);
    }

    /// <summary><see cref="ColorDepthType"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class ColorDepthTypeToStringConverter : EnumToLocalizedStringConverter
    {
        /// <summary>Creates the converter for <see cref="ColorDepthType"/>.</summary>
        public ColorDepthTypeToStringConverter() => EnumType = typeof(ColorDepthType);
    }
}
