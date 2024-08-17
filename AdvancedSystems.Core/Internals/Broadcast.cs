using System.Threading.Channels;

namespace AdvancedSystems.Core.Internals;

internal class Broadcast<T>
{
    internal required string Name { get; init; }

    internal required Channel<T> Channel { get; init; }

    internal string? Topic { get; init; }
}