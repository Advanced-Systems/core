using System;

namespace AdvancedSystems.Core.Common;

/// <summary>
///     Utility class for capturing additional structured data to enhance the debugging experience.
///     Exception filters run where the exception is thrown, not where the exception is caught, which
///     makes it possible to capture contextual data during logging.
/// </summary>
/// <remarks>
///     The functions defined here don't work well with <c>async</c> code, because <c>async</c> will
///     cause exceptions to be caught and then re-thrown at the point of the <c>await</c>. So, the
///     exception filter runs at the point of the <c>await</c> instead of where the exception was
///     originally thrown.
/// </remarks>
public static class ExceptionFilter
{
    /// <summary>
    ///     Applies side effects to exception filters when the <c>catch</c> block handles the exception.
    /// </summary>
    /// <param name="action">The side effect to run in the exception filter.</param>
    /// <returns>Returns <c>true</c>.</returns>
    public static bool True(Action action)
    {
        action();
        return true;
    }

    /// <summary>
    ///     Applies side effects to exception filters when the <c>catch</c> block rethrows the exception.
    /// </summary>
    /// <param name="action">The side effect to run in the exception filter.</param>
    /// <returns>Returns <c>false</c>.</returns>
    public static bool False(Action action)
    {
        action();
        return false;
    }
}