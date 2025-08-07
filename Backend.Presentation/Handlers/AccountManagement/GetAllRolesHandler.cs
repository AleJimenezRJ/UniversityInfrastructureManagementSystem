using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Blazor.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all registered roles.
/// </summary>
public static class GetAllRolesHandler
{
    /// <summary>
    /// Handles the request to retrieve all roles.
    /// </summary>
    /// <param name="roleService">The service used to retrieve role data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains:
    /// An <see cref="Ok{T}"/> result with a <see cref="GetAllRolesResponse"/>, containing a list (possibly empty) of roles.
    /// </returns>
    public static async Task<Results<
       Ok<GetAllRolesResponse>,
       NotFound<string>,
       Conflict<string>>> HandleAsync(
       [FromServices] IRoleService roleService)
    {
        try
        {
            var roles = await roleService.GetAllRolesAsync();

            if (roles.Count == 0)
                return TypedResults.NotFound("There are no registered roles.");

            var dtoList = RoleDtoMapper.ToDtoList(roles);

            var response = new GetAllRolesResponse
            {
                Roles = dtoList
            };

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}

