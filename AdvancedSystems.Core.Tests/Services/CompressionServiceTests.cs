using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.DependencyInjection;
using AdvancedSystems.Core.Tests.Fixtures;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace AdvancedSystems.Core.Tests.Services;

public class CompressionServiceTests : IClassFixture<CompressionServiceFixture>
{
    private readonly CompressionServiceFixture _sut;

    public CompressionServiceTests(CompressionServiceFixture fixture)
    {
        this._sut = fixture;
    }

    #region Tests

    [Fact]
    public void TestCompressionRoundtrip()
    {
        // Arrange
        var buffer = Enumerable.Repeat<byte>(0xFF, 100).ToArray();

        // Act
        var compressedBuffer = this._sut.CompressionService.Compress(buffer, CompressionLevel.Optimal);
        var expandedBuffer = this._sut.CompressionService.Expand(compressedBuffer);

        // Assert
        Assert.NotEmpty(compressedBuffer);
        Assert.True(buffer.Length > compressedBuffer.Length);
        Assert.Equal(buffer, expandedBuffer);
    }

    [Fact]
    public async Task TestAddCompressionService()
    {
        // Arrange
        using var hostBuilder = await new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer();
                builder.ConfigureServices(services =>
                {
                    services.AddCompressionService();
                });
                builder.Configure(app =>
                {

                });
            })
            .StartAsync();

        // Act
        var compressionService = hostBuilder.Services.GetService<ICompressionService>();

        // Assert
        Assert.NotNull(compressionService);
        await hostBuilder.StopAsync();
    }

    #endregion
}
