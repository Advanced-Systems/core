using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;

namespace AdvancedSystems.Core.Services;

public sealed class MessageBus : IMessageBus
{
    private readonly Channel<IMessage> _channel;

    public MessageBus()
    {
        var options = new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
        };

        this._channel = Channel.CreateUnbounded<IMessage>(options);
    }

    #region Methods

    public async ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class, IMessage
    {
        await this._channel.Writer.WriteAsync(message, cancellationToken);
    }

    public async ValueTask<T> SubscribeAsync<T>(CancellationToken cancellationToken) where T : class, IMessage
    {
        while (await this._channel.Reader.WaitToReadAsync(cancellationToken))
        {
            T message = await this._channel.Reader.ReadAsync(cancellationToken) as T
                ?? throw new InvalidCastException($"Failed to cast message to type of {typeof(T)}.");

            return message;
        }

        throw new ChannelClosedException("The message channel has been closed.");
    }

    public ValueTask DisposeAsync()
    {
        this._channel.Writer.TryComplete();
        return ValueTask.CompletedTask;
    }

    #endregion
}
