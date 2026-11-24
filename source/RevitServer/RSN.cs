using BIMPlugins.Windows.RevitServer.Classes;
using BIMPlugins.Windows.RevitServer.ViewModels;
using BIMPlugins.Windows.RevitServer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer
{
    /// <summary>
    /// Choosing projects on Revit Server, from the structure file at <see cref="StructureFilePath"/>.
    /// A missing or broken file is told to the user, and the result is then null, as on cancel.
    /// </summary>
    public static class RSN
    {
        /// <summary>
        /// The structure file of Revit Server that the windows read, and that the add-in which collects the structure
        /// writes; <c>%AppData%\BIMPlugins\RevitServer.xml</c> unless set.
        /// </summary>
        public static string StructureFilePath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BIMPlugins", "RevitServer.xml");

        /// <summary>Lets the user choose projects; null when cancelled.</summary>
        /// <param name="version">The Revit version, such as "2023", to show its servers alone; null for every version.</param>
        public static IReadOnlyList<RevitServerFile> SelectFiles(string version = null) =>
            Show(() => version == null ? new MultiFilesViewModel() : new MultiFilesViewModel(version),
                vm => new MultiFilesWindow(vm),
                vm => (IReadOnlyList<RevitServerFile>)vm.Projects.Select(ToResult).ToList());

        /// <summary>Lets the user choose one project of a Revit version, such as "2023"; null when cancelled.</summary>
        public static RevitServerFile SelectFile(string version) =>
            Show(() => new OneFileViewModel(version), vm => new OneFileWindow(vm), vm => ToResult(vm.SelectedFile));

        private static RevitServerFile ToResult(FileItem item) => new(item.Name, item.FilePath, item.Version);

        private static TResult Show<TViewModel, TResult>(Func<TViewModel> createViewModel, Func<TViewModel, Window> createWindow,
            Func<TViewModel, TResult> getResult) where TViewModel : RSNViewModel where TResult : class
        {
            var vm = CreateViewModel(createViewModel);
            if (vm == null) return null;

            var window = createWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : getResult(vm);
        }

        private static T CreateViewModel<T>(Func<T> create) where T : RSNViewModel
        {
            try
            {
                return create();
            }
            catch (InvalidDataException e)
            {
                MessageWindow.ShowMessage(e.Message, icon: MessageBoxImage.Warning);
                return null;
            }
        }
    }
}
