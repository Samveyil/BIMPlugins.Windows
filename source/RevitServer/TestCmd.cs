using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMPlugins.Windows.RevitServer.ViewModels;
using BIMPlugins.Windows.RevitServer.Windows;

namespace BIMPlugins.Windows.RevitServer
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var viewModel = new MultiRSNViewModel("2019");
            var window = new MultiRSNWindow(viewModel);
            viewModel.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}
