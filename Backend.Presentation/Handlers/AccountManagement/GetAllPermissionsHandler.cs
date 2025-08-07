using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all registered permissions.
/// </summary>
public static class GetAllPermissionsHandler
{
    /// <summary>
    /// Handles the request to retrieve all permissions.
    /// </summary>
    /// <param name="permissionService">The service used to retrieve permission data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains:
    /// An <see cref="Ok{T}"/> result with a <see cref="GetAllPermissionsResponse"/>, containing a list (possibly empty) of permissions.
    /// </returns>
    public static async Task<Results<
        Ok<GetAllPermissionsResponse>,
        NotFound<string>,
        Conflict<string>>> HandleAsync(
       [FromServices] IPermissionService permissionService)
    {
        try
        {
            var permissions = await permissionService.GetAllPermissionsAsync();

            if (permissions.Count == 0)
                return TypedResults.NotFound("There are no registered permissions.");

            var dtoList = PermissionDtoMapper.ToDtoList(permissions);

            var response = new GetAllPermissionsResponse
            {
                Permissions = dtoList
            };

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}

