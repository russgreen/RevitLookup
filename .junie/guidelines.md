# RevitLookup Guidelines

Guidelines for developing the RevitLookup project. These guidelines enforce a strict production quality standard, ensuring maintainability, scalability, and consistency across the codebase.

## 1. Project Structure

The solution follows a logical hierarchy to separate concerns, decouple the UI from Revit, and enable rapid prototyping.

### 1.1. Solution Groups (Virtual Folders)

* **`/Automation`**: Build scripts and Installers.
    * `build`: ModularPipelines projects for building and publishing the solution.
    * `install`: WiX Toolset projects for creating MSIs.
* **`/Engine`**: Core LookupEngine framework.
    * `LookupEngine.Abstractions`: **[Submodule - DO NOT MODIFY]** Interfaces and contract definitions.
    * `LookupEngine`: **[Submodule - DO NOT MODIFY]** Core decomposition and object analysis engine.
* **`/Frontend`**: UI framework and RevitLookup UI components.
    * `LookupEngine.UI.Abstractions`: **[Submodule - DO NOT MODIFY]** UI interfaces and contracts.
    * `LookupEngine.UI`: **[Submodule - DO NOT MODIFY]** Core UI components.
    * `RevitLookup.UI.Framework`: Shared UI components and base classes specific to RevitLookup.
* **`/Playground`**: The prototyping environment.
    * `RevitLookup.UI.Playground`: The mockup entry point for testing UI without Revit.
* **`/Revit`**: The Production environment (Revit Plugin).
    * `RevitLookup`: The main Plugin Host (Entry Point).
* **`/Tests`**: Testing projects.
    * `RevitLookup.Tests.Unit`: Unit tests running on TUnit.
* **Root Level**: Shared projects. Pure C#, no Revit references.
    * `RevitLookup.Abstractions`: Interfaces and contract definitions.
    * `RevitLookup.Common`: General purpose utilities.
    * `RevitLookup.ServiceDefaults`: Service configurations shared between Playground and Revit.

## 2. Architecture & Environments

The project runs in two distinct environments. Code must be written to be compatible with both where possible (UI/Logic), or isolated where specific (Revit API).

### 2.1. Revit Environment (Production)

* **Context:** Runs inside the `Revit.exe` process.
* **Entry Point:** `RevitLookup`.
* **Constraints:** Single-threaded (Main Thread), specialized API access.

### 2.2. Playground Environment (Prototyping)

* **Context:** Runs as a standalone WPF application.
* **Entry Point:** `RevitLookup.UI.Playground`.
* **Purpose:** Rapid UI development, testing, and mocking without the overhead of restarting Revit.

### 2.3. LookupEngine Architecture

RevitLookup is built on the [LookupEngine](https://github.com/lookup-foundation/LookupEngine) framework, which provides a system for analyzing object structures at runtime.

#### Descriptors

Descriptors are specialized classes that define how objects should be handled by the LookupEngine. Each descriptor is responsible for a specific type or family of types in Revit.

To add a descriptor for a new class:

1. Create a new descriptor class in the appropriate folder under `source/RevitLookup/Core/Decomposition/Descriptors/`
2. Register the descriptor in the descriptor map located at `source/RevitLookup/Core/Decomposition/DescriptorMap.cs`

#### IDescriptorResolver

This interface allows descriptors to control how methods and properties with parameters are evaluated.
In RevitLookup, `Document` serves as the context for resolution.

**Single Value Resolution:**

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ElementDescriptor.cs
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver<Document>
{
    public virtual Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.IsHidden) => ResolveIsHidden,
            _ => null
        };

        IVariant ResolveIsHidden(Document context)
        {
            return Variants.Value(element.IsHidden(context.ActiveView), "Active view");
        }
    }
}
```

**Multiple Value Resolution:**

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ElementDescriptor.cs
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver<Document>
{
    public virtual Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.GetBoundingBox) => ResolveBoundingBox,
            _ => null
        };

        IVariant ResolveBoundingBox(Document context)
        {
            return Variants.Values<BoundingBoxXYZ>(2)
                .Add(element.get_BoundingBox(null), "Model")
                .Add(element.get_BoundingBox(context.ActiveView), "Active view")
                .Consume();
        }
    }
}
```

**Disabling Methods:**

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/DocumentDescriptor.cs
public class DocumentDescriptor(Document document) : Descriptor, IDescriptorResolver
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) => Variants.Disabled,
            _ => null
        };
    }
}
```

**Targeting Specific Overloads:**

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/EntityDescriptor.cs
public sealed class EntityDescriptor(Entity entity) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Entity.Get) when parameters.Length == 1 &&
                                    parameters[0].ParameterType == typeof(string) => ResolveGetByField,
            _ => null
        };

        IVariant ResolveGetByField()
        {
            return Variants.Value(entity.Get("Parameter Name"));
        }
    }
}
```

#### IDescriptorExtension

This interface allows adding custom methods and properties to objects that don't exist in the original type.
In RevitLookup, extensions can use the `Document` as a context during registration for document-dependent elements.

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ColorDescriptor.cs
public sealed class ColorDescriptor(Color color) : Descriptor, IDescriptorExtension
{
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("HEX", () => Variants.Value(ColorRepresentationUtils.ColorToHex(color)));
        manager.Register("RGB", () => Variants.Value(ColorRepresentationUtils.ColorToRgb(color)));
        manager.Register("HSL", () => Variants.Value(ColorRepresentationUtils.ColorToHsl(color)));
    }
}
```

With context:

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/SchemaDescriptor.cs
public sealed class SchemaDescriptor(Schema schema) : Descriptor, IDescriptorExtension<Document>
{
    public void RegisterExtensions(IExtensionManager<Document> manager)
    {
        manager.Register("GetElements", context => Variants.Value(context
            .GetElements()
            .WherePasses(new ExtensibleStorageFilter(schema.GUID))
            .ToElements()));
    }
}
```

#### IDescriptorRedirector

This interface lets a descriptor redirect to another object. For example, converting from an ID to the actual element.

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ElementIdDescriptor.cs
public sealed class ElementIdDescriptor(ElementId elementId) : Descriptor, IDescriptorRedirector<Document>
{
    public bool TryRedirect(string target, Document context, out object result)
    {
        if (elementId == ElementId.InvalidElementId)
        {
            result = null;
            return false;
        }

        result = elementId.ToElement(context);
        return result is not null;
    }
}
```

#### IDescriptorCollector

This interface serves as a marker indicating that the descriptor can decompose the object's members. It's essential for allowing users to inspect an object's internal structure.

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ApplicationDescriptor.cs
public sealed class ApplicationDescriptor : Descriptor, IDescriptorCollector
{
    public ApplicationDescriptor(Application application)
    {
        Name = application.VersionName;
    }
}
```

#### IDescriptorConnector

This interface enables integration with the RevitLookup UI, allowing descriptors to add custom context menu options and commands.

```csharp
// source/RevitLookup/Core/Decomposition/Descriptors/ElementDescriptor.cs
public sealed class ElementDescriptor : Descriptor, IDescriptorConnector
{
    public void RegisterMenu(ContextMenu contextMenu)
    {
        contextMenu.AddMenuItem()
            .SetHeader("Show element")
            .SetAvailability(_element is not ElementType)
            .SetCommand(_element, element =>
            {
                Context.UiDocument.ShowElements(element);
                Context.UiDocument.Selection.SetElementIds([element.Id]);
            })
            .AddShortcut(ModifierKeys.Alt, Key.F7);
    }
}
```

#### UI Styling

The application UI is data-template based, with templates customizable for different data types. Templates are located in `source/RevitLookup/Styles/ComponentStyles/` directory.

To customize the display of a specific type:

1. Create a DataTemplate in a XAML file within the ComponentStyles directory:

```xml
// source/RevitLookup/Styles/ComponentStyles/ObjectsTree/TreeGroupTemplates.xaml
<DataTemplate
    x:Key="DefaultSummaryTreeItemTemplate"
    DataType="{x:Type decomposition:ObservableDecomposedObject}">
    <ui:TextBlock
        FontTypography="Caption"
        Text="{Binding .,
            Converter={valueConverters:SingleDescriptorLabelConverter},
            Mode=OneTime}" />
</DataTemplate>
```

2. Add a selector rule in the `TemplateSelector` class:

```csharp
// source/RevitLookup/Styles/ComponentStyles/ObjectsTree/TreeViewItemTemplateSelector.cs
public sealed class TreeViewItemTemplateSelector : DataTemplateSelector
{
    /// <summary>
    ///     Tree view row style selector
    /// </summary>
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is null) return null;

        var presenter = (FrameworkElement) container;
        var decomposedObject = (ObservableDecomposedObject) item;
        var templateName = decomposedObject.RawValue switch
        {
            Color => "SummaryMediaColorItemTemplate",
            _ => "DefaultSummaryTreeItemTemplate"
        };

        return (DataTemplate) presenter.FindResource(templateName);
    }
}
```

For custom visualization of specific data types, create specialized templates following the pattern above and register them in the appropriate style selectors.

## 3. Strict C# Production Style

All code must adhere to enterprise-grade standards. "It works" is not enough; it must be clean, readable, and robust.

### 3.1. General Principles

* **SOLID:** strictly follow Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion.
* **DRY (Don't Repeat Yourself):** Extract common logic to `RevitLookup.Common` or `RevitLookup.Abstractions`.
* **Explicit over Implicit:** Code should be self-explanatory. Avoid "magic" behavior.
* **Modern C#:** Always use the latest language features.
* **Use Span:** Utilize `Span<T>` and `ReadOnlySpan<T>` for memory-efficient data processing where applicable outside of buisness logic.
* **JetBrains Annotations:** Use JetBrains Annotations where applicable to improve code analysis.

### 3.2. Naming Conventions

* **Clarity is King:** Names must be descriptive.
* **No Abbreviations:**
    * ❌ `repo`, `config`, `ctx`, `svc`
    * ✅ `repository`, `configuration`, `context`, `service`
* **No Single-Letter Variables:**
    * ❌ `p`, `i`, `e` (except in very short lambdas or for loops).
    * ✅ `property`, `element`, `exception`.
* **Async Suffix:** Methods returning `Task` or `Task<T>` must end with `Async`.
    * ✅ `GetDataAsync()`

### 3.3. Formatting & Layout

* **File-Scoped Namespaces:** Always use `namespace RevitLookup;` (no braces).
* **Nullable Reference Types:** Enabled project-wide. Treat warnings as errors.
* **Organization:**
    1. Private Fields (if strictly necessary)
    2. Primary Constructor
    3. Public Properties
    4. Public Methods
    5. Private Methods

### 3.4. Async/Await

* **Task:** Use `Task` everywhere. Avoid `async void` (except for top-level event handlers).
* **Context:** Be mindful of the SynchronizationContext. In UI/Revit, `await` returns to the main thread by default. Use `ConfigureAwait(false)` in pure library code (Core/Common) if it doesn't touch UI/Revit API.

### 3.5. Error Handling

* **Centralized Handling:** Do not clutter business logic with `try-catch` blocks. Let exceptions propagate to a global handler or boundary.
* **Custom Exceptions:** Define semantic exceptions (e.g., `ConfigurationMissingException`) rather than throwing generic `Exception`.
* **Validation:** Validate inputs at the boundary (public methods).

### 3.6. Data Objects

* **Immutability:** Use `record` for DTOs, messages, and configuration objects.
* **Properties:** Use `{ get; init; }` for immutable properties in classes.

## 4. Dependency Injection

We use strict Constructor Injection via Primary Constructors.

* **Primary Constructors Only:** C# 12 syntax is mandatory.
* **No Manual Fields:** Do not declare `private readonly` fields for injected services.
* **Registration:**
    * Revit: `source/RevitLookup/Host.cs`
    * Playground: `source/RevitLookup.UI.Playground/Host.cs`.

```csharp
// ✅ Correct
public class ContentService(
    IContentRepository repository,
    ILogger<ContentService> logger) : IContentService
{
    public async Task<Content> GetAsync(string id)
    {
        logger.LogInformation("Getting content {Id}", id);
        return await repository.GetByIdAsync(id);
    }
}
```

## 5. MVVM Pattern (CommunityToolkit)

* **Framework:** `CommunityToolkit.Mvvm`.
* **Source Generators:** extensively use `[ObservableProperty]` and `[RelayCommand]`.
* **State:** Keep ViewModel state private, expose via generated properties.
* **Messaging:** Use `IMessenger` for loose coupling between ViewModels.

```csharp
public partial class SearchViewModel(ISearchService searchService) : ObservableObject
{
    [ObservableProperty]
    private string _query; // Generates "Query" property

    [RelayCommand]
    private async Task SearchAsync()
    {
        // ...
    }
}
```

## 6. Logging & Telemetry

* **Serilog:** The standard logging backend.
* **OpenTelemetry:** Configured for tracing.
* **Structured Logging:** **MANDATORY**. Never use string interpolation in log messages.
    * ❌ `LogInformation($"User {id} logged in")`
    * ✅ `LogInformation("User {UserId} logged in", id)`

## 7. Revit Development Best Practices

* **Toolkit:** Use `Nice3point.Revit.Toolkit` and `Nice3point.Revit.Extensions` for all Revit interactions.
* **Thread Safety:** The Revit API is single-threaded.
    * **NEVER** use `Dispatcher.Invoke`.
    * **ALWAYS** use `ExternalEvents` (via Toolkit) to marshal execution from async/background threads to the Revit Context.
* **Transactions:** explicit `Transaction` management.

## 8. Testing Strategy

* **API testing:** Use `RevitLookup.Tests.Unit` project with `Nice3point.TUnit.Revit` (runs on top of TUnit).
    * Test API only, without Revit UI references.
* **UI Testing:** Use `RevitLookup.UI.Playground` with `Debug.Playground` configuration.
    * Test UI changes without launching Revit.
    * Uses mock data simulating Revit objects.

## 9. Package Management

* **Centralized:** All versions are defined in `Directory.Packages.props`.
* **Clean csproj:** No `<Version>` tags in individual project files.
