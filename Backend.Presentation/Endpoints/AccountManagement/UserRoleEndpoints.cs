using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to user management.
/// </summary>
internal static class UserRoleEndpoints
{
    /// <summary>
    /// Maps the user role endpoints to the specified route builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapUserRoleEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/users/{userId}/roles", GetRolesByUserIdHandler.HandleAsync)
             .WithName("GetRolesByUserId")
             .WithTags("Users")
             .RequireAuthorization("View Users")
             .WithOpenApi();

        builder.MapGet("/users/{userId}/roles/{roleId}", GetByUserAndRoleHandler.HandleAsync)
            .WithName("GetByUserAndRole")
            .WithTags("User Role Management")
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapPost("/users/{userId}/roles", PostUserRoleHandler.HandleAsync)
            .WithName("PostUserRole")
            .WithTags("User Role Management")
            .RequireAuthorization("Create Users")
            .WithOpenApi();

        builder.MapDelete("/users/{userId}/roles/{roleId}", DeleteUserRoleHandler.HandleAsync)
            .WithName("DeleteUserRole")
            .WithTags("User Role Management")
            .RequireAuthorization("Delete Users")
            .WithOpenApi();

        return builder;
    }
}
