using BIMPlugins.Windows.RevitServer.ViewModels;
using BIMPlugins.Windows.RevitServer.Windows;
using System.Collections.Generic;

namespace BIMPlugins.Windows.RevitServer
{
    public static class RSN
    {
        public static List<string> GetFilePaths()
        {
            var vm = new MultiFilesViewModel();
            var window = new MultiFilesWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePaths;
        }
        public static List<string> GetFilePaths(string version)
        {
            var vm = new MultiFilesViewModel(version);
            var window = new MultiFilesWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePaths;
        }

        public static string GetFilePath(string version)
        {
            var vm = new OneFileViewModel(version);
            var window = new OneFileWindow(vm);

            vm.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return vm.IsCancelled ? null : vm.FilePath;
        }
    }
}
