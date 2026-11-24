using BIMPlugins.Windows.Mvvm;
using BIMPlugins.Windows.RevitServer.Classes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    internal class MultiFilesViewModel : RSNViewModel
    {
        private ICommand _folderCheckedCommand;
        private ICommand _projectCheckedCommand;
        private ICommand _removeProjectCommand;
        private ICommand _runCommand;

        public MultiFilesViewModel(string revitVersion) : base(revitVersion)
        {
        }

        public MultiFilesViewModel() : base()
        {
        }

        public ObservableCollection<FileItem> Projects { get; } = [];

        public ICommand FolderCheckedCommand => _folderCheckedCommand ??= new RelayCommand<FolderItem>(FolderChecked);
        public ICommand ProjectCheckedCommand => _projectCheckedCommand ??= new RelayCommand<FileItem>(ProjectChecked);
        public ICommand RemoveProjectCommand => _removeProjectCommand ??= new RelayCommand<FileItem>(RemoveProject);
        public ICommand RunCommand => _runCommand ??= new RelayCommand(Run);

        private void FolderChecked(FolderItem folderItem) => CheckItemsRecursively(folderItem, folderItem.IsSelected);

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

        private void RemoveProject(FileItem fileItem)
        {
            Projects.Remove(fileItem);

            fileItem.IsSelected = false;

            var items = fileItem.Parent.Items.Cast<FileItem>();
            if (items.All(f => !f.IsSelected))
                fileItem.Parent.IsSelected = false;
        }

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
