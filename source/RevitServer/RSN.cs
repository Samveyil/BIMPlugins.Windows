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
        public static List<string> GetFilePaths()
        {
            var vm = CreateViewModel(() => new MultiFilesViewModel());
            if (vm == null) return null;

            var window = new MultiFilesWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePaths;
        }
        public static List<string> GetFilePaths(string version)
        {
            var vm = CreateViewModel(() => new MultiFilesViewModel(version));
            if (vm == null) return null;

            var window = new MultiFilesWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePaths;
        }

        public static string GetFilePath(string version)
        {
            var vm = CreateViewModel(() => new OneFileViewModel(version));
            if (vm == null) return null;

            var window = new OneFileWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePath;
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
