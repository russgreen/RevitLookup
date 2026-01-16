using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;

[UsedImplicitly]
public sealed partial class ToggleSwitchViewModel : ObservableObject
{
    [ObservableProperty] private bool _isStandardToggleSwitchEnabled = true;
}