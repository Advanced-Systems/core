using System;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Security.Common;

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

    public async ValueTask SetAsync<T>(string? key, T value, CancellationToken cancellationToken = default) where T : class, ICacheable, new()
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        byte[] cacheValue = ObjectSerializer.Serialize(value).ToArray();
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = value?.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = value?.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = value?.SlidingExpiration,
        };

        await this._distributedCache.SetAsync(key, cacheValue, options, cancellationToken);
    }

    public async ValueTask<T?> GetAsync<T>(string? key, CancellationToken cancellationToken = default) where T : class, ICacheable, new()
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        byte[]? cachedValues = await this._distributedCache.GetAsync(key, cancellationToken);

        if (cachedValues == null || cachedValues.Length == 0) return default;

        T @object = ObjectSerializer.Deserialize<T>(cachedValues);
        return @object;
    }

    #endregion

}
