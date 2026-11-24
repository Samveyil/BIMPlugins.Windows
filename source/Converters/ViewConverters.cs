using Autodesk.Revit.DB;

namespace BIMPlugins.Windows.Converters
{
    /// <summary><see cref="ViewDuplicateOption"/> as text and back. See <see cref="EnumToLocalizedStringConverter"/>.</summary>
    public class ViewDuplicateOptionToStringConverter : EnumToLocalizedStringConverter
    {
        public ViewDuplicateOptionToStringConverter() => EnumType = typeof(ViewDuplicateOption);
    }
}
