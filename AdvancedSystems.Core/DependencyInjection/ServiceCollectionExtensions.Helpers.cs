using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Core.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers and binds <typeparamref name="TOptions"/> to the underlying <paramref name="services"/> collection
    ///     if it has not already been registered, and binds the options to values from the given <paramref name="configurationSection"/>.
    /// </summary>
    /// <typeparam name="TOptions">
    ///     The type of the options to register and configure.
    /// </typeparam>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <param name="configurationSection">
    ///     A section of the application configuration.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    /// <remarks>
    ///     This method performs implicit data annotations on startup for this options instance.
    /// </remarks>
    public static IServiceCollection TryAddOptions<TOptions>(this IServiceCollection services, IConfigurationSection configurationSection) where TOptions : class
    {
        bool hasOptions = services.Any(service => service.ServiceType == typeof(IConfigureOptions<TOptions>));

        if (!hasOptions)
        {
            services.AddOptions<TOptions>()
                .Bind(configurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        return services;
    }

    /// <summary>
    ///     Registers and binds <typeparamref name="TOptions"/> to the underlying <paramref name="services"/> collection
    ///     if it has not already been registered, and binds the options from <paramref name="configureOptions"/>.
    /// </summary>
    /// <typeparam name="TOptions">
    ///     The type of the options to register and configure.
    /// </typeparam>
    /// <param name="services">
    ///     The service collection containing the service.
    /// </param>
    /// <param name="configureOptions">
    ///     An action to configure the options of type <typeparamref name="TOptions"/>.
    /// </param>
    /// <returns>
    ///     The value of <paramref name="services"/>.
    /// </returns>
    /// <remarks>
    ///     This method performs implicit data annotations on startup for this options instance.
    /// </remarks>
    public static IServiceCollection TryAddOptions<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class, new()
    {
        services.TryAddSingleton(_ =>
        {
            var options = new TOptions();
            configureOptions(options);
            ValidateOptions(options);
            return Options.Create(options);
        });

        return services;
    }

    private static void ValidateOptions<TOptions>(TOptions options) where TOptions : class
    {
        var context = new ValidationContext(options);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(options, context, results, true);

        if (!isValid)
        {
            throw new ValidationException($"Validation failed for options: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
        }
    }
}
