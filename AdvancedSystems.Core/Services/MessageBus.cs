using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Internals;

using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Core.Services;

public sealed class MessageBus : IMessageBus
{
    private const string DEFAULT_TOPIC = "default";
    private readonly ConcurrentDictionary<string, Broadcast<IMessage>> _broadcasts;
    private readonly ILogger<MessageBus> _logger;

    public MessageBus(ILogger<MessageBus> logger)
    {
        this._broadcasts = new ConcurrentDictionary<string, Broadcast<IMessage>>();
        this._logger = logger;
    }

    #region Methods

    public bool Register(string channelName, string? topic = default)
    {
        topic ??= DEFAULT_TOPIC;
        this._logger.LogDebug("Registering channel {Name} with topic {topic}.", channelName, topic);
        var options = new UnboundedChannelOptions { AllowSynchronousContinuations = false };
        var channel = new Broadcast<IMessage>
        {
            Name = channelName,
            Channel = Channel.CreateUnbounded<IMessage>(options),
            Topic = topic,
        };
        return this._broadcasts.TryAdd(channelName, channel);
    }

    public bool Unregister(string channelName)
    {
        this._logger.LogDebug("Unregistering channel {Name}.", channelName);
        bool success = this._broadcasts.TryRemove(channelName, out Broadcast<IMessage>? broadcast);
        broadcast?.Channel.Writer.TryComplete();
        return success;
    }

    public async ValueTask PublishAsync<T>(T message, string? topic = default, CancellationToken cancellationToken = default) where T : class, IMessage
    {
        if (this._broadcasts.IsEmpty) throw new InvalidOperationException("There are no registered channels on this message bus.");

        topic ??= DEFAULT_TOPIC;
        var snapshot = this._broadcasts.Values
            .Where(x => string.Equals(x.Topic, topic))
            .ToList();

        if (snapshot.Count == 0) throw new InvalidOperationException($"Unknown topic ('{topic}').");

        foreach(var broadcast in snapshot)
        {
            this._logger.LogDebug("Publishing {Message} to channel {Channel}.", message, broadcast.Name);
            await broadcast.Channel.Writer.WriteAsync(message, cancellationToken);
        }
    }

    public async ValueTask<T?> SubscribeAsync<T>(string channelName, string? topic = default, CancellationToken cancellationToken = default) where T: class, IMessage
    {
        if (!this._broadcasts.TryGetValue(channelName, out Broadcast<IMessage>? broadcast) || broadcast is null)
        {
            throw new KeyNotFoundException($"Channel {channelName} not registered.");
        }

        topic ??= DEFAULT_TOPIC;

        if (!string.Equals(broadcast.Topic, topic, StringComparison.Ordinal)) return null;

        this._logger.LogDebug("Listening to channel {Channel}.", broadcast.Name);

        while (await broadcast.Channel.Reader.WaitToReadAsync(cancellationToken))
        {
            return await broadcast.Channel.Reader.ReadAsync(cancellationToken) as T;
        }

        throw new ChannelClosedException($"Channel {broadcast.Name} has been closed.");
    }

    public ValueTask DisposeAsync()
    {
        this._logger.LogTrace("Disposing message bus.");
        foreach (var broadcast in this._broadcasts.Values) broadcast.Channel.Writer.TryComplete();
        this._broadcasts.Clear();
        return ValueTask.CompletedTask;
    }

    #endregion
}
