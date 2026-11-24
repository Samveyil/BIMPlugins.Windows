using System.Collections;
using System.Windows;

namespace BIMPlugins.Windows.Behaviors
{
    /// <summary>
    /// Binds a DataGrid's selected rows to a list of the view model, both ways, which DataGrid.SelectedItems cannot do:
    /// <c>behaviors:DataGridBehavior.SelectedItems="{Binding SelectedRows}"</c>.
    /// </summary>
    /// <remarks>
    /// The list must be an <see cref="IList"/> that can be changed, such as an ObservableCollection: the user's selection
    /// is written into it. A new list given to the property selects its items in the grid. In a grid with single
    /// selection only the first item is selected.
    /// </remarks>
    public static class DataGridBehavior
    {
        /// <summary>Identifies the <c>SelectedItems</c> attached property.</summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),
                typeof(DataGridBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedItemsSync.OnSelectedItemsChanged));

        /// <summary>Gets the list that holds the grid's selected rows.</summary>
        public static IList GetSelectedItems(DependencyObject obj) => (IList)obj.GetValue(SelectedItemsProperty);

        /// <summary>Sets the list that holds the grid's selected rows, and selects its items.</summary>
        public static void SetSelectedItems(DependencyObject obj, IList value) => obj.SetValue(SelectedItemsProperty, value);
    }
}
