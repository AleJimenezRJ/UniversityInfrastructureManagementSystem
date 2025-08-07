using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the deletion of user-role associations in the system.
/// </summary>
public static class DeleteUserRoleHandler
{
    /// <summary>
    /// Processes a request to remove a role from a user.
    /// </summary>
    /// <param name="request">Contains the user and role IDs.</param>
    /// <param name="userRoleService">Service to handle user-role associations.</param>
    /// <returns>
    /// A result indicating success (<see cref="Ok{T}"/>),  
    /// not found (<see cref="NotFound{T}"/>),  
    /// or bad request (<see cref="BadRequest{T}"/>).
    /// </returns>
    public static async Task<Results<
        Ok<DeleteUserRoleResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [AsParameters] DeleteUserRoleRequest request,
        [FromServices] IUserRoleService userRoleService)
    {
        var errorMessages = new List<ValidationError>();

        if (request.UserId <= 0)
            errorMessages.Add(new ValidationError("UserId", "UserId must be a positive integer."));
        if (request.RoleId <= 0)
            errorMessages.Add(new ValidationError("RoleId", "RoleId must be a positive integer."));

        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        try
        {
            await userRoleService.RemoveRoleAsync(request.UserId, request.RoleId);
            return TypedResults.Ok(
            new DeleteUserRoleResponse($"The user-role association for UserId {request.UserId} and RoleId {request.RoleId} has been deleted successfully.")
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
