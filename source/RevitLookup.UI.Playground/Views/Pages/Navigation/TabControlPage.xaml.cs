using RevitLookup.UI.Playground.ViewModels.Pages.Navigation;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Navigation;

public sealed partial class TabControlPage : INavigableView<TabControlViewModel>
{
    public TabControlViewModel ViewModel { get; }

    public TabControlPage(TabControlViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}