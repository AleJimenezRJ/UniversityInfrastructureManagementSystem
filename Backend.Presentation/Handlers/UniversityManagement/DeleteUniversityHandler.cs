using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Provides a handler for deleting a university.
/// </summary>
public static class DeleteUniversityHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to delete university by name.
    /// </summary>
    /// <returns>
    /// A result indicating success or failure (NotFound with an error message).
    /// </returns>
    public static async Task<Results<Ok<string>, NotFound<DeleteUniversityResponse>>> HandleAsync(
         [FromServices] IUniversityServices universityServices,
         [FromRoute] string UniversityName)
    {
        if (string.IsNullOrEmpty(UniversityName))
        {
            return TypedResults.NotFound(
                new DeleteUniversityResponse("University name cannot be null or empty.")
            );
        }

        // Attempt to delete the university using the service.
        var result = await universityServices.DeleteUniversityAsync(UniversityName);

        // Return NotFound if the deletion fails.
        if (!result)
            return TypedResults.NotFound(
                new DeleteUniversityResponse($"Error deleting university with name {UniversityName}. Please check if the name is correct.")
            );

        // Return OK result.
        return TypedResults.Ok($"The university {UniversityName} has been deleted from the system successfully.");
    }
}
