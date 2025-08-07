using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.Spaces;

/// <summary>
/// Provides extension methods to map endpoints related to floors.
/// </summary>
public static class FloorEndpoints
{
    /// <summary>
    /// Maps the endpoints for floor operations.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IEndpointRouteBuilder"/> used to define the routes.
    /// </param>
    /// <returns>
    /// The same <see cref="IEndpointRouteBuilder"/> instance to allow method chaining.
    /// </returns>
    public static IEndpointRouteBuilder MapFloorEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("buildings/{buildingId}/floors", GetFloorListHandler.HandleAsync)
            .WithName("GetFloorList")
            .WithTags("Floors")
            .RequireAuthorization("List Floors")
            .WithOpenApi();

        builder.MapGet("/buildings/{buildingId}/floors/paginated", GetFloorPaginatedListHandler.HandleAsync)
            .WithName("GetFloorPaginatedList")
            .WithTags("Floors")
            .RequireAuthorization("List Floors")
            .WithOpenApi();

        builder.MapDelete("/floors/{floorId}", DeleteFloorHandler.HandleAsync)
            .WithName("DeleteFloor")
            .WithTags("Floors")
            .RequireAuthorization("Delete Floors")
            .WithOpenApi();

        builder.MapPost("/buildings/{buildingId}/floors", PostFloorHandler.HandleAsync)
            .WithName("CreateFloor")
            .WithTags("Floors")
            .RequireAuthorization("Create Floors")
            .WithOpenApi();

        return builder;
    }
}
