using BIMPlugins.Windows.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BIMPlugins.Windows
{
    /// <summary>A message box in the BIMPlugins look, above Revit. Ctrl+C copies the message.</summary>
    public partial class MessageWindow : Window
    {
        private MessageWindow(MessageViewModel viewModel)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);

            DataContext = viewModel;
            viewModel.CloseRequest += (s, e) => Close();

            // Focus on the first button, the default one, once the window is shown and active: on Loaded the window
            // still takes the focus back when it is activated.
            ContentRendered += (s, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        /// <summary>
        /// Shows the message and waits for an answer, as <see cref="MessageBox.Show(string, string, MessageBoxButton, MessageBoxImage)"/>.
        /// Closing the window answers as the cancel button would: Cancel, No for Yes/No, OK for OK alone.
        /// </summary>
        /// <param name="title">The window's title; BIMPlugins when empty.</param>
        public static MessageBoxResult ShowMessage(string message, string title = null,
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
        {
            var viewModel = new MessageViewModel(message, title, buttons, icon);
            new MessageWindow(viewModel).ShowDialog();

            return viewModel.Result;
        }
    }
}
