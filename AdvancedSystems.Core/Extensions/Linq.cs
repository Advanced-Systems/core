using System.Collections.Generic;
using System.Linq;

namespace AdvancedSystems.Core.Extensions;

public static class Linq
{
    /// <summary>
    ///     Checks whether enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable.</typeparam>
    /// <param name="enumerable">The enumerable to be checked.</param>
    /// <returns> True if enumerable is null or empty, false otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }
}
