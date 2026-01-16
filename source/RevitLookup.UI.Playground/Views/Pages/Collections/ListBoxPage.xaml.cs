using RevitLookup.UI.Playground.ViewModels.Pages.Collections;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Collections;

public sealed partial class ListBoxPage : INavigableView<ListBoxViewModel>
{
    public ListBoxViewModel ViewModel { get; }

    public ListBoxPage(ListBoxViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}