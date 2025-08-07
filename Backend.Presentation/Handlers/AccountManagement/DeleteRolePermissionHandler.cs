using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the deletion of Role Permisson associations in the system.
/// </summary>
public static class DeleteRolePermissionHandler
{
    /// <summary>
    /// Processes a request to remove a permission from a role.
    /// </summary>
    /// <param name="request">Contains the role and permission IDs.</param>
    /// <param name="rolePermissionService">Service to manage role-permission operations.</param>
    /// <returns>
    /// A result indicating success (<see cref="Ok{T}"/>),  
    /// not found (<see cref="NotFound{T}"/>),  
    /// or invalid input (<see cref="BadRequest{T}"/>).
    /// </returns>
    public static async Task<Results<
        Ok<DeleteRolePermissionResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
            [AsParameters] DeleteRolePermissionRequest request,
        [FromServices] IRolePermissionService rolePermissionService)
    {
        var errors = new List<ValidationError>();

        if (request.PermId <= 0)
            errors.Add(new ValidationError("PermId", "PermId must be a positive number."));

        if (request.RoleId <= 0)
            errors.Add(new ValidationError("RoleId", "RoleId must be a positive number."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);
        // Attempt to remove the permission from the role
        try
        {
            await rolePermissionService.RemovePermissionFromRoleAsync(request.RoleId, request.PermId);

            return TypedResults.Ok(
                new DeleteRolePermissionResponse($"The role-permission association for RoleId {request.RoleId} and PermId {request.PermId} has been deleted successfully.")
            );
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
