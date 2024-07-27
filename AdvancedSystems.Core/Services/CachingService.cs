using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Common;

using Microsoft.Extensions.Caching.Distributed;

namespace AdvancedSystems.Core.Services;

public sealed class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;

    public CachingService(IDistributedCache distributedCache)
    {
        this._distributedCache = distributedCache;
    }

    #region Methods

    public async ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var options = new CacheOptions();
        await this.SetAsync<T>(key, value, options, cancellationToken);
    }

    public async ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken = default) where T : class
    {
        byte[] cacheValue = ObjectSerializer.Serialize(value).ToArray();
        await this._distributedCache.SetAsync(key, cacheValue, options, cancellationToken);
    }

    public async ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        byte[]? cachedValues = await this._distributedCache.GetAsync(key, cancellationToken);

        if (cachedValues == null || cachedValues.Length == 0) return default;

        T @object = ObjectSerializer.Deserialize<T>(cachedValues);
        return @object;
    }

    #endregion

}
