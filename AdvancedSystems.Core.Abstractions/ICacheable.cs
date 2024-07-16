using System;

namespace AdvancedSystems.Core.Abstractions
{
    /// <summary>
    ///     Provides the cache options for an entry in <seealso cref="ICachingService"/>.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.distributedcacheentryoptions"/>
    public interface ICacheable
    {
        /// <summary>
        ///     Gets or sets an absolute expiration date for the cache entry.
        /// </summary>
        DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        ///     Gets or sets an absolute expiration time, relative to now.
        /// </summary>
        TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        /// <summary>
        ///     Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will
        ///     be removed. This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        TimeSpan? SlidingExpiration { get; set; }
    }
}
