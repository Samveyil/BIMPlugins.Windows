using Autodesk.Revit.DB;

namespace BIMPlugins.Windows.Converters
{
    /// <summary><see cref="ViewDuplicateOption"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class ViewDuplicateOptionToStringConverter : EnumToLocalizedStringConverter
    {
        /// <summary>Creates the converter for <see cref="ViewDuplicateOption"/>.</summary>
        public ViewDuplicateOptionToStringConverter() => EnumType = typeof(ViewDuplicateOption);
    }
}
