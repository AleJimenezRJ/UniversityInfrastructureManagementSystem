using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application;

/// <summary>
/// Provides extension methods for configuring dependency injection in the application layer.
/// </summary>
public static class DependencyInjection
{

    /// <summary>
    /// Registers application layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the services will be added.
    /// </param>
    /// <returns>
    /// The updated <see cref="IServiceCollection"/> with the registered services.
    /// </returns>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        // Registers the BuildingsServices to be used whenever IBuildingsServices is requested.
        services.AddScoped<IBuildingsServices, BuildingsServices>();
        // Registers the BuildingLogServices to be used whenever IBuildingLogServices is requested.
        services.AddScoped<IBuildingLogServices, BuildingLogServices>();
        // Registers the UniversityServices to be used whenever IUniversityServices is requested.
        services.AddScoped<IUniversityServices, UniversityServices>();
        // Registers the AreaServices to be used whenever IAreaServices is requested.
        services.AddScoped<IAreaServices, AreaServices>();
        // Registers the CampusServices to be used whenever ICampusServices is requested.
        services.AddScoped<ICampusServices, CampusServices>();
        // Registers the FloorServices to be used whenever IFloorServices is requested.
        services.AddScoped<IFloorServices, FloorServices>();
        // Registers the LearningSpaceServices to be used whenever ILearningSpaceServices is requested.
        services.AddScoped<ILearningSpaceServices, LearningSpaceServices>();
        // Registers the LearningSpaceLogServices to be used whenever ILearningSpaceLogServices is requested.
        services.AddScoped<ILearningSpaceLogServices, LearningSpaceLogServices>();
        // Registers the LearningComponentServices to be used whenever ILearningComponentServices is requested.
        services.AddScoped<IWhiteboardServices, WhiteboardServices>();
        services.AddScoped<IProjectorServices, ProjectorServices>();
        services.AddScoped<ILearningComponentServices, LearningComponentServices>();
        // Registers the UserRoleService to be used whenever IUserRoleService is requested.
        services.AddScoped<IUserRoleService, UserRoleService>();
        // Registers the UserService to be used whenever IUserService is requested.
        services.AddScoped<IUserService, UserService>();
        // Registers the PersonService to be used whenever IPersonService is requested.
        services.AddScoped<IPersonService, PersonService>();
        // Registers the StudentService to be used whenever IStudentService is requested.
        services.AddScoped<IStudentService, StudentService>();
        // Registers the StaffService to be used whenever IStaffService is requested.
        services.AddScoped<IStaffService, StaffService>();
        // Registers the RoleService to be used whenever IRoleService is requested.
        services.AddScoped<IRoleService, RoleService>();
        // Registers the RolePermissionService to be used whenever IRolePermissionService is requested.
        services.AddScoped<IRolePermissionService, RolePermissionService>();
        // Registers the PermissionService to be used whenever IPermissionService is requested.
        services.AddScoped<IPermissionService, PermissionService>();
        // Registers the UserWithPersonService to be used whenever IUserWithPersonService is requested.
        services.AddScoped<IUserWithPersonService, UserWithPersonService>();
        // Registers the UserAuditService to be used whenever IUserAuditService is requested.
        services.AddScoped<IUserAuditService, UserAuditService>();
        // Registers the LearningComponentAuditServices to be used whenever ILearningComponentAuditServices is requested.
        services.AddScoped<ILearningComponentAuditServices, LearningComponentAuditServices>();
        return services;
    }
}
