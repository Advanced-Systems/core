using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.Logging;

using Moq;

namespace AdvancedSystems.Core.Tests.Fixtures;

public sealed class MessageBusFixture
{
    public MessageBusFixture()
    {
        this.Logger = new Mock<ILogger<MessageBus>>();
        this.MessageBus = new MessageBus(this.Logger.Object);
    }

    #region Properties

    public Mock<ILogger<MessageBus>> Logger { get; private set; }

    public IMessageBus MessageBus { get; private set; }

    #endregion
}
