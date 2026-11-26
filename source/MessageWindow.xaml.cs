using BIMPlugins.Windows.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BIMPlugins.Windows
{
    /// <summary>The window of <see cref="MessageDialog"/>.</summary>
    internal partial class MessageWindow : Window
    {
        public MessageWindow(MessageViewModel viewModel)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);
            FitToContent.Apply(this);

            DataContext = viewModel;
            viewModel.CloseRequest += (s, e) => Close();

            // The focus starts on the default button once the window is shown and active: on Loaded the window still takes
            // the focus back when it is activated. Enter presses the focused button, so the focus must be on the default one.
            ContentRendered += (s, e) => FocusDefaultButton(viewModel);
        }

        private void FocusDefaultButton(MessageViewModel viewModel)
        {
            var item = viewModel.Buttons.First(b => b.IsDefault);
            if (ButtonsPanel.ItemContainerGenerator.ContainerFromItem(item) is DependencyObject container
                && FindButton(container) is Button button)
                button.Focus();
        }

        private static Button FindButton(DependencyObject parent)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Button button) return button;
                if (FindButton(child) is Button found) return found;
            }

            return null;
        }
    }
}
