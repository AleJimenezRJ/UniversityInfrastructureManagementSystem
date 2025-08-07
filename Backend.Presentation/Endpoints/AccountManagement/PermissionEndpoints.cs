using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints for managing permissions.
/// </summary>
public static class PermissionEndpoints
{
    /// <summary>
    /// Maps the permission-related endpoints to the specified route builder.
    /// </summary>
    /// <param name="builder"> The route builder to which the endpoints will be added.</param>
    /// <returns> The updated route builder with the permission endpoints.</returns>
    public static IEndpointRouteBuilder MapPermissionEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapGet("/permission/list", GetAllPermissionsHandler.HandleAsync)
            .WithName("GetAllPermissions")
            .WithTags("Permissions Management")
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapPost("/permission/access_token", PostTokenEnrichmentHandler.HandleAsync)
            .WithName("PostTokenEnrichment")
            .WithTags("Permissions Management")
            .WithOpenApi();


        return builder;
    }
}
