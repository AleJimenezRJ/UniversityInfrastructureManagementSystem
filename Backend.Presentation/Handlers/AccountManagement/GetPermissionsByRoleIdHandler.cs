using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Requests.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving the list of permissions associated with a specific role by its ID.
/// Validates the request, interacts with the role-permission service, and returns the appropriate HTTP response.
/// </summary>
public static class GetPermissionsByRoleIdHandler
{
    /// <summary>
    /// Handles the request to get permissions by role ID.
    /// </summary>
    /// <param name="request">The request containing the role ID.</param>
    /// <param name="rolePermissionService">The service to retrieve permissions for the role.</param>
    /// <returns>
    /// An HTTP result containing either the list of permissions, a not found message, or a validation error.
    /// </returns>
    public static async Task<Results<
        Ok<GetPermissionsByRoleIdResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [AsParameters] GetPermissionsByRoleIdRequest request,
        [FromServices] IRolePermissionService rolePermissionService)
    {
        var errors = new List<ValidationError>();
        if (request.RoleId <= 0)
            errors.Add(new ValidationError("RoleId", "RoleId must be a positive number."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        var permissions = await rolePermissionService.ViewPermissionsByRoleIdAsync(request.RoleId);
        if (permissions is null || permissions.Count == 0)
        {
            return TypedResults.NotFound($"No permissions found for role with ID {request.RoleId}.");
        }

        var dtoList = PermissionDtoMapper.ToDtoList(permissions);
        return TypedResults.Ok(new GetPermissionsByRoleIdResponse(dtoList));
    }
}
