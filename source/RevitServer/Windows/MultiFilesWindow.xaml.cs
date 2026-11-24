using BIMPlugins.Windows.RevitServer.ViewModels;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer.Windows
{
    /// <summary>
    /// Логика взаимодействия для MultiFilesWindow.xaml
    /// </summary>
    public partial class MultiFilesWindow : Window
    {
        public MultiFilesWindow(MultiFilesViewModel viewModel)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);
            DataContext = viewModel;
        }
    }
}
