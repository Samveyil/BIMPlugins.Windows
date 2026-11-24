using BIMPlugins.Windows.Properties;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BIMPlugins.Windows.ViewModels
{
    /// <summary>A button of <see cref="MessageWindow"/>: Enter presses the default one, Esc the cancel one.</summary>
    public sealed class MessageButton(string text, MessageBoxResult result, bool isDefault, bool isCancel)
    {
        public string Text { get; } = text;
        public MessageBoxResult Result { get; } = result;
        public bool IsDefault { get; } = isDefault;
        public bool IsCancel { get; } = isCancel;
    }

    public sealed partial class MessageViewModel
    {
        public MessageViewModel(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            Message = message;
            Title = string.IsNullOrEmpty(title) ? "BIMPlugins" : title;
            Icon = icon switch
            {
                MessageBoxImage.Information => PackIconKind.InformationOutline,
                MessageBoxImage.Question => PackIconKind.HelpCircleOutline,
                MessageBoxImage.Warning => PackIconKind.AlertOutline,
                MessageBoxImage.Error => PackIconKind.AlertCircleOutline,
                _ => null
            };

            MessageButton ok = new(Resources.OK, MessageBoxResult.OK, true, false);
            MessageButton yes = new(Resources.Yes, MessageBoxResult.Yes, true, false);
            MessageButton cancel = new(Resources.Cancel, MessageBoxResult.Cancel, false, true);

            // Closing the window answers as the cancel button would, so that it never returns a button that is not there.
            (Buttons, Result) = buttons switch
            {
                MessageBoxButton.OKCancel => (new[] { ok, cancel }, MessageBoxResult.Cancel),
                MessageBoxButton.YesNo => (new[] { yes, new(Resources.No, MessageBoxResult.No, false, true) }, MessageBoxResult.No),
                MessageBoxButton.YesNoCancel => (new[] { yes, new(Resources.No, MessageBoxResult.No, false, false), cancel }, MessageBoxResult.Cancel),
                _ => (new[] { new MessageButton(Resources.OK, MessageBoxResult.OK, true, true) }, MessageBoxResult.OK)
            };
        }

        public string Message { get; }
        public string Title { get; }
        public PackIconKind? Icon { get; }
        public IReadOnlyList<MessageButton> Buttons { get; }
        public MessageBoxResult Result { get; private set; }

        public event EventHandler CloseRequest;

        [RelayCommand]
        private void Close(MessageBoxResult result)
        {
            Result = result;
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void Copy() => ClipboardText.Set(Message);
    }
}
