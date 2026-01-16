using System.Windows;
using RevitLookup.Common.Utils;

namespace RevitLookup.UI.Playground.Controls;

public sealed partial class HeaderTile
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(HeaderTile), new PropertyMetadata(""));
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("ColorExplanation", typeof(string), typeof(HeaderTile), new PropertyMetadata(""));
    public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(nameof(Link), typeof(string), typeof(HeaderTile), new PropertyMetadata(null));
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(HeaderTile), new PropertyMetadata(null));

    public HeaderTile()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Link
    {
        get => (string)GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }


    private void OnTileClicked(object sender, RoutedEventArgs e)
    {
        ProcessTasks.StartShell(Link);
    }
}