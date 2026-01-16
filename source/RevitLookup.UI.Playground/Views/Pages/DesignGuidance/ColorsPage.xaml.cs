using System.Windows;
using System.Windows.Controls;
using RevitLookup.UI.Playground.Views.Pages.DesignGuidance.ColorCategories;

namespace RevitLookup.UI.Playground.Views.Pages.DesignGuidance;

public sealed partial class ColorsPage
{
    public ColorsPage()
    {
        DataContext = this;
        InitializeComponent();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var self = (ComboBox)sender;
        switch (self.SelectedIndex)
        {
            case 0:
                ColorSubpageNavigationFrame.Navigate(new TextSection());
                break;
            case 1:
                ColorSubpageNavigationFrame.Navigate(new FillSection());
                break;
            case 2:
                ColorSubpageNavigationFrame.Navigate(new StrokeSection());
                break;
            case 3:
                ColorSubpageNavigationFrame.Navigate(new BackgroundSection());
                break;
            case 4:
                ColorSubpageNavigationFrame.Navigate(new SignalSection());
                break;
        }
    }

    private void OnSelectorLoaded(object sender, RoutedEventArgs args)
    {
        var self = (ComboBox)sender;
        self.SelectedItem = self.Items[0];
    }
}