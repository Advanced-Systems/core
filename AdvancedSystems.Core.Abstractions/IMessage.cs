using System;

using Mediator;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Represents a message in the system, which includes an identifier and is used for communication purposes.
/// </summary>
/// <remarks>
///     This interface extends <see cref="INotification"/> to integrate with notification-based systems.
///     Implementations of this interface include a unique identifier to distinguish different messages.
/// </remarks>
public interface IMessage : INotification
{
    /// <summary>
    ///     Gets the unique identifier of the message.
    /// </summary>
    /// <value>
    ///     A <see cref="Guid"/> that uniquely identifies the message.
    ///     This property is intended to provide a unique reference for each message instance.
    /// </value>
    Guid Id { get; init; }
}
