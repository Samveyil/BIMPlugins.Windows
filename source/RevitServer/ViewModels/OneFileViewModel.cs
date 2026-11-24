using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    public partial class OneFileViewModel(string revitVersion) : RSNViewModel(revitVersion)
    {
        [ObservableProperty] private string _filePath;

        [RelayCommand]
        private void TreeItemClicked(FileItem fileItem) => FilePath = fileItem.FilePath;

        [RelayCommand]
        private void Run()
        {
            IsCancelled = string.IsNullOrEmpty(FilePath);

            RaiseCloseRequest();
        }
    }
}