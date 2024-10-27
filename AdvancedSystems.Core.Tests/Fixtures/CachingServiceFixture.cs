using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Moq;

namespace AdvancedSystems.Core.Tests.Fixtures;

public sealed class CachingServiceFixture
{
    private readonly MemoryDistributedCache _memoryCache;

    public CachingServiceFixture()
    {
        var options = Options.Create(new MemoryDistributedCacheOptions());
        this._memoryCache = new MemoryDistributedCache(options);

        this.DistributedCache
            .Setup(dc => dc.Get(It.IsAny<string>()))
            .Returns((string key) => this._memoryCache.Get(key));

        this.DistributedCache
            .Setup(dc => dc.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string key, CancellationToken cancellationToken) =>
            {
                return this._memoryCache.Get(key);
            });

        this.DistributedCache
            .Setup(dc => dc.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()))
            .Callback<string, byte[], DistributedCacheEntryOptions>((key, value, options) =>
            {
                this._memoryCache.Set(key, value, options);
            });

        this.DistributedCache
            .Setup(dc => dc.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>(async (key, value, options, cancellationToken) =>
            {
                await this._memoryCache.SetAsync(key, value, options, cancellationToken);
            })
            .Returns(Task.CompletedTask);

        this.CachingService = new CachingService(this.DistributedCache.Object, this.SerializationService.Object);
    }

    #region Properties

    public Mock<IDistributedCache> DistributedCache { get; private set; } = new();

    public Mock<ISerializationService> SerializationService { get; private set; } = new();

    public ICachingService CachingService { get; private set; }

    #endregion
}