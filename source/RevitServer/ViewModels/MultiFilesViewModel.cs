using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    internal partial class MultiFilesViewModel : RSNViewModel
    {
        [ObservableProperty] private ObservableCollection<FileItem> _projects = [];

        public MultiFilesViewModel(string revitVersion) : base(revitVersion)
        {
        }

        public MultiFilesViewModel() : base()
        {
        }


        [RelayCommand]
        private void FolderChecked(FolderItem folderItem) => CheckItemsRecursively(folderItem, folderItem.IsSelected);

        [RelayCommand]
        private void ProjectChecked(FileItem fileItem)
        {
            if (fileItem.IsSelected && !Projects.Contains(fileItem))
                Projects.Add(fileItem);
            else if (!fileItem.IsSelected)
                Projects.Remove(fileItem);

            var items = fileItem.Parent.Items.Cast<FileItem>();
            if (items.All(f => f.IsSelected))
                fileItem.Parent.IsSelected = true;
            else if (items.All(f => !f.IsSelected))
                fileItem.Parent.IsSelected = false;
        }

        [RelayCommand]
        private void RemoveProject(FileItem fileItem)
        {
            Projects.Remove(fileItem);
            
            fileItem.IsSelected = false;

            var items = fileItem.Parent.Items.Cast<FileItem>();
            if (items.All(f => !f.IsSelected))
                fileItem.Parent.IsSelected = false;
        }

        [RelayCommand]
        private void Run()
        {
            IsCancelled = Projects.Count == 0;

            RaiseCloseRequest();
        }

        private void CheckItemsRecursively(FolderItem folder, bool isSelected)
        {
            foreach (var item in folder.Items.Cast<FileItem>())
            {
                item.IsSelected = isSelected;

                if (item is FolderItem subFolder)
                    CheckItemsRecursively(subFolder, isSelected);
                else
                    ProjectChecked(item);
            }
        }
    }
}