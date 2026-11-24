using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace BIMPlugins.Windows.RevitServer.Classes
{
    internal partial class FolderItem(string name) : FileItem(name)
    {
        [ObservableProperty] private bool _isExpanded = false;

        public ICollectionView Items { get; set; }
        public bool IsServer { get; set; } = false;
    }
}