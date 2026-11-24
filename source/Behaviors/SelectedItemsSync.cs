using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BIMPlugins.Windows.Behaviors
{
    /// <summary>
    /// Keeps a view model's list and a control's selected items the same, for <see cref="DataGridBehavior"/> and <see cref="ListBoxBehavior"/>.
    /// </summary>
    internal static class SelectedItemsSync
    {
        // Set on a control once it listens to SelectionChanged, so that a new bound list never adds a second handler.
        private static readonly DependencyProperty SyncedPropertyProperty =
            DependencyProperty.RegisterAttached("SyncedProperty", typeof(DependencyProperty), typeof(SelectedItemsSync));

        // Set while the control's selection is written from the bound list, so that SelectionChanged does not write it back.
        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(SelectedItemsSync));

        public static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Selector selector || GetSelection(selector) is not IList selection)
                return;

            if (selector.GetValue(SyncedPropertyProperty) == null)
            {
                selector.SetValue(SyncedPropertyProperty, e.Property);
                selector.SelectionChanged += OnSelectionChanged;
            }

            // A copy: the bound list may be changed while the selection is written.
            var items = (e.NewValue as IList)?.Cast<object>().ToList();

            selector.SetValue(IsUpdatingProperty, true);
            try
            {
                if (IsSingleSelection(selector))
                {
                    selector.SelectedItem = items?.FirstOrDefault();
                }
                else
                {
                    selection.Clear();
                    foreach (var item in items ?? Enumerable.Empty<object>())
                        selection.Add(item);
                }
            }
            finally
            {
                selector.SetValue(IsUpdatingProperty, false);
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (Selector)sender;

            // SelectionChanged bubbles up from combo boxes and lists inside the control's cells and items.
            if (e.OriginalSource != selector || (bool)selector.GetValue(IsUpdatingProperty))
                return;

            var property = (DependencyProperty)selector.GetValue(SyncedPropertyProperty);
            if (selector.GetValue(property) is not IList target || GetSelection(selector) is not IList selection)
                return;

            target.Clear();
            foreach (var item in selection)
                target.Add(item);
        }

        private static IList GetSelection(Selector selector) => selector switch
        {
            ListBox listBox => listBox.SelectedItems,
            MultiSelector multiSelector => multiSelector.SelectedItems,
            _ => null
        };

        // SelectedItems.Add throws in single selection mode, so the selection is written through SelectedItem there.
        private static bool IsSingleSelection(Selector selector) => selector switch
        {
            ListBox listBox => listBox.SelectionMode == SelectionMode.Single,
            DataGrid dataGrid => dataGrid.SelectionMode == DataGridSelectionMode.Single,
            _ => false
        };
    }
}
