using BIMPlugins.Windows.Properties;
using BIMPlugins.Windows.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Xml;
using System.Xml.Linq;

namespace BIMPlugins.Windows.RevitServer.ViewModels
{
    public partial class RSNViewModel : ObservableObject
    {
        [ObservableProperty] private string _filter;
        [ObservableProperty] private ICollectionView _serverItems;

        private readonly string _xmlFilePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\BIMPlugins\Структура RevitServer.xml";

        public bool IsCancelled { get; set; } = true;

        private Regex _filterRegex;

        partial void OnFilterChanged(string value)
        {
            _filterRegex = CreateFilterRegex(value);

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

            if (MatchesFilter(fileItem.Name))
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
                if (MatchesFilter(child.Name))
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

        public RSNViewModel()
        {
            var xRoot = LoadStructure().Element("RevitServer")
                ?? throw new InvalidDataException(string.Format(Resources.StructureFileInvalid, _xmlFilePath));

            var revits = new List<FolderItem>();
            foreach (var xRSN in xRoot.Elements())
            {
                var revitItem = new FolderItem(xRSN.Name.LocalName) { IsServer = true};

                revitItem.Items = CreateView(GetServers(xRSN, revitItem));
                revits.Add(revitItem);
            }

            ServerItems = CreateView(revits);
        }
        public RSNViewModel(string revitVersion)
        {
            var xDoc = LoadStructure();
            var xRSN = xDoc.Descendants($"RSN{revitVersion}").FirstOrDefault()
                ?? throw new InvalidDataException(string.Format(Resources.StructureVersionNotFound, revitVersion, _xmlFilePath));

            ServerItems = CreateView(GetServers(xRSN, null));
        }

        private List<FolderItem> GetServers(XElement xRSN, FolderItem parent)
        {
            var servers = new List<FolderItem>();
            foreach (var xServer in xRSN.Elements())
            {
                var serverItem = new FolderItem(xServer.Attribute("name").Value) { IsServer = true, Parent = parent };
                GetData(serverItem, xServer);

                servers.Add(serverItem);
            }

            return servers;
        }

        private ICollectionView CreateView<T>(List<T> items) where T : FileItem
        {
            var view = CollectionViewSource.GetDefaultView(items);
            view.Filter = UniversalFilter;
            return view;
        }

        // Every problem with the structure file comes out as InvalidDataException with a message for the user.
        private XDocument LoadStructure()
        {
            if (!File.Exists(_xmlFilePath))
                throw new InvalidDataException(string.Format(Resources.StructureFileNotFound, _xmlFilePath));

            try
            {
                return XDocument.Load(_xmlFilePath);
            }
            catch (Exception e) when (e is XmlException || e is IOException || e is UnauthorizedAccessException)
            {
                throw new InvalidDataException(string.Format(Resources.StructureFileInvalid, _xmlFilePath), e);
            }
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
                        FilePath = GetRSNPath(xChildElement),
                        Version = xChildElement.Ancestors("server").First().Parent.Name.LocalName.Replace("RSN", "")
                    };
                    items.Add(fileItem);
                }
            }

            if (item is FolderItem folderItem)
            {
                folderItem.Items = CreateView(items);
            }
        }

        private bool UniversalFilter(object item)
        {
            if (string.IsNullOrEmpty(Filter))
                return true;

            var fileItem = (FileItem)item;

            if (MatchesFilter(fileItem.Name))
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
                if (MatchesFilter(parent.Name))
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
                if (MatchesFilter(child.Name))
                    return true;

                if (AnyParentMatchesFilter(child))
                    return true;

                if (child is FolderItem childFolder && HasMatchingChildren(childFolder))
                    return true;
            }

            return false;
        }

        // A filter that is not a valid regular expression, such as "(" or "[", is searched for as plain text.
        private static Regex CreateFilterRegex(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return null;

            try
            {
                return new Regex(filter, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
            catch (ArgumentException)
            {
                return new Regex(Regex.Escape(filter), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
        }

        private bool MatchesFilter(string name) => _filterRegex != null && name != null && _filterRegex.IsMatch(name);

        private string GetRSNPath(XElement fileElement)
        {
            var pathParts = new List<string>();

            var fileName = fileElement.Attribute("name")?.Value;
            if (!string.IsNullOrEmpty(fileName))
                pathParts.Add(fileName);

            XElement current = fileElement.Parent;
            string serverName = null;

            while (current != null)
            {
                var elementName = current.Name.LocalName;
                var nameAttr = current.Attribute("name")?.Value;

                switch (elementName)
                {
                    case "folder":
                        if (!string.IsNullOrEmpty(nameAttr))
                            pathParts.Insert(0, nameAttr);
                        break;

                    case "project":
                        if (!string.IsNullOrEmpty(nameAttr))
                            pathParts.Insert(0, nameAttr);
                        break;

                    case "server":
                        serverName = nameAttr;
                        break;
                }

                current = current.Parent;
            }

            if (!string.IsNullOrEmpty(serverName) && pathParts.Count > 0)
            {
                return $"RSN://{serverName}/{string.Join("/", pathParts)}";
            }

            return string.Empty;
        }


        public event EventHandler CloseRequest;
        public void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}