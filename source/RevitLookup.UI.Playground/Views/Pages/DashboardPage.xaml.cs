using RevitLookup.UI.Playground.ViewModels.Pages;

namespace RevitLookup.UI.Playground.Views.Pages;

public sealed partial class DashboardPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}