using RevitLookup.UI.Playground.ViewModels.Pages.DialogsAndFlyouts;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.DialogsAndFlyouts;

public sealed partial class SnackbarPage : INavigableView<SnackbarViewModel>
{
    public SnackbarViewModel ViewModel { get; }

    public SnackbarPage(SnackbarViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}