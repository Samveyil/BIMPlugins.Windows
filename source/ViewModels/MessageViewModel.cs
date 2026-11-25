using BIMPlugins.Windows.Mvvm;
using BIMPlugins.Windows.Properties;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BIMPlugins.Windows.ViewModels
{
    /// <summary>A button of <see cref="MessageWindow"/>: Enter presses the default one, Esc the cancelling one.</summary>
    internal sealed class MessageButton(string text, MessageResult result)
    {
        public string Text { get; } = text;
        public MessageResult Result { get; } = result;
        public bool IsDefault { get; set; }
        public bool IsCancel { get; set; }
    }

    internal sealed class MessageViewModel
    {
        public MessageViewModel(string message, string title, MessageButtons buttons, MessageIcon icon, MessageResult? defaultResult)
        {
            Message = message;
            Title = string.IsNullOrEmpty(title) ? "BIMPlugins" : title;
            HasIcon = icon != MessageIcon.None;
            Icon = icon switch
            {
                MessageIcon.Question => PackIconKind.HelpCircleOutline,
                MessageIcon.Warning => PackIconKind.AlertOutline,
                MessageIcon.Error => PackIconKind.AlertCircleOutline,
                _ => PackIconKind.InformationOutline
            };

            Buttons = (buttons switch
            {
                MessageButtons.Ok => new[] { MessageResult.Ok },
                MessageButtons.OkCancel => new[] { MessageResult.Ok, MessageResult.Cancel },
                MessageButtons.YesNo => new[] { MessageResult.Yes, MessageResult.No },
                MessageButtons.YesNoCancel => new[] { MessageResult.Yes, MessageResult.No, MessageResult.Cancel },
                MessageButtons.RetryCancel => new[] { MessageResult.Retry, MessageResult.Cancel },
                _ => new[] { MessageResult.Close }
            }).Select(result => new MessageButton(Text(result), result)).ToList();

            var defaultButton = Buttons.FirstOrDefault(b => b.Result == defaultResult) ?? Buttons[0];
            defaultButton.IsDefault = true;

            // Closing the window answers as the cancelling button would, so that it never returns a button that is not there.
            var cancelButton = Buttons.FirstOrDefault(b => b.Result == MessageResult.Cancel)
                ?? Buttons.FirstOrDefault(b => b.Result == MessageResult.No)
                ?? Buttons[Buttons.Count - 1];
            cancelButton.IsCancel = true;
            Result = cancelButton.Result;
        }

        public string Message { get; }
        public string Title { get; }
        public PackIconKind Icon { get; }
        public bool HasIcon { get; }
        public IReadOnlyList<MessageButton> Buttons { get; }
        public MessageResult Result { get; private set; }

        public event EventHandler CloseRequest;

        public ICommand CloseCommand => _closeCommand ??= new RelayCommand<MessageResult>(Close);
        public ICommand CopyCommand => _copyCommand ??= new RelayCommand(() => ClipboardText.Set(Message));

        private ICommand _closeCommand;
        private ICommand _copyCommand;

        private static string Text(MessageResult result) => result switch
        {
            MessageResult.Ok => Resources.OK,
            MessageResult.Cancel => Resources.Cancel,
            MessageResult.Yes => Resources.Yes,
            MessageResult.No => Resources.No,
            MessageResult.Retry => Resources.Retry,
            _ => Resources.Close
        };

        private void Close(MessageResult result)
        {
            Result = result;
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
