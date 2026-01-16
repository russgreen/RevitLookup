using RevitLookup.UI.Playground.ViewModels.Pages.Collections;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Collections;

public sealed partial class ListViewPage : INavigableView<ListViewViewModel>
{
    public ListViewViewModel ViewModel { get; }

    public ListViewPage(ListViewViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}