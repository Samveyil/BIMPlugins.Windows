using System.Collections;

namespace BIMPlugins.Windows
{
    /// <summary>
    /// A list of lines in the BIMPlugins look, above Revit, as <see cref="MessageDialog"/>.
    /// The button or Ctrl+C copies every line; Enter and Esc close it.
    /// </summary>
    /// <example>
    /// <code>
    /// ReportDialog.Show(closedModels, "Closing models");
    /// </code>
    /// </example>
    public static class ReportDialog
    {
        /// <summary>Shows the lines and waits until the window is closed. Each item is shown as its text.</summary>
        /// <param name="items">The lines; null shows an empty list. A line wider than the window wraps.</param>
        /// <param name="title">The window's title; "Report" when empty.</param>
        public static void Show(IEnumerable items, string title = null) => new ReportWindow(items, title).ShowDialog();
    }
}
