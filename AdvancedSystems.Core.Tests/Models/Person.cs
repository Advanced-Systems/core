using System.Text.Json.Serialization;

namespace AdvancedSystems.Core.Tests.Models;

internal record Person(string FirstName, string LastName);

[JsonSourceGenerationOptions(/*GenerationMode = JsonSourceGenerationMode.Default*/)]
[JsonSerializable(typeof(Person), GenerationMode = JsonSourceGenerationMode.Default)]
internal partial class PersonContext : JsonSerializerContext;
