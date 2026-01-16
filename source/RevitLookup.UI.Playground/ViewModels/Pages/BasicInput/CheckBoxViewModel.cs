using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;

[UsedImplicitly]
public sealed partial class CheckBoxViewModel : ObservableObject
{
    [ObservableProperty] private bool _isStandardCheckBoxEnabled = true;
    [ObservableProperty] private bool _isThreeStateCheckBoxEnabled = true;
}