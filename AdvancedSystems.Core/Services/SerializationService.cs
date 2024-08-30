using System;
using System.Text.Json.Serialization.Metadata;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Common;

namespace AdvancedSystems.Core.Services;

/// <inheritdoc cref="ISerializationService" />
public sealed class SerializationService : ISerializationService
{
    #region Methods

    /// <inheritdoc />
    public ReadOnlySpan<byte> Serialize<T>(T value, JsonTypeInfo<T> typeInfo) where T : class
    {
        return ObjectSerializer.Serialize(value, typeInfo);
    }

    /// <inheritdoc />
    public T? Deserialize<T>(ReadOnlySpan<byte> buffer, JsonTypeInfo<T> typeInfo) where T : class
    {
        return ObjectSerializer.Deserialize(buffer, typeInfo);
    }

    #endregion
}
