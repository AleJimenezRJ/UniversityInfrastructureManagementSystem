using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.UniversityManagement;

/// <summary>
/// Provides extension methods for mapping building log-related endpoints to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
/// <remarks>This class contains methods to register API endpoints for managing building logs, such as retrieving
/// building log information. The endpoints are configured with specific route patterns, authorization requirements, and
/// OpenAPI metadata.</remarks>
public static class BuildingLogsEndpoints
{
    /// <summary>
    /// Maps the endpoints related to building logs to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This method registers the following endpoint: <list type="bullet"> <item> <description> A GET
    /// endpoint at <c>/list-building-logs</c> that retrieves building log information.  This endpoint requires the
    /// "ApiAccess" authorization policy and is tagged with "BuildingLogs". </description> </item> </list></remarks>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to which the building logs endpoints will be added.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> with the building logs endpoints mapped.</returns>
    public static IEndpointRouteBuilder MapBuildingLogsEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapGet("/list-building-logs", GetBuildingLogHandler.HandleAsync)
            .WithName("GetBuildingLogs")
            .WithTags("BuildingLogs")
            .Produces<List<BuildingLogDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("List Buildings")
            .WithOpenApi();

        return builder;
    }
}
