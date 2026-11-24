using System.Collections;
using System.Windows;

namespace BIMPlugins.Windows.Behaviors
{
    /// <summary>
    /// Binds a ListBox's selected items to a list of the view model, both ways, which ListBox.SelectedItems cannot do:
    /// <c>behaviors:ListBoxBehavior.SelectedItems="{Binding SelectedItems}"</c> on a ListBox with
    /// <c>SelectionMode="Extended"</c> or <c>"Multiple"</c>.
    /// </summary>
    /// <remarks>
    /// The list must be an <see cref="IList"/> that can be changed, such as an ObservableCollection: the user's selection
    /// is written into it. A new list given to the property selects its items in the ListBox. With
    /// <c>SelectionMode="Single"</c> only the first item is selected.
    /// </remarks>
    public static class ListBoxBehavior
    {
        /// <summary>Identifies the <c>SelectedItems</c> attached property.</summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),
                typeof(ListBoxBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedItemsSync.OnSelectedItemsChanged));

        /// <summary>Gets the list that holds the ListBox's selected items.</summary>
        public static IList GetSelectedItems(DependencyObject obj) => (IList)obj.GetValue(SelectedItemsProperty);

        /// <summary>Sets the list that holds the ListBox's selected items, and selects its items.</summary>
        public static void SetSelectedItems(DependencyObject obj, IList value) => obj.SetValue(SelectedItemsProperty, value);
    }
}
