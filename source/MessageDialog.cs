using BIMPlugins.Windows.ViewModels;

namespace BIMPlugins.Windows
{
    /// <summary>
    /// A message box in the BIMPlugins look, above Revit, as <c>TaskDialog.Show</c> of the Revit API.
    /// Enter presses the default button, Esc and the window's close button the cancelling one, Ctrl+C copies the message.
    /// </summary>
    /// <example>
    /// <code>
    /// MessageDialog.Show("Sheets copied.");
    ///
    /// if (MessageDialog.Show("Detach the model?", "Detach", MessageButtons.YesNo, MessageIcon.Question) == MessageResult.Yes)
    /// {
    /// }
    /// </code>
    /// </example>
    public static class MessageDialog
    {
        /// <summary>Shows the message and waits for an answer.</summary>
        /// <param name="message">The text; a long one scrolls.</param>
        /// <param name="title">The window's title; BIMPlugins when empty.</param>
        /// <param name="buttons">The buttons; Close alone unless set.</param>
        /// <param name="icon">The icon left of the text; none unless set.</param>
        /// <param name="defaultResult">
        /// The button Enter presses and the focus starts on; the first one of <paramref name="buttons"/> when null or not among them.
        /// </param>
        /// <returns>
        /// The button pressed. Esc and the window's close button answer as Cancel, No or Close when the dialog has one,
        /// otherwise as its only button.
        /// </returns>
        public static MessageResult Show(string message, string title = null, MessageButtons buttons = MessageButtons.Close,
            MessageIcon icon = MessageIcon.None, MessageResult? defaultResult = null)
        {
            var viewModel = new MessageViewModel(message, title, buttons, icon, defaultResult);
            new MessageWindow(viewModel).ShowDialog();

            return viewModel.Result;
        }
    }

    /// <summary>The buttons of <see cref="MessageDialog"/>, left to right.</summary>
    public enum MessageButtons
    {
        /// <summary>Close.</summary>
        Close,

        /// <summary>OK.</summary>
        Ok,

        /// <summary>OK and Cancel.</summary>
        OkCancel,

        /// <summary>Yes and No.</summary>
        YesNo,

        /// <summary>Yes, No and Cancel.</summary>
        YesNoCancel,

        /// <summary>Retry and Cancel.</summary>
        RetryCancel
    }

    /// <summary>The button pressed in <see cref="MessageDialog"/>.</summary>
    public enum MessageResult
    {
        /// <summary>Close, or the window closed in a dialog with Close.</summary>
        Close,

        /// <summary>OK.</summary>
        Ok,

        /// <summary>Cancel, or the window closed in a dialog with Cancel.</summary>
        Cancel,

        /// <summary>Yes.</summary>
        Yes,

        /// <summary>No, or the window closed in a Yes/No dialog.</summary>
        No,

        /// <summary>Retry.</summary>
        Retry
    }

    /// <summary>The icon of <see cref="MessageDialog"/>.</summary>
    public enum MessageIcon
    {
        /// <summary>No icon.</summary>
        None,

        /// <summary>An "i" in a circle.</summary>
        Information,

        /// <summary>A question mark in a circle.</summary>
        Question,

        /// <summary>An exclamation mark in a triangle.</summary>
        Warning,

        /// <summary>An exclamation mark in a circle.</summary>
        Error
    }
}
