using System.Windows.Data;
using RevitLookup.UI.Playground.Models;
using RevitLookup.UI.Playground.ViewModels.Pages.Collections;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Collections;

public sealed partial class DataGridPage : INavigableView<DataGridViewModel>
{
    public DataGridViewModel ViewModel { get; }

    public DataGridPage(DataGridViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();

        GroupingDataGrid.Items.GroupDescriptions!.Clear();
        GroupingDataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Person.Company)));
    }
}