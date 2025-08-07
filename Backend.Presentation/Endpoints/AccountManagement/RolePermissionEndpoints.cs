using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to role permission management.
/// </summary>
internal static class RolePermissionEndpoints
{
    /// <summary>
    /// Maps the role endpoints to the specified route builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapRolePermissionEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapPost("/role-permission/assign", PostCreateRolePermissionHandler.HandleAsync)
            .WithName("PostCreateRolePermission")
            .WithTags("Role Permission Manager")
            .RequireAuthorization("Create Users")
            .WithOpenApi();

        builder.MapDelete("/role-permission/remove/{roleId:int}/{permId:int}", DeleteRolePermissionHandler.HandleAsync)
            .WithName("DeleteRolePermission")
            .WithTags("Role Permission Manager")
            .RequireAuthorization("Delete Users")
            .WithOpenApi();

        return builder;
    }
}
