using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.ViewModels.Pages.DialogsAndFlyouts;

[UsedImplicitly]
public partial class FlyoutViewModel : ObservableObject
{
    [ObservableProperty] private bool _isStandardFlyoutOpen;
    [ObservableProperty] private bool _isRightFlyoutOpen;

    [RelayCommand]
    private void OnStandardButtonClick()
    {
        if (!IsStandardFlyoutOpen)
        {
            IsStandardFlyoutOpen = true;
        }
    }

    [RelayCommand]
    private void OnRightButtonClick()
    {
        if (!IsRightFlyoutOpen)
        {
            IsRightFlyoutOpen = true;
        }
    }
}