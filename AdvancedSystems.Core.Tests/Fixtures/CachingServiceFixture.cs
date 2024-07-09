using AdvancedSystems.Core.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace AdvancedSystems.Core.Tests.Fixtures;

public sealed class CachingServiceFixture
{
    public IDistributedCache DistributedCache { get; private set; }

    public CachingService CachingService { get; private set; }

    public CachingServiceFixture()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions());
        this.DistributedCache = new MemoryDistributedCache(options);
        this.CachingService = new CachingService(this.DistributedCache);
    }
}
