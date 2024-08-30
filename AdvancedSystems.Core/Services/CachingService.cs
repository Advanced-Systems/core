using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;

using Microsoft.Extensions.Caching.Distributed;

namespace AdvancedSystems.Core.Services;

/// <inheritdoc cref="ICachingService" />
public sealed class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ISerializationService _serializationService;

    public CachingService(IDistributedCache distributedCache, ISerializationService serializationService)
    {
        this._distributedCache = distributedCache;
        _serializationService = serializationService;
    }

    #region Methods

    /// <inheritdoc />
    public async ValueTask SetAsync<T>(string key, T value, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default) where T : class
    {
        var options = new CacheOptions();
        await this.SetAsync(key, value, jsonTypeInfo, options, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask SetAsync<T>(string key, T value, JsonTypeInfo<T> jsonTypeInfo, CacheOptions options, CancellationToken cancellationToken = default) where T : class
    {
        byte[] cacheValue = this._serializationService.Serialize(value, jsonTypeInfo);
        await this._distributedCache.SetAsync(key, cacheValue, options, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<T?> GetAsync<T>(string key, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default) where T : class
    {
        byte[]? cachedValues = await this._distributedCache.GetAsync(key, cancellationToken);

        if (cachedValues == null || cachedValues.Length == 0) return default;

        T? @object = this._serializationService.Deserialize(cachedValues, jsonTypeInfo);
        return @object;
    }

    #endregion

}
