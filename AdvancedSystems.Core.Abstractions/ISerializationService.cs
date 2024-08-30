using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Defines functionality to serialize objects to UTF-8 encoded JSON and deserialize UTF-8 encoded JSON into objects.
/// </summary>
public interface ISerializationService
{
    #region Methods

    /// <summary>
    ///     Converts the provided value into a <see cref="string"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value to serialize.
    /// </typeparam>
    /// <param name="value">
    ///     The <paramref name="value"/> to convert and write.
    /// </param>
    /// <param name="typeInfo">
    ///     The metadata for the specified type.
    /// </param>
    /// <returns>
    ///     A <seealso cref="string"/> representation of the <paramref name="value"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <seealso cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
    /// </exception>
    ReadOnlySpan<byte> Serialize<T>(T value, JsonTypeInfo<T> typeInfo) where T : class;

    /// <summary>
    ///     Parses the text representing a single JSON value into a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to deserialize the JSON value into.
    /// </typeparam>
    /// <param name="buffer">
    ///     JSON text to parse.
    /// </param>
    /// <param name="typeInfo">
    ///     The metadata for the specified type.
    /// </param>
    /// <returns>
    ///     A <typeparamref name="T"/> representation of the JSON value.
    /// </returns>
    /// <exception cref="JsonException">
    ///     The JSON is invalid, <typeparamref name="T"/> is not compatible with the JSON, or there is remaining data in the Stream.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     There is no compatible <seealso cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
    /// </exception>
    T? Deserialize<T>(ReadOnlySpan<byte> buffer, JsonTypeInfo<T> typeInfo) where T : class;

    #endregion
}