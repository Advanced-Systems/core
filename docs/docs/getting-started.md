# Getting Started

## Serialization Service

### Basic Usage

The default implementation of `ISerializationService`, and by extension, `ICachingService`,
is supposed to be used with source generation (from `System.Text.Json`) to serialize or
deserialize a type. This facilitates assembly trimming and improves performance by
reducing private memory usage. By default, `System.Text.Json` uses reflection to collect
and cache metadata.

To disable the default behavior, set the following MSBuild property in your project file:

```xml
<PropertyGroup>
  <JsonSerializerIsReflectionEnabledByDefault>false</JsonSerializerIsReflectionEnabledByDefault>
</PropertyGroup>
```

Create a partial class that derives from `JsonSerializerContext`.

- For an entire context, use the `JsonSourceGenerationOptionsAttribute.GenerationMode` property.
- For an individual type, use the `JsonSerializableAttribute.GenerationMode` property.

```csharp
// data structure to serialize/deserialize
internal record Person(string FirstName, string LastName);

// context class configured to do source generation for the preceding Person record
[JsonSerializable(typeof(Person), GenerationMode = JsonSourceGenerationMode.Default)]
internal partial class PersonContext : JsonSerializerContext;
```

Next, add `ISerializationService` to the DI container and consume the service:

```csharp
// register service
services.AddSerializationService();

// retrieve an instance of a service
var serializationService = hostBuilder.Services.GetService<ISerializationService>();

// serialize and deserialize an object
var serialized = serializationService.Serialize(expected, PersonContext.Default.Person);
var actual = serializationService.Deserialize(serialized, PersonContext.Default.Person);
```

### Further Reading

- https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation

### See Also

- [`ObjectSerializer`](/api/AdvancedSystems.Core.Common.ObjectSerializer.html)
- [`CachingService`](/api/AdvancedSystems.Core.Services.CachingService.html)
