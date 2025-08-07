using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

public static class DeleteUserWithPersonHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to remove a user with person by their IDs.
    /// </summary>
    /// <param name="userWithPersonService">The service used to manage user-person-related operations.</param>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>
    /// Returns 200 OK with a success message if the user was deleted,  
    /// or 404 Not Found with an error message if the user was not found or could not be deleted.
    /// </returns>
    public static async Task<Results<
        Ok<string>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
            [AsParameters] DeleteUserWithPersonRequest request,
        [FromServices] IUserWithPersonService userWithPersonService)
    {
        var errors = new List<ValidationError>();

        if (request.UserId <= 0)
            errors.Add(new ValidationError("UserId", "UserId must be a positive integer."));

        if (request.PersonId <= 0)
            errors.Add(new ValidationError("PersonId", "PersonId must be a positive integer."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            await userWithPersonService.DeleteUserWithPersonAsync(request.UserId, request.PersonId);
            return TypedResults.Ok($"User with ID {request.UserId} was successfully deleted.");
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

