using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation;

/// <summary>
/// Provides an extension method to configure and add presentation layer services
/// to the dependency injection container for the application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures and registers the services related to the presentation layer
    /// into the application's dependency injection container.
    /// </summary>
    /// <param name="services">Represents the service collection to which the presentation layer services will be added.</param>
    /// <returns>
    /// The modified service collection after adding the presentation layer services.
    /// </returns>
    public static IServiceCollection AddPresentationLayerServices(this IServiceCollection services)
    {
        services.AddScoped<GlobalMapper>();

        return services;
    }

}