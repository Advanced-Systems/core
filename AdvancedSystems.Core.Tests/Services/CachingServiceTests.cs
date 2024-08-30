using System;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.DependencyInjection;
using AdvancedSystems.Core.Tests.Fixtures;
using AdvancedSystems.Core.Tests.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

using Xunit;

namespace AdvancedSystems.Core.Tests.Services;

public class CachingServiceTests : IClassFixture<CachingServiceFixture>
{
    private readonly CachingServiceFixture _fixture;

    public CachingServiceTests(CachingServiceFixture fixture)
    {
        this._fixture = fixture;
    }

    #region Tests

    [Fact]
    public async Task TestCachingRoundtrip_HappyPath()
    {
        // Arrange
        string key = "test";
        var expected = new Person("Stefan", "Greve");
        var options = new CacheOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(1),
        };

        // Act
        await this._fixture.CachingService.SetAsync(key, expected, PersonContext.Default.Person, options, CancellationToken.None);
        Person? actual = await this._fixture.CachingService.GetAsync(key, PersonContext.Default.Person, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected.FirstName, actual?.FirstName);
        Assert.Equal(expected.LastName, actual?.LastName);
        this._fixture.DistributedCache.Verify(service => service.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }

    [Fact]
    public async Task TestCachingRoundtrip_UnhappyPath()
    {
        // Arrange
        var expected = new Person("Stefan", "Greve");

        // Act
        await this._fixture.CachingService.SetAsync("test1", expected, PersonContext.Default.Person, CancellationToken.None);
        Person? actual = await this._fixture.CachingService.GetAsync("test2", PersonContext.Default.Person, CancellationToken.None);

        // Assert
        Assert.Null(actual);
        this._fixture.DistributedCache.Verify(service => service.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }

    [Fact]
    public async Task TestAddCachingService()
    {
        // Arrange
        using var hostBuilder = await new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer();
                builder.ConfigureServices(services =>
                {
                    services.AddDistributedMemoryCache();
                    services.AddCachingService();
                });
                builder.Configure(app =>
                {

                });
            })
            .StartAsync();

        // Act
        var cachingService = hostBuilder.Services.GetService<ICachingService>();

        // Assert
        Assert.NotNull(cachingService);
        await hostBuilder.StopAsync();
    }

    #endregion
}
