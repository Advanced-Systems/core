using System.Diagnostics.CodeAnalysis;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedSystems.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Singleton<ICachingService, CachingService>());
        return services;
    }

    public static IServiceCollection AddCompressionService(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Scoped<ICompressionService, CompressionService>());
        return services;
    }

    [Experimental("Preview008")]
    public static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Singleton<IMessageBus, MessageBus>());
        return services;
    }
}
