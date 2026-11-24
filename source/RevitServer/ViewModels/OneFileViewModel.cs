using BIMPlugins.Windows.Mvvm;
using BIMPlugins.Windows.RevitServer.Classes;
using System.Windows.Input;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    internal class OneFileViewModel(string revitVersion) : RSNViewModel(revitVersion)
    {
        private FileItem _selectedFile;
        private ICommand _treeItemClickedCommand;
        private ICommand _runCommand;

        public FileItem SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        // A folder is clicked too; only a file is a choice.
        public ICommand TreeItemClickedCommand => _treeItemClickedCommand ??=
            new RelayCommand<FileItem>(fileItem => SelectedFile = fileItem is FolderItem ? null : fileItem);

        public ICommand RunCommand => _runCommand ??= new RelayCommand(Run);

        private void Run()
        {
            IsCancelled = string.IsNullOrEmpty(SelectedFile?.FilePath);

            RaiseCloseRequest();
        }
    }
}
