using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.DependencyInjection;
using AdvancedSystems.Core.Tests.Fixtures;
using AdvancedSystems.Core.Tests.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace AdvancedSystems.Core.Tests.Services;

public class SerializationServiceTests : IClassFixture<SerializationServiceFixture>
{
    private readonly SerializationServiceFixture _sut;

    public SerializationServiceTests(SerializationServiceFixture fixture)
    {
        this._sut = fixture;
    }

    #region Tests

    [Fact]
    public void TestSerializationRoundtrip()
    {
        // Arrange
        var expected = new Person("Stefan", "Greve");

        // Act
        var serialized = this._sut.SerializationService.Serialize(expected, PersonContext.Default.Person);
        var actual = this._sut.SerializationService.Deserialize(serialized, PersonContext.Default.Person);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public async Task TestAddSerializationService()
    {
        // Arrange
        using var hostBuilder = await new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer();
                builder.ConfigureServices(services =>
                {
                    services.AddSerializationService();
                });
                builder.Configure(app =>
                {

                });
            })
            .StartAsync();

        // Act
        var serializationService = hostBuilder.Services.GetService<ISerializationService>();

        // Assert
        Assert.NotNull(serializationService);
        await hostBuilder.StopAsync();
    }

    #endregion
}
