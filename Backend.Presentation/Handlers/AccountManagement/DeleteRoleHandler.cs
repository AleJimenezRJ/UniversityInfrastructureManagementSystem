using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the deletion of Role associations in the system.
/// </summary>
public static class DeleteRoleHandler
{
    /// <summary>
    /// Handles the HTTP request to delete a role from the system.
    /// </summary>
    /// <param name="request">The request containing the ID of the role to delete.</param>
    /// <param name="RoleService">The service responsible for role management operations.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.  
    /// The result is a <see cref="Results{T1, T2, T3}"/> which can be:
    /// <list type="bullet">
    /// <item><description><see cref="Ok{T}"/> with <see cref="DeleteRoleResponse"/> if the role was deleted successfully.</description></item>
    /// <item> <description><see cref="NotFound{T}"/> if the role with the specified ID does not exist.</description></item>
    /// <item> <description><see cref="Conflict{T}"/> if there is a domain exception during the deletion process.</description></item>
    /// <item> <description><see cref="BadRequest{T}"/> with a list of validation errors if the request is invalid.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<
        Ok<DeleteRoleResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
            [AsParameters] DeleteRoleRequest request,
        [FromServices] IRoleService RoleService)
    {
        var errors = new List<ValidationError>();

        if (request.Id <= 0)
            errors.Add(new ValidationError("Id", "Role Id must be a positive integer."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            await RoleService.DeleteRoleAsync(request.Id);

            return TypedResults.Ok(
                new DeleteRoleResponse($"The role with Id: {request.Id} has been deleted successfully.")
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
