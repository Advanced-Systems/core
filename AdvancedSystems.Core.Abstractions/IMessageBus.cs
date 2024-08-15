using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Defines an in-memory message bus for publishing and subscribing to messages of type <see cref="IMessage"/>.
/// </summary>
public interface IMessageBus : IAsyncDisposable
{
    /// <summary>
    ///     Publishes a message to the message bus.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the message to subscribe to. Must implement <see cref="IMessage"/>.
    /// </typeparam>
    /// <param name="message">
    ///     The message to be published.
    /// </param>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to send messages to the bus. Subscribers will receive these messages asynchronously.
    /// </remarks>
    ValueTask PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class, IMessage;

    /// <summary>
    ///     Subscribes to messages of type <typeparamref name="T"/> from the message bus.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the message to subscribe to. Must implement <see cref="IMessage"/>.
    /// </typeparam>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     This method is used to receive messages of the specified type from the bus. If no messages
    ///     of the type are currently available, it will wait until one becomes available.
    /// </returns>
    /// <exception cref="InvalidCastException">
    ///     Thrown when a message retrieved from the channel cannot be cast to the type <typeparamref name="T"/>.
    /// </exception>
    /// <exception cref="ChannelClosedException">
    ///     Thrown when the channel is closed and no more messages are available. This indicates that the channel
    ///     has been marked as complete and will not accept any more messages.
    /// </exception>
    ValueTask<T> SubscribeAsync<T>(CancellationToken cancellationToken = default) where T : class, IMessage;
}
