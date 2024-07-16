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
}
