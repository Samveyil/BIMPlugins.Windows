using CommunityToolkit.Mvvm.ComponentModel;

namespace BIMPlugins.Windows.RevitServer.Classes
{
    public partial class FileItem(string name) : ObservableObject
    {
        [ObservableProperty] private bool _isSelected = false;

        public string Name { get; } = name;
        public FolderItem Parent { get; set; }
    }
}