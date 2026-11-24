using System.ComponentModel;

namespace BIMPlugins.Windows.RevitServer.Classes
{
    internal class FolderItem(string name) : FileItem(name)
    {
        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public ICollectionView Items { get; set; }
        public bool IsServer { get; set; } = false;
    }
}
