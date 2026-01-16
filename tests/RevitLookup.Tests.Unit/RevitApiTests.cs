using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;
using RevitLookup.Core;
using TUnit.Core.Executors;

namespace RevitLookup.Tests.Unit;

public sealed class RevitApiTests : RevitApiTest
{
    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Parameters_Builtin_ShouldCreateAllCategories()
    {
        var builtInParameters = Enum.GetValues<BuiltInParameter>();

        foreach (var builtInParameter in builtInParameters)
        {
            var parameter = RevitShell.GetBuiltinParameter(builtInParameter);

            await Assert.That(parameter.Definition.Name).IsNotEmpty();
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Categories_Builtin_ShouldCreateAllCategories()
    {
        var builtInCategories = Enum.GetValues<BuiltInCategory>();

        foreach (var builtInCategory in builtInCategories)
        {
            var category = RevitShell.GetBuiltinCategory(builtInCategory);

            await Assert.That(category.Name).IsNotEmpty();
        }
    }
}