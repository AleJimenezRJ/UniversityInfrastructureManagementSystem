using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Blazor.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving the roles assigned to a user by their user ID.
/// </summary>
public static class GetRolesByUserIdHandler
{
    /// <summary>
    /// Handles the request to get roles for a specific user.
    /// </summary>
    /// <param name="request">The request containing the user ID.</param>
    /// <param name="userRoleService">The service for user-role operations.</param>
    /// <returns>
    /// An HTTP result containing either:
    /// - Ok with the roles found for the user,
    /// - NotFound if no roles are found,
    /// - BadRequest if the request is invalid.
    /// </returns>
    public static async Task<Results<
        Ok<GetRolesByUserIdResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [AsParameters] GetRolesByUserIdRequest request,
        [FromServices] IUserRoleService userRoleService)
    {
        var errorMessages = new List<ValidationError>();

        if (request.UserId <= 0)
        {
            errorMessages.Add(new ValidationError("UserId", "UserId must be a positive integer."));
            return TypedResults.BadRequest(errorMessages);
        }

        var roles = await userRoleService.GetRolesByUserIdAsync(request.UserId);

        if (roles == null || roles.Count == 0)
        {
            return TypedResults.NotFound($"No roles were found for user with ID {request.UserId}.");
        }

        var dtoList = roles.Select(RoleDtoMapper.ToDto).ToList();
        return TypedResults.Ok(new GetRolesByUserIdResponse(dtoList));
    }
}

