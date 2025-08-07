using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.DependencyInjection;

/// <summary>
/// Provides extension methods for registering application and infrastructure services
/// following the Clean Architecture pattern in a Blazor WebAssembly project.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the application and infrastructure layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCleanArchitectureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register application layer services
        services.AddApplicationLayerServices();
        // Register infrastructure layer services with configuration
        services.AddInfrastructureLayerServices(configuration);

        return services;
    }
}
