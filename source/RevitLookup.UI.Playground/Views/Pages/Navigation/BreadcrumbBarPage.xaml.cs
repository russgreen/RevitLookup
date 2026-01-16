using RevitLookup.UI.Playground.ViewModels.Pages.Navigation;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Navigation;

public sealed partial class BreadcrumbBarPage : INavigableView<BreadcrumbBarViewModel>
{
    public BreadcrumbBarViewModel ViewModel { get; }

    public BreadcrumbBarPage(BreadcrumbBarViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}