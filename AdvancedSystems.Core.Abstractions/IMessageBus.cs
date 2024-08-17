using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Defines an in-memory message bus for publishing and subscribing to messages of type <see cref="IMessage"/> via broadcasts.
/// </summary>
public interface IMessageBus : IAsyncDisposable
{
    /// <summary>
    ///     Registers a new channel with the specified channel name and optional topic.
    /// </summary>
    /// <param name="channelName">
    ///     The name of the channel to register.
    /// </param>
    /// <param name="topic">
    ///     An optional topic associated with the channel. If not provided, the default topic will be used.
    /// </param>
    /// <returns>
    ///     Returns <c>true</c> if the channel is successfully registered; otherwise, <c>false</c> if a
    ///     channel with the same name already exists.
    /// </returns>
    bool Register(string channelName, string? topic = default);

    /// <summary>
    ///     Unregisters the channel with the specified channel name and optional topic.
    /// </summary>
    /// <param name="channelName">
    ///     The name of the channel to unregister.
    /// </param>
    /// <returns>
    ///     Returns <c>true</c> if the channel is successfully unregistered; otherwise, <c>false</c> if
    ///     no channel with the specified name exists.
    /// </returns>
    bool Unregister(string channelName);

    /// <summary>
    ///     Publishes a message to the message bus.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the message to subscribe to. Must implement <see cref="IMessage"/>.
    /// </typeparam>
    /// <param name="message">
    ///     The message to be published.
    /// </param>
    /// <param name="topic">
    ///     An optional topic used to filter which channels are meant to receive the message. If not provided,
    ///     the default topic will be used.
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
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no channels with the specified topic are registered before attempting to publish a message.
    /// </exception>
    ValueTask PublishAsync<T>(T message, string? topic = default, CancellationToken cancellationToken = default) where T : class, IMessage;

    /// <summary>
    ///     Subscribes to messages of type <typeparamref name="T"/> from the message bus.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the message to subscribe to. Must implement <see cref="IMessage"/>.
    /// </typeparam>
    /// <param name="channelName">
    ///     The name of the channel to subscribe to.
    /// </param>
    /// <param name="topic">
    ///     An optional topic to filter messages. If not provided, the default topic will be used.
    /// </param>
    /// <param name="cancellationToken">
    ///     Propagates notification that operations should be cancelled.
    /// </param>
    /// <returns>
    ///     This method is used to receive messages of the specified type from the bus. If no messages
    ///     of the type are currently available, it will wait until one becomes available. Returns <c>null</c>
    ///     if the topic doesn't match, or when the message couldn't be casted to type of <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown when no channel with the specified name is found.
    /// </exception>
    /// <exception cref="ChannelClosedException">
    ///     Thrown if the channel is closed before a message can be received.
    /// </exception>
    ValueTask<T?> SubscribeAsync<T>(string channelName, string? topic = default, CancellationToken cancellationToken = default) where T : class, IMessage;
}
