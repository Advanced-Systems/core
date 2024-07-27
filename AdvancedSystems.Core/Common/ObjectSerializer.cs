using System;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdvancedSystems.Core.Common;

/// <summary>
///     Provides functionality to serialize objects to UTF-8 encoded JSON and deserialize UTF-8 encoded JSON into objects.
/// </summary>
public static class ObjectSerializer
{
    /// <summary>
    ///     Converts the provided value into a <seealso cref="string"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="value">The <paramref name="value"/> to convert and write.</param>
    /// <returns>A <seealso cref="string"/> representation of the <paramref name="value"/>.</returns>
    /// <exception cref="NotSupportedException">There is no compatible <seealso cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.</exception>
    public static ReadOnlySpan<byte> Serialize<T>(T value) where T : class
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan;
    }

    /// <summary>
    ///     Parses the text representing a single JSON value into a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
    /// <param name="buffer">JSON text to parse.</param>
    /// <returns>A <typeparamref name="T"/> representation of the JSON value.</returns>
    /// <exception cref="JsonException">The JSON is invalid, <typeparamref name="T"/> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="NotSupportedException">There is no compatible <seealso cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.</exception>
    public static T Deserialize<T>(ReadOnlySpan<byte> buffer) where T : class
    {
        var payload = new Utf8JsonReader(buffer);
        return JsonSerializer.Deserialize<T>(ref payload)!;
    }
}
