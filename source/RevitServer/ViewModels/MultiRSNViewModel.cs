using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    public partial class MultiRSNViewModel(string revitVersion) : RSNViewModel(revitVersion)
    {
        [ObservableProperty] private ObservableCollection<string> _projects = [];

        [RelayCommand]
        private void FolderChecked(FolderItem folderItem) => CheckItemsRecursively(folderItem, folderItem.IsSelected);

        [RelayCommand]
        private void ProjectChecked(FileItem fileItem)
        {
            if (fileItem.IsSelected && !Projects.Contains(fileItem.Name))
                Projects.Add(fileItem.Name);
            else if (!fileItem.IsSelected)
                Projects.Remove(fileItem.Name);

            var items = fileItem.Parent.Items.Cast<FileItem>();
            if (items.All(f => f.IsSelected))
                fileItem.Parent.IsSelected = true;
            else if (items.All(f => !f.IsSelected))
                fileItem.Parent.IsSelected = false;
        }

        [RelayCommand]
        private void RemoveProject(string project)
        {
            Projects.Remove(project);
            
            var fileItem = FindFileItemByName(project);
            fileItem.IsSelected = false;

            var items = fileItem.Parent.Items.Cast<FileItem>();
            if (items.All(f => !f.IsSelected))
                fileItem.Parent.IsSelected = false;
        }

        public FileItem FindFileItemByName(string fileName)
        {
            foreach (var server in ServerItems.Cast<FolderItem>())
            {
                var result = FindFileItemRecursively(server, fileName);
                if (result != null)
                    return result;
            }
            return null;
        }
        private FileItem FindFileItemRecursively(FolderItem folder, string fileName)
        {
            if (folder.Items == null) return null;

            foreach (var item in folder.Items.Cast<FileItem>())
            {
                if (item.Name == fileName)
                    return item;

                if (item is FolderItem subFolder)
                {
                    var result = FindFileItemRecursively(subFolder, fileName);
                    if (result != null)
                        return result;
                }
            }
            return null;
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