using System;
using System.Threading;
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

public class MessageBusTests : IClassFixture<MessageBusFixture>
{
    private readonly MessageBusFixture _fixture;

    public MessageBusTests(MessageBusFixture fixture)
    {
        this._fixture = fixture;
    }

    public record Message(Guid Id) : IMessage;

    public record Passport(string FirstName, string LastName, Guid Id) : IMessage;

    #region Tests

    [Fact]
    public async Task TestPublishSubscribeRoundtrip_HappyPath()
    {
        // Arrange
        var messageBus = this._fixture.MessageBus;
        var expected = new Message(Guid.NewGuid());
        string channelName = "internal";

        // Act
        bool wasCreated = messageBus.Register(channelName);
        await messageBus.PublishAsync(expected);
        Message actual = await messageBus.SubscribeAsync<Message>(channelName);

        // Assert
        Assert.True(wasCreated);
        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public async Task TestPublishSubscribeRoundtrip_UnhappyPath()
    {
        // Arrange
        var messageBus = this._fixture.MessageBus;
        var expected = new Message(Guid.NewGuid());
        string channelName = "internal";
        string topic = "test";

        // Act
        bool wasCreated = messageBus.Register(channelName, topic);
        await messageBus.PublishAsync(expected, topic, CancellationToken.None);

        Passport? actual = await messageBus.SubscribeAsync<Passport>(channelName, null, CancellationToken.None);

        // Assert
        Assert.True(wasCreated);
        Assert.Null(actual);
    }

    [Fact]
    public async Task TestPublishAsync_ThrowExceptionWithoutRegistration()
    {
        // Arrange
        var messageBus = this._fixture.MessageBus;
        var expected = new Message(Guid.NewGuid());

        // Act
        async Task PublishAsync()
        {
            await messageBus.PublishAsync(expected);
        }

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(PublishAsync);
    }

    [Fact]
    public async Task TestAddMessageBus()
    {
        // Arrange
        using var hostBuilder = await new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer();
                builder.ConfigureServices(services =>
                {
#pragma warning disable Preview008 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    services.AddMessageBus();
#pragma warning restore Preview008 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                });
                builder.Configure(app =>
                {

                });
            })
            .StartAsync();

        // Act
        var messageBus = hostBuilder.Services.GetService<IMessageBus>();

        // Assert
        Assert.NotNull(messageBus);
        await hostBuilder.StopAsync();
    }

    #endregion
}
