using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Provides a handler for deleting a person by their identity number.
/// </summary>
public static class DeletePersonHandler
{
    /// <summary>
    /// Handles the deletion of a person by their identity number.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <param name="personService"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains:
    /// a <see cref="Ok{T}"/> with a success message if the person is deleted successfully,
    /// a <see cref="NotFound"/> if the person is not found, and
    /// a <see cref="BadRequest{T}"/> with an <see cref="ErrorResponse"/> if the identity number is invalid.
    /// </returns>
    public static async Task<Results<
        Ok<string>,
        NotFound<string>,
        BadRequest<ErrorResponse>>> HandleAsync(
        [FromRoute] string identityNumber,
        [FromServices] IPersonService personService)
    {
        var errorMessages = new List<string>();
        IdentityNumber? identityObj = null;

        try
        {
            identityObj = IdentityNumber.Create(identityNumber);
        }
        catch (ValidationException ex)
        {
            errorMessages.Add(ex.Message);
        }

        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }

        try
        {
            await personService.DeletePersonAsync(identityObj!.Value);
            return TypedResults.Ok("Person has been deleted from the system successfully.");
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound($"Person with identity number '{identityNumber}' was not found.");
        }
        catch (DomainException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(new List<string> { "An unexpected error occurred while deleting the person.", ex.Message }));
        }
    }
}