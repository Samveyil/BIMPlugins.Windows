using BIMPlugins.Windows.Mvvm;
using System.Windows.Input;

namespace BIMPlugins.Windows.RevitServer.Classes
{
    internal class FileItem : ObservableBase
    {
        private bool _isSelected;
        private ICommand _copyPathCommand;

        public FileItem(string name)
        {
            Name = name;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public string Name { get; }
        public FolderItem Parent { get; set; }
        public string FilePath { get; set; }
        public string Version { get; set; }

        // Created on first use: a tree has thousands of items, and few of them are ever copied.
        public ICommand CopyPathCommand => _copyPathCommand ??= new RelayCommand(() => ClipboardText.Set($"Revit {Version} | {FilePath}"));
    }
}
