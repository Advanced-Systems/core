using System;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using AdvancedSystems.Core.Abstractions;

namespace AdvancedSystems.Core.Common;

/// <inheritdoc cref="ISerializationService" />
public static class ObjectSerializer
{
    /// <inheritdoc cref="ISerializationService.Serialize{T}(T, JsonTypeInfo{T})" />
    public static ReadOnlySpan<byte> Serialize<T>(T value, JsonTypeInfo<T> typeInfo) where T : class
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value, typeInfo);
        return buffer.WrittenSpan;
    }

    /// <inheritdoc cref="ISerializationService.Deserialize{T}(byte[], JsonTypeInfo{T})" />
    public static T? Deserialize<T>(ReadOnlySpan<byte> buffer, JsonTypeInfo<T> typeInfo) where T : class
    {
        return JsonSerializer.Deserialize(buffer, typeInfo);
    }
}