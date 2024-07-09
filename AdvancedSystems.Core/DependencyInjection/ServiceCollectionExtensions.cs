using System;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Options;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedSystems.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, Action<CachingOptions> setupActions)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(setupActions, nameof(setupActions));

        services.AddOptions();
        services.Configure(setupActions);

        // see also: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0#establish-distributed-caching-services
        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());

        throw new NotImplementedException("TODO: Use Distributed Redis Cache in production environment.");
    }

    public static IServiceCollection AddCachingService(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        // Should only be used in single server scenarios as this cache stores items in memory and doesn't
        // expand across multiple machines. For those scenarios it is recommended to use a proper distributed
        // cache that can expand across multiple machines.
        services.AddDistributedMemoryCache();
        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());

        return services;
    }
}
