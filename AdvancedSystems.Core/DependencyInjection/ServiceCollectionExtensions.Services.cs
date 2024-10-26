using System.Diagnostics.CodeAnalysis;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedSystems.Core.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the default implementation of <seealso cref="ICachingService"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    /// <remarks>
    ///     To use this service, you must configure an <seealso cref="IDistributedCache"/> provider separately.
    /// </remarks>
    public static IServiceCollection AddCachingService(this IServiceCollection services)
    {
        services.AddSerializationService();
        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());
        return services;
    }

    /// <summary>
    ///     Adds the default implementation of <seealso cref="ICompressionService"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    public static IServiceCollection AddCompressionService(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Scoped<ICompressionService, CompressionService>());
        return services;
    }

    /// <summary>
    ///     Adds the default implementation of <seealso cref="IMessageBus"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    [Experimental("Preview008")]
    public static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Singleton<IMessageBus, MessageBus>());
        return services;
    }

    /// <summary>
    ///     Adds the default implementation of <seealso cref="ISerializationService"/> to <paramref name="services"/>.
    /// </summary>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    public static IServiceCollection AddSerializationService(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Scoped<ISerializationService, SerializationService>());
        return services;
    }
}
