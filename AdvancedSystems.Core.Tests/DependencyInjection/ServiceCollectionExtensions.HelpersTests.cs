using System.Collections.Generic;

using AdvancedSystems.Core.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace AdvancedSystems.Core.Tests.DependencyInjection;

public sealed class ServiceCollectionExtensionsHelperTests
{
    public record MyOptions
    {
        public string? ConnectionString { get; set; }

        public string? Port { get; set; }
    }

    #region Tests

    /// <summary>
    ///     Tests that <seealso cref="ServiceCollectionExtensions.TryAddOptions{TOptions}(IServiceCollection, IConfigurationSection)"/>
    ///     configures options from configuration sections only once.
    /// </summary>
    [Fact]
    public void TestTryAddOptions_FromConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        var expectedOption = new MyOptions
        {
            ConnectionString = "localhost",
            Port = "443"
        };

        var configurationSection = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>($"{nameof(MyOptions)}:{nameof(MyOptions.ConnectionString)}", expectedOption.ConnectionString),
                new KeyValuePair<string, string?>($"{nameof(MyOptions)}:{nameof(MyOptions.Port)}", expectedOption.Port),
            ])
            .Build()
            .GetSection(nameof(MyOptions));

        // Act
        services.TryAddOptions<MyOptions>(configurationSection);
        services.TryAddOptions<MyOptions>(configurationSection);
        var serviceProvider = services.BuildServiceProvider();
        var actualOption = serviceProvider.GetService<IOptions<MyOptions>>();
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<MyOptions>>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(actualOption);
            Assert.Equal(expectedOption.ConnectionString, actualOption.Value.ConnectionString);
            Assert.Equal(expectedOption.Port, actualOption.Value.Port);
            Assert.Single(services, service => service.ServiceType == typeof(IConfigureOptions<MyOptions>));
        });
    }

    /// <summary>
    ///     Tests that <seealso cref="ServiceCollectionExtensions.TryAddOptions{TOptions}(IServiceCollection, System.Action{TOptions})"/>
    ///      configures options from the action builder only once.
    /// </summary>
    [Fact]
    public void TestTryAddOptions_FromActionsBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        var expectedOption = new MyOptions
        {
            ConnectionString = "localhost",
            Port = "443"
        };
        int counter = 0;

        // Act
        services.TryAddOptions<MyOptions>(options =>
        {
            counter++;
            options.ConnectionString = expectedOption.ConnectionString;
            options.Port = expectedOption.Port;
        });

        services.TryAddOptions<MyOptions>(_ => counter++);
        services.TryAddOptions<MyOptions>(_ => counter++);
        services.TryAddOptions<MyOptions>(_ => counter++);

        var serviceProvider = services.BuildServiceProvider();
        var actualOption = serviceProvider.GetService<IOptions<MyOptions>>();

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualOption);
            Assert.Equal(1, counter);
            Assert.Equal(expectedOption.ConnectionString, actualOption.Value.ConnectionString);
            Assert.Equal(expectedOption.Port, actualOption.Value.Port);
        });
    }

    #endregion
}