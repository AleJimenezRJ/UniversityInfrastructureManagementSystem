using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure;

public static class DependencyInjection
{

    /// <summary>
    /// Registers infrastructure layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
    public static IServiceCollection AddInfrastructureLayerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient(serviceProvider =>
        {
            // API requires authentication
            var authProvider = new BaseBearerTokenAuthenticationProvider(
                new KiotaAccessTokenProvider(
                    serviceProvider.GetRequiredService<IHttpContextAccessor>()));
            // Create request adapter using the HttpClient-based implementation
            // Workaround taken from https://github.com/microsoft/kiota-dotnet/issues/481
            var httpClient = KiotaClientFactory.Create(finalHandler: new HttpClientHandler { AllowAutoRedirect = false });
            var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
            // Set Base URL.
            adapter.BaseUrl = configuration["ApiBaseUrl"];

            // Create the API client
            return new ApiClient(adapter);
        });


        // Repositories for user entities and user role management
        services.AddTransient<IUserWithPersonRepository, KiotaUsersRepository>();
        // Registers the service for user role management.
        services.AddTransient<IUserRoleService, KiotaUserRoleRepository>();
        // Repositories for university management entities
        services.AddTransient<IBuildingsRepository, KiotaBuildingsRepository>();
        // Repository for the log of the building
        services.AddTransient<IBuildingLogRepository, KiotaBuildingLogRepository>();
        // Repositories for area entities
        services.AddTransient<IAreaRepository, KiotaAreaRepository>();
        // Repositories for campus entities
        services.AddTransient<ICampusRepository, KiotaCampusRepository>();
        // Repositories for university entities
        services.AddTransient<IUniversityRepository, KiotaUniversityRepository>();
        // Repositories for components management entities
        services.AddTransient<ILearningComponentRepository, KiotaLearningComponentRepository>();
        // Repositories for projector entities
        services.AddTransient<IProjectorRepository, KiotaProjectorRepository>();
        // Repositories for whiteboard entities
        services.AddTransient<IWhiteboardRepository, KiotaWhiteboardRepository>();
        // Repositories for projector requests
        services.AddTransient<ILearningComponentRequestBuilder, ProjectorRequestBuilder>();
        // Repositories for whiteboard requests
        services.AddTransient<ILearningComponentRequestBuilder, WhiteboardRequestBuilder>();
        // Registers for LearningComponentAuditRepository requests
        services.AddScoped<ILearningComponentAuditRepository, KiotaLearningComponentAuditRepository>();
        // Repositories for space entities
        services.AddTransient <ILearningSpaceRepository, KiotaLearningSpaceRepository>();
        // Repositories for floor entities
        services.AddTransient<IFloorRepository, KiotaFloorRepository>();
        // Repositories for role management entities
        services.AddScoped<IRoleRepository, KiotaRoleRepository>();
        // Registers the IPermissionRepository to be used whenever IPermissionRepository is requested.
        services.AddScoped<IRolePermissionRepository, KiotaRolePermissionRepository>();
        // Registers the IUserAuditRepository to be used whenever IUserAuditRepository is requested.
        services.AddScoped<IUserAuditRepository, KiotaUserAuditRepository>();
        // Registers the ILearningSpaceLogRepository to be used whenever ILearningSpaceLogRepository is requested.
        services.AddScoped<ILearningSpaceLogRepository, KiotaLearningSpaceLogRepository>();
        // Registers the UserNavigationContext service to manage user navigation state.
        services.AddScoped<UserNavigationContext>();
        return services;
    }
}