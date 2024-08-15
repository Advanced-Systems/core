using System;

using Microsoft.Extensions.Caching.Distributed;

namespace AdvancedSystems.Core.Abstractions;

public class CacheOptions
{
    /// <summary>
    ///     Provides the cache options for an entry in <seealso cref="ICachingService"/>.
    /// </summary>
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    /// <summary>
    ///     Gets or sets an absolute expiration time, relative to now.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    ///     Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
    ///     This will not extend the entry lifetime beyond the absolute expiration (if set).
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    public static implicit operator DistributedCacheEntryOptions(CacheOptions options)
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration,
        };
    }
}
