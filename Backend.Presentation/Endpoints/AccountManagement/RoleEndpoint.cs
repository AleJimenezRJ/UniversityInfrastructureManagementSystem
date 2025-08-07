using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to role management.
/// </summary>
internal static class RoleEndpoints
{
    /// <summary>
    /// Maps the role endpoints to the specified route builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapGet("/roles/{roleId}/permissions", GetPermissionsByRoleIdHandler.HandleAsync)
            .WithName("GetPermissionsByRoleId")
            .WithTags("Role Permissions")
            .Produces<GetPermissionsByRoleIdResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapGet("/role/list", GetAllRolesHandler.HandleAsync)
            .WithName("GetAllRoles")
            .WithTags("Role Management")
            .Produces<GetAllRolesResponse>(StatusCodes.Status200OK)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapDelete("/role/delete/{Id:int}", DeleteRoleHandler.HandleAsync)
            .WithName("DeleteRole")
            .WithTags("Role Management")
            .RequireAuthorization("Delete Users")
            .WithOpenApi();

        return builder;
    }
}
