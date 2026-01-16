using RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.BasicInput;

public sealed partial class HyperlinkButtonPage : INavigableView<HyperlinkButtonViewModel>
{
    public HyperlinkButtonViewModel ViewModel { get; }

    public HyperlinkButtonPage(HyperlinkButtonViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}