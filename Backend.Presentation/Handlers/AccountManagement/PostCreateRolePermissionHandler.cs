using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

public static class PostCreateRolePermissionHandler
{
    /// <summary>
    /// Assigns a role to a permission and returns the result.
    /// </summary>
    /// <param name="request">The user-role request object.</param>
    /// <param name="userRoleService">Injected role-permission service.</param>
    /// <returns>
    /// A result containing:
    /// <list type="bullet">
    /// <item><see cref="Ok{T}"/> with <see cref="PostCreateRolePermissionResponse"/> if successful.</item>
    /// <item><see cref="Conflict"/> if the permission was already assigned.</item>
    /// <item><see cref="BadRequest{T}"/> if validation or domain error occurs.</item>
    /// </list>
    /// </returns>
    public static async Task<Results<
        Ok<PostCreateRolePermissionResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [FromBody] PostCreateRolePermissionRequest request,
        [FromServices] IRolePermissionService rolePermissionService)
    {
        var errorMessages = new List<ValidationError>();

        if (request.RolePermission.PermId <= 0)
            errorMessages.Add(new ValidationError("PermId","PermId must be a positive number."));

        if (request.RolePermission.RoleId <= 0)
            errorMessages.Add(new ValidationError("RoleId","RoleId must be a positive number."));

        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        try
        {
            var success = await rolePermissionService.AssignPermissionToRoleAsync(request.RolePermission.RoleId, request.RolePermission.PermId);

            if (!success)
                return TypedResults.Conflict("");

            var response = new PostCreateRolePermissionResponse(
                new RolePermissionDto(request.RolePermission.RoleId, request.RolePermission.PermId),
                "Permission successfully assigned to role."
            );

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
