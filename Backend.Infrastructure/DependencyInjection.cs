using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure;

/// <summary>
/// Dependency injection configuration for the Infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
    public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the SQL implementations for the IBuildingsRepository interface.
        services.AddTransient<IBuildingsRepository, SqlBuildingRepository>();

        // Register the SQL implementations for the IAreaRepository interface.
        services.AddTransient<IAreaRepository, SqlAreaRepository>();

        // Register the SQL implementations for the IUniversityRepository interface.
        services.AddTransient<IUniversityRepository, SqlUniversityRepository>();

        // Register the SQL implementations for the ICampusRepository interface.
        services.AddTransient<ICampusRepository, SqlCampusRepository>();

        // Register the SQL implementations
        services.AddTransient<IFloorRepository, SqlFloorRepository>();

        // Registers the DummyLearningSpaceRepository implementation for the ILearningSpaceRepository interface.
        services.AddTransient<ILearningSpaceRepository, SqlLearningSpaceRepository>();

        // 
        services.AddScoped<ILearningSpaceLogRepository, SqlLearningSpaceLogRepository>();

        // Register the SqlWhiteboard, implementation for IWhiteboardRepository interface
        services.AddTransient<IWhiteboardRepository, SqlWhiteboardRepository>();

        // Register the SqlProjector, implementation for IProjectorRepository interface
        services.AddTransient<IProjectorRepository, SqlProjectorRepository>();

        // Register the SqlLearningComponent, implementation for ILearningComponentRepository interface
        services.AddTransient<ILearningComponentRepository, SqlLearningComponentRepository>();

        // Register the SqlLearningComponentAudit, implementation for ILearningComponentAuditRepository interface
        services.AddTransient<ILearningComponentAuditRepository, SqlLearningComponentAuditRepository>();

        // Registers the SQL implementations for the IPersonRepository interface.
        services.AddScoped<IPersonRepository, SqlPersonRepository>();

        // Registers the SQL implementations for the IStudentRepository interface.
        services.AddScoped<IStudentRepository, SqlStudentRepository>();

        // Registers the SQL implementations for the SqlStaffRepository interface.
        services.AddScoped<IStaffRepository, SqlStaffRepository>();

        // Registers the SQL implementations for the IUserRepositoryInterface
        services.AddScoped<IUserRepository, SqlUserRepository>();

        // Registers the SQL implementations for the IUserRoleRepository interface.
        services.AddScoped<IUserRoleRepository, SqlUserRoleRepository>();

        // Registers the SQL implementations for the IRoleRepository interface.
        services.AddScoped<IRoleRepository, SqlRoleRepository>();

        // Registers the SQL implementations for the IRolePermissionRepository interface.
        services.AddScoped<IRolePermissionRepository, SqlRolePermissionRepository>();

        // Registers the SQL implementations for the IPermissionRepository interface.
        services.AddScoped<IPermissionRepository, SqlPermissionRepository>();

        // Registers the SQL implementations for the IUserWithPersonRepository interface.
        services.AddScoped<IUserWithPersonRepository, SqlUserWithPersonRepository>();

        // Registers the SQL implementations for the IUserAuditService interface.
        services.AddScoped<IUserAuditRepository, SqlUserAuditRepository>();

        // Registers the SQL implementations for the IBuildingLogRepository interface.
        services.AddScoped<IBuildingLogRepository, SqlBuildingLogRepository>(); 

        // Configure the database context
        services.AddDbContext<ThemeParkDataBaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
