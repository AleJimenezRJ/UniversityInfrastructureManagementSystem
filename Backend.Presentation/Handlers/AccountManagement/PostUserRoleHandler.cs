using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;


/// <summary>
/// Handles the assignment of a role to a user.
/// </summary>
public static class PostUserRoleHandler
{
    /// <summary>
    /// Assigns a role to a user and returns the result.
    /// </summary>
    /// <param name="request">The user-role request object.</param>
    /// <param name="userRoleService">Injected user-role service.</param>
    /// <returns>
    /// A result containing:
    /// <list type="bullet">
    /// <item><see cref="Ok{T}"/> with <see cref="PostUserRoleResponse"/> if successful.</item>
    /// <item><see cref="Conflict"/> if the role was already assigned.</item>
    /// <item><see cref="BadRequest{T}"/> if validation or domain error occurs.</item>
    /// </list>
    /// </returns>
    public static async Task<Results<
    Ok<PostUserRoleResponse>,
    NotFound<string>,
    Conflict<string>,
    BadRequest<List<ValidationError>>>> HandleAsync(
    [FromBody] PostUserRoleRequest request,
    [FromServices] IUserRoleService userRoleService)
    {
        var errorMessages = new List<ValidationError>();

        if (request.UserRole.UserId <= 0)
            errorMessages.Add(new ValidationError("UserId","UserId must be a positive number."));

        if (request.UserRole.RoleId <= 0)
            errorMessages.Add(new ValidationError("RoleId", "RoleId must be a positive number."));

        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        try
        {
            var success = await userRoleService.AssignRoleAsync(request.UserRole.UserId, request.UserRole.RoleId);

            if (!success)
                return TypedResults.Conflict("User already has this role assigned.");

            var response = new PostUserRoleResponse(
                new UserRoleDto(request.UserRole.UserId, request.UserRole.RoleId),
                "Role successfully assigned to user."
            );

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
