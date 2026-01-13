using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Xml.Linq;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    public partial class RSNViewModel : ObservableObject
    {
        [ObservableProperty] private string _filter;
        [ObservableProperty] private ICollectionView _serverItems;

        private readonly string _xmlFilePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\BIMPlugins\Структура RevitServer.xml";

        public bool IsCancelled { get; private set; } = true;

        partial void OnFilterChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ResetAllExpanded(ServerItems);
                
                RefreshAllFilters(ServerItems);
            }
        }

        [RelayCommand]
        private void FilterKeyDown()
        {
            ResetAllExpanded(ServerItems);

            RefreshAllFilters(ServerItems);

            if (!string.IsNullOrEmpty(Filter))
                SetExpandedForFilteredItems(ServerItems);
        }

        [RelayCommand]
        private void ResetAllExpanded(ICollectionView view)
        {
            if (view == null) return;

            foreach (var item in view.OfType<FolderItem>())
            {
                item.IsExpanded = false;
                if (item.Items is ICollectionView childView)
                {
                    ResetAllExpanded(childView);
                }
            }
        }
        
        private void SetExpandedForFilteredItems(ICollectionView view)
        {
            if (view == null) return;

            foreach (var item in view.OfType<FileItem>())
            {
                if (IsDirectMatch(item))
                    SetExpandedForParents(item);

                if (item is FolderItem folder && folder.Items is ICollectionView childView)
                {
                    SetExpandedForFilteredItems(childView);
                }
            }
        }
        private bool IsDirectMatch(FileItem fileItem)
        {
            if (string.IsNullOrEmpty(Filter))
                return false;

            if (Regex.IsMatch(fileItem.Name, Filter))
                return true;

            if (fileItem is FolderItem folderItem)
                return HasDirectMatchingChildren(folderItem);

            return false;
        }
        private bool HasDirectMatchingChildren(FolderItem folderItem)
        {
            if (folderItem.Items?.SourceCollection == null)
                return false;

            foreach (FileItem child in folderItem.Items.SourceCollection)
            {
                if (Regex.IsMatch(child.Name, Filter))
                    return true;

                if (child is FolderItem childFolder && HasDirectMatchingChildren(childFolder))
                    return true;
            }

            return false;
        }
        private void SetExpandedForParents(FileItem fileItem)
        {
            var parent = fileItem.Parent;
            while (parent != null && parent is FolderItem folderParent)
            {
                folderParent.IsExpanded = true;
                parent = folderParent.Parent;
            }
        }
        private void RefreshAllFilters(ICollectionView view)
        {
            if (view == null) return;

            view.Refresh();

            foreach (var item in view.OfType<FolderItem>())
            {
                if (item.Items is ICollectionView childView)
                {
                    RefreshAllFilters(childView);
                }
            }
        }

        public RSNViewModel(string revitVersion)
        {
            var xDoc = XDocument.Load(_xmlFilePath);
            var xRSN = xDoc.Descendants($"RSN{revitVersion}").FirstOrDefault();

            var servers = new List<FolderItem>();
            foreach (var xServer in xRSN.Elements())
            {
                var serverItem = new FolderItem(xServer.Attribute("name").Value) { IsServer = true };
                GetData(serverItem, xServer);

                servers.Add(serverItem);
            }

            ServerItems = CollectionViewSource.GetDefaultView(servers);
            ServerItems.Filter = UniversalFilter;
        }

        [RelayCommand]
        private void Run()
        {
            RaiseCloseRequest();
            IsCancelled = false;
        }

        private void GetData(FileItem item, XElement xElement)
        {
            var items = new List<FileItem>();
            foreach (var xChildElement in xElement.Elements())
            {
                if (xChildElement.Name == "project" || xChildElement.Name == "folder")
                {
                    if (xChildElement.HasElements)
                    {
                        var childItem = new FolderItem(xChildElement.Attribute("name").Value)
                        {
                            Parent = item as FolderItem,
                        };
                        items.Add(childItem);
                        GetData(childItem, xChildElement);
                    }
                }
                else if (xChildElement.Name == "file")
                {
                    var fileItem = new FileItem(xChildElement.Attribute("name").Value)
                    {
                        Parent = item as FolderItem,
                    };
                    items.Add(fileItem);
                }
            }

            if (item is FolderItem folderItem)
            {
                folderItem.Items = CollectionViewSource.GetDefaultView(items);
                folderItem.Items.Filter = UniversalFilter;
            }
        }

        private bool UniversalFilter(object item)
        {
            if (string.IsNullOrEmpty(Filter))
                return true;

            var fileItem = (FileItem)item;

            if (Regex.IsMatch(fileItem.Name, Filter))
                return true;

            if (AnyParentMatchesFilter(fileItem))
                return true;

            if (fileItem is FolderItem folderItem)
                return HasMatchingChildren(folderItem);

            return false;
        }
        private bool AnyParentMatchesFilter(FileItem fileItem)
        {
            var parent = fileItem.Parent;
            while (parent != null)
            {
                if (Regex.IsMatch(parent.Name, Filter))
                    return true;
                
                parent = parent.Parent;
            }
            return false;
        }
        private bool HasMatchingChildren(FolderItem folderItem)
        {
            if (folderItem.Items?.SourceCollection == null)
                return false;

            foreach (FileItem child in folderItem.Items.SourceCollection)
            {
                if (Regex.IsMatch(child.Name, Filter))
                    return true;

                if (AnyParentMatchesFilter(child))
                    return true;

                if (child is FolderItem childFolder && HasMatchingChildren(childFolder))
                    return true;
            }

            return false;
        }


        public event EventHandler CloseRequest;
        public void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}