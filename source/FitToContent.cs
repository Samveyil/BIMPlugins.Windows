using System.Windows;

namespace BIMPlugins.Windows
{
    /// <summary>
    /// Sizes a window with <see cref="Window.SizeToContent"/> once more after it is shown.
    /// </summary>
    internal static class FitToContent
    {
        /// <summary>
        /// The first size is measured before the window has its frame. In Revit (another DPI, a non-resizable frame) that
        /// frame comes out of another width than WPF expected, and the content is cut off at the right and bottom edges.
        /// Switching <see cref="Window.SizeToContent"/> off and on measures the window again with the real frame.
        /// </summary>
        public static void Apply(Window window) => window.ContentRendered += (s, e) =>
        {
            var mode = window.SizeToContent;
            window.SizeToContent = SizeToContent.Manual;
            window.SizeToContent = mode;
        };
    }
}
