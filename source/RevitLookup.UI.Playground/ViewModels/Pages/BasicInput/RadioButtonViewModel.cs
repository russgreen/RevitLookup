using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;

[UsedImplicitly]
public sealed partial class RadioButtonViewModel : ObservableObject
{
    [ObservableProperty] private bool _isStandardRadioButtonEnabled = true;
}