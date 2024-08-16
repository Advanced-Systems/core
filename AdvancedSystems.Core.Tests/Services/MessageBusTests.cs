using System;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Tests.Fixtures;

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

    public record Passport(Guid Id) : IMessage;

    #region Tests

    [Fact]
    public async Task TestPublishSubscribeRoundtrip_HappyPath()
    {
        // Arrange
        var messageBus = this._fixture.MessageBus;
        var expected = new Message(Guid.NewGuid());

        // Act
        await messageBus.PublishAsync(expected);
        Message actual = await messageBus.SubscribeAsync<Message>();

        // Assert
        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public async Task TestPublishSubscribeRoundtrip_UnhappyPath()
    {
        // Arrange
        var messageBus = this._fixture.MessageBus;
        var expected = new Message(Guid.NewGuid());

        // Act
        await messageBus.PublishAsync(expected);

        async Task SubscribeAsync()
        {
            await messageBus.SubscribeAsync<Passport>();
        }

        // Assert
        await Assert.ThrowsAsync<InvalidCastException>(SubscribeAsync);
    }

    #endregion
}
