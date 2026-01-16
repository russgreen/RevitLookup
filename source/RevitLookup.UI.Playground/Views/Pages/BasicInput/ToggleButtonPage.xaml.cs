using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class ToggleButtonPage : INavigableView<ToggleButtonViewModel>
{
    public ToggleButtonViewModel ViewModel { get; }

    public ToggleButtonPage(ToggleButtonViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}