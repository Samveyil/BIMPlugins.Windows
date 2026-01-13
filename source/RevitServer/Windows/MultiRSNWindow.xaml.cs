using BIMPlugins.Windows.RevitServer.ViewModels;
using System.Windows;

namespace BIMPlugins.Windows.RevitServer.Windows
{
    /// <summary>
    /// Логика взаимодействия для MultiRSNWindow.xaml
    /// </summary>
    public partial class MultiRSNWindow : Window
    {
        public MultiRSNWindow(MultiRSNViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
