using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Provides a handler for deleting a campus.
/// </summary>
public static class DeleteCampusHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to delete campus by name.
    /// </summary>
    /// <returns>
    /// A result indicating success or failure (NotFound with an error message).
    /// </returns>
    public static async Task<Results<Ok<string>, NotFound<DeleteCampusResponse>>> HandleAsync(
         [FromServices] ICampusServices campusServices,
         [FromRoute] string campusName)
    {
        // Attempt to delete the campus using the service.
        var result = await campusServices.DeleteCampusAsync(campusName);

        // Return NotFound if the deletion fails.
        if (!result)
            return TypedResults.NotFound(
                new DeleteCampusResponse($"Error deleting campus with name {campusName}. Please check if the name is correct.")
            );

        // Return OK result.
        return TypedResults.Ok($"The campus {campusName} has been deleted from the system successfully.");
    }
}
