using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Provides a handler for deleting a user by their Id.
/// </summary>
public static class DeleteUserHandler
{
    /// <summary>
    /// Handles the deletion of a user by their Id.
    /// </summary>
    /// <param name="id">The Id of the user to delete.</param>
    /// <param name="userService">The user service for executing the deletion.</param>
    /// <returns>
    /// Returns one of the following results:
    /// A successful deletion returns <see cref="Ok{T}"/> with a success message.
    /// A <see cref="NotFound{T}"/> result if the user with the specified Id does not exist.
    /// A <see cref="Conflict{T}"/> result if there is a domain exception during deletion.
    /// A <see cref="BadRequest{T}"/> result if the provided Id is invalid or if there are validation errors.
    /// </returns>
    public static async Task<Results<Ok<string>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
     [FromRoute] int id,
     [FromServices] IUserService userService)
    {
        var errors = new List<ValidationError>();

        // ID validation
        if (id <= 0)
            errors.Add(new ValidationError("UserId", "Invalid user id."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            await userService.DeleteUserAsync(id);
            return TypedResults.Ok("User deleted successfully.");
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
