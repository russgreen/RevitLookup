using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class CheckBoxPage : INavigableView<CheckBoxViewModel>
{
    public CheckBoxViewModel ViewModel { get; }

    public CheckBoxPage(CheckBoxViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}