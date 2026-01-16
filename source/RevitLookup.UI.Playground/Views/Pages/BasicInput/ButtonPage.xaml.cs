using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class ButtonPage : INavigableView<ButtonViewModel>
{
    public ButtonViewModel ViewModel { get; }

    public ButtonPage(ButtonViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}