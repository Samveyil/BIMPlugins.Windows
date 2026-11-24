using BIMPlugins.Windows.RevitServer.ViewModels;
using BIMPlugins.Windows.RevitServer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer
{
    public static class RSN
    {
        /// <summary>Projects of every Revit version; null when cancelled.</summary>
        public static List<string> GetFilePaths() =>
            Show(() => new MultiFilesViewModel(), vm => new MultiFilesWindow(vm), vm => vm.FilePaths);

        /// <summary>Projects of one Revit version, such as "2023"; null when cancelled.</summary>
        public static List<string> GetFilePaths(string version) =>
            Show(() => new MultiFilesViewModel(version), vm => new MultiFilesWindow(vm), vm => vm.FilePaths);

        /// <summary>One project of one Revit version, such as "2023"; null when cancelled.</summary>
        public static string GetFilePath(string version) =>
            Show(() => new OneFileViewModel(version), vm => new OneFileWindow(vm), vm => vm.FilePath);

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

        // A missing or broken structure file is told to the user instead of failing the add-in; the caller gets null, as on cancel.
        private static T CreateViewModel<T>(Func<T> create) where T : RSNViewModel
        {
            try
            {
                return create();
            }
            catch (InvalidDataException e)
            {
                MessageWindow.ShowMessage(e.Message, MessageBoxImage.Warning);
                return null;
            }
        }
    }
}
