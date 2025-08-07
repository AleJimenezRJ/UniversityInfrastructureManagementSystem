using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.UniversityManagement;

/// <summary>
/// Provides extension methods to map endpoints related to the building module.
/// </summary>
public static class BuildingEndpoints
{
    /// <summary>
    /// Maps endpoints related to building operations to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This method registers the following endpoints for building operations: <list type="bullet">
    /// <item> <description><c>POST /add-building</c>: Adds a new building.</description> </item> <item>
    /// <description><c>GET /list-buildings</c>: Retrieves a list of all buildings.</description> </item> <item>
    /// <description><c>PUT /update-building/{buildingId}</c>: Updates an existing building by its ID.</description>
    /// </item> <item> <description><c>GET /list-building/{buildingId}</c>: Retrieves details of a specific building by
    /// its ID.</description> </item> <item> <description><c>GET /list-paginated-buildings</c>: Retrieves a paginated
    /// list of buildings.</description> </item> <item> <description><c>DELETE /delete-building/{buildingId}</c>:
    /// Deletes a building by its ID.</description> </item> </list> Each endpoint is tagged with "Buildings" and
    /// includes OpenAPI metadata for documentation purposes.</remarks>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to which the building endpoints will be mapped.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> with the building endpoints mapped.</returns>
    public static IEndpointRouteBuilder MapBuildingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/add-building", PostBuildingHandler.HandleAsync)
            .WithName("AddBuilding")
            .WithTags("Buildings")
            .RequireAuthorization("Create Buildings")
            .WithOpenApi();

        builder.MapGet("/list-buildings", GetBuildingHandler.HandleAsync)
            .WithName("GetBuildingsInformation")
            .WithTags("Buildings")
            .RequireAuthorization("List Buildings")
            .WithOpenApi();

        builder.MapPut("/update-building/{buildingId}", PutBuildingHandler.HandleAsync)
            .WithName("PutBuilding")
            .WithTags("Buildings")
            .RequireAuthorization("Edit Buildings")
            .WithOpenApi();

        builder.MapGet("/list-building/{buildingId}", GetBuildingByIdHandler.HandleAsync)
            .WithName("GetBuildingById")
            .WithTags("Buildings")
            .RequireAuthorization("View Specific Building")
            .WithOpenApi();

        builder.MapGet("list-paginated-buildings", GetBuildingPaginatedListHandler.HandleAsync)
           .WithName("GetBuildingPaginatedList")
           .WithTags("Buildings")
           .RequireAuthorization("List Buildings")
           .WithOpenApi();

        builder.MapDelete("/delete-building/{buildingId}", DeleteBuildingHandler.HandleAsync)
            .WithName("DeleteBuilding")
            .WithTags("Buildings")
            .RequireAuthorization("Delete Buildings")
            .WithOpenApi();

        return builder;
    }
}