using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class ToggleSwitchPage : INavigableView<ToggleSwitchViewModel>
{
    public ToggleSwitchViewModel ViewModel { get; }

    public ToggleSwitchPage(ToggleSwitchViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}