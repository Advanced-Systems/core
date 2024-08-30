using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Represents a cache of serialized values.
/// </summary>
public interface ICachingService
{
    #region Methods

    /// <summary>
    ///     Sets a values in the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">
    ///     The type argument must be a reference type and  have a public parameterless constructor.
    /// </typeparam>
    /// <param name="key">
    ///     A string identifying the requested values.
    /// </param>
    /// <param name="value">
    ///     The values to set in the cache.
    /// </param>
    /// <param name="typeInfo">
    ///     The metadata for the specified type.
    /// </param>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     A ValueTask representing the asynchronous operation.
    /// </returns>
    ValueTask SetAsync<T>(string key, T value, JsonTypeInfo<T> typeInfo, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Sets a values in the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">
    ///     The type argument must be a reference type and  have a public parameterless constructor.
    /// </typeparam>
    /// <param name="key">
    ///     A string identifying the requested values.
    /// </param>
    /// <param name="value">
    ///     The values to set in the cache.
    /// </param>
    /// <param name="typeInfo">
    ///     The metadata for the specified type.
    /// </param>
    /// <param name="options">
    ///     The cache options for the value.
    /// </param>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     A ValueTask representing the asynchronous operation.
    /// </returns>
    ValueTask SetAsync<T>(string key, T value, JsonTypeInfo<T> typeInfo, CacheOptions options, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Gets a values from the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">
    ///     The type argument must be a reference type and  have a public parameterless constructor.
    /// </typeparam>
    /// <param name="key">
    ///     A string identifying the requested values.
    /// </param>
    /// <param name="typeInfo">
    ///     The metadata for the specified type.
    /// </param>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     A ValueTask containing the result of type <typeparamref name="T"/> representing the asynchronous
    ///     operation. The result is null if <paramref name="key"/> can not be identified in the cache.
    /// </returns>
    ValueTask<T?> GetAsync<T>(string key, JsonTypeInfo<T> typeInfo, CancellationToken cancellationToken = default) where T : class;

    #endregion
}