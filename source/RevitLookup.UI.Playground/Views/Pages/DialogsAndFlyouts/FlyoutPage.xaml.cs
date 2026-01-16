using RevitLookup.UI.Playground.ViewModels.Pages.DialogsAndFlyouts;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.DialogsAndFlyouts;

public sealed partial class FlyoutPage : INavigableView<FlyoutViewModel>
{
    public FlyoutViewModel ViewModel { get; }

    public FlyoutPage(FlyoutViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}