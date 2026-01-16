using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;

[UsedImplicitly]
public partial class ButtonViewModel : ObservableObject
{
    [ObservableProperty] private bool _isStandardButtonEnabled = true;
    [ObservableProperty] private bool _isPrimaryButtonEnabled = true;
    [ObservableProperty] private bool _isSecondaryButtonEnabled = true;
    [ObservableProperty] private bool _isDangerButtonEnabled = true;
    [ObservableProperty] private bool _isTransparentButtonEnabled = true;
}