using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

namespace AdvancedSystems.Core.Tests.Fixtures;

public sealed class MessageBusFixture
{
    public MessageBusFixture()
    {
        this.MessageBus = new MessageBus();
    }

    #region Properties

    public IMessageBus MessageBus { get; private set; }

    #endregion
}
