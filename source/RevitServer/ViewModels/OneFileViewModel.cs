using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    internal partial class OneFileViewModel(string revitVersion) : RSNViewModel(revitVersion)
    {
        [ObservableProperty] private FileItem _selectedFile;

        // A folder is clicked too; only a file is a choice.
        [RelayCommand]
        private void TreeItemClicked(FileItem fileItem) => SelectedFile = fileItem is FolderItem ? null : fileItem;

        [RelayCommand]
        private void Run()
        {
            IsCancelled = string.IsNullOrEmpty(SelectedFile?.FilePath);

            RaiseCloseRequest();
        }
    }
}
