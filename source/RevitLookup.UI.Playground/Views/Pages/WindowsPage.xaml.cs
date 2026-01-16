using RevitLookup.UI.Playground.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages;

public sealed partial class WindowsPage : INavigableView<WindowsViewModel>
{
    public WindowsPage(WindowsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public WindowsViewModel ViewModel { get; }
}