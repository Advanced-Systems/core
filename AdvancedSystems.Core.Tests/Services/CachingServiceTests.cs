using System;
using System.Threading;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Tests.Fixtures;

using Xunit;

namespace AdvancedSystems.Core.Tests.Services;

public class CachingServiceTests : IClassFixture<CachingServiceFixture>
{
    private readonly CachingServiceFixture _fixture;

    public CachingServiceTests(CachingServiceFixture fixture)
    {
        this._fixture = fixture;
    }

    public record Person : ICacheable
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }

    #region Tests

    [Fact]
    public async Task TestCachingRoundtrip()
    {
        // Arrange
        string key = "test";
        var expected = new Person
        {
            FirstName = "Stefan",
            LastName = "Greve",
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(1)
        };

        // Act
        await this._fixture.CachingService.SetAsync(key, expected, CancellationToken.None);
        Person? actual = await this._fixture.CachingService.GetAsync<Person>(key, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected.FirstName, actual?.FirstName);
        Assert.Equal(expected.LastName, actual?.LastName);
    }

    #endregion
}
