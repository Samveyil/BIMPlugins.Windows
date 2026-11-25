using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BIMPlugins.Windows
{
    /// <summary>The window of <see cref="ReportDialog"/>.</summary>
    internal partial class ReportWindow : Window
    {
        private readonly IEnumerable _items;

        public ReportWindow(IEnumerable items, string title)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);

            _items = items ?? Array.Empty<object>();
            Box.ItemsSource = _items;
            Title = string.IsNullOrEmpty(title) ? Properties.Resources.ReportWindowTitle : title;
        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs e) =>
            ClipboardText.Set(string.Join(Environment.NewLine, _items.Cast<object>().Select(i => i?.ToString())));

        private void OnClose(object sender, RoutedEventArgs e) => Close();
    }
}
