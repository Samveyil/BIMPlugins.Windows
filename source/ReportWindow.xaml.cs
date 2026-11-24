using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BIMPlugins.Windows
{
    /// <summary>A list of lines in the BIMPlugins look, above Revit. Ctrl+C or the button copies every line.</summary>
    public partial class ReportWindow : Window
    {
        private readonly IEnumerable _items;

        private ReportWindow(IEnumerable items, string title)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);

            _items = items ?? Array.Empty<object>();
            Box.ItemsSource = _items;
            Title = string.IsNullOrEmpty(title) ? Properties.Resources.ReportWindowTitle : title;
        }

        /// <summary>Shows the lines and waits until the window is closed. Each item is shown as its text.</summary>
        /// <param name="title">The window's title; "Report" when empty.</param>
        public static void ShowReport(IEnumerable items, string title = null) => new ReportWindow(items, title).ShowDialog();

        private void OnCopy(object sender, ExecutedRoutedEventArgs e) =>
            ClipboardText.Set(string.Join(Environment.NewLine, _items.Cast<object>().Select(i => i?.ToString())));

        private void OnClose(object sender, RoutedEventArgs e) => Close();
    }
}
