using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.UniversityManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation;

/// <summary>
/// This class contains extension methods for mapping endpoints in the presentation layer.
/// </summary>
public static class EndpointMappings
{
    /// <summary>
    /// Maps the endpoints for the presentation layer.
    /// </summary>
    /// <param name="builder">The endpoint route builder used to define routes.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> instance to allow method chaining.</returns>
    public static IEndpointRouteBuilder MapPresentationLayerEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapBuildingEndpoints();
        builder.MapUniversityEndpoints();
        builder.MapAreaEndpoints();
        builder.MapCampusEndpoints();
        builder.MapFloorEndpoints();
        builder.MapLearningSpaceEndpoints();
        builder.MapProjectorEndpoints();
        builder.MapWhiteboardEndpoints();
        builder.MapPersonEndpoints();
        builder.MapLearningComponentEndpoints();
        builder.MapLearningComponentAuditEndpoints();
        builder.MapUserEndpoints();
        builder.MapUserAuditEndpoints();
        builder.MapUserRoleEndpoints();
        builder.MapRoleEndpoints();
        builder.MapPermissionEndpoints();
        builder.MapStudentEndpoints();
        builder.MapStaffEndpoints();
        builder.MapBuildingLogsEndpoints();
        builder.MapLearningSpaceLogsEndpoints();

        return builder;
    }
}