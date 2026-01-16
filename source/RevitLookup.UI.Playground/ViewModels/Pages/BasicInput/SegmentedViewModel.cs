using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitLookup.UI.Playground.ViewModels.Pages.BasicInput;

public sealed class SegmentedViewModel : ObservableObject
{
    public List<string> SegmentedLabels { get; } = new Faker<string>()
        .CustomInstantiator(faker => faker.Music.Genre())
        .Generate(5);
}