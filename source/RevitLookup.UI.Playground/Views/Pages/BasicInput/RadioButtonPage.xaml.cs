using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class RadioButtonPage : INavigableView<RadioButtonViewModel>
{
    public RadioButtonViewModel ViewModel { get; }

    public RadioButtonPage(RadioButtonViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}