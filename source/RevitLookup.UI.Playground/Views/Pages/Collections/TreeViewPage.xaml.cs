using RevitLookup.UI.Playground.ViewModels.Pages.Collections;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Collections;

public sealed partial class TreeViewPage : INavigableView<TreeViewViewModel>
{
    public TreeViewViewModel ViewModel { get; }

    public TreeViewPage(TreeViewViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}