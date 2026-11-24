using BIMPlugins.Windows.RevitServer.ViewModels;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer.Windows
{
    /// <summary>
    /// Логика взаимодействия для OneFileWindow.xaml
    /// </summary>
    public partial class OneFileWindow : Window
    {
        public OneFileWindow(OneFileViewModel viewModel)
        {
            InitializeComponent();
            WindowOwner.SetToRevit(this);
            DataContext = viewModel;
        }
    }
}
