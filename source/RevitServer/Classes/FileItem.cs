using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        private void CopyPath() => ClipboardText.Set($"Revit {Version} | {FilePath}");
    }
}