using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer.Classes
{
    public partial class FileItem(string name) : ObservableObject
    {
        [ObservableProperty] private bool _isSelected = false;

        public string Name { get; } = name;
        public FolderItem Parent { get; set; }
        public string FilePath { get; set; }
        public string Version { get; set; }

        [RelayCommand]
        private void CopyPath()
        {
            // Another program may hold the clipboard open for a moment, and SetText then throws instead of waiting.
            for (var attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    Clipboard.SetText($"Revit {Version} | {FilePath}");
                    return;
                }
                catch (ExternalException)
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}