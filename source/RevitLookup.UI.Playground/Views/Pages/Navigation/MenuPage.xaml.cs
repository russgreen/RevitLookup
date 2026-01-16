using System.Windows;
using System.Windows.Controls;
using RevitLookup.UI.Playground.ViewModels.Pages.Navigation;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Views.Pages.Navigation;

public sealed partial class MenuPage : INavigableView<MenuViewModel>
{
    public MenuViewModel ViewModel { get; }

    public MenuPage(MenuViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    private void OnMenuItemClicked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            if (e.OriginalSource is MenuItem originalMenuItem && originalMenuItem == menuItem)
            {
                StatusMenuItem.Visibility = Visibility.Visible;
                StatusMenuItem.Text = menuItem.Tag != null ? $"You pressed {menuItem.Tag}" : $"You pressed {menuItem.Header}";
            }

            if (menuItem.Parent is MenuItem parentMenuItem)
            {
                parentMenuItem.Focus();
            }
            else
            {
                menuItem.Focus();
            }
        }
    }
}