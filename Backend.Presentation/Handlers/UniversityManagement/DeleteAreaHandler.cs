using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Provides a handler for deleting an area.
/// </summary>
public static class DeleteAreaHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to delete area by name.
    /// </summary>
    /// <returns>
    /// A result indicating success or failure (NotFound with an error message).
    /// </returns>
    public static async Task<Results<Ok<string>, NotFound<DeleteAreaResponse>, Conflict<string>>> HandleAsync(
         [FromServices] IAreaServices areaServices,
         [FromRoute] string areaName)
    {
        try
        {
            var request = new DeleteAreaRequest(areaName);

            // Attempt to delete the area using the service.
            var result = await areaServices.DeleteAreaAsync(request.area);

            // Return NotFound if the deletion fails.
            if (!result)
            {
                return TypedResults.NotFound(
                    new DeleteAreaResponse($"Error deleting area with name {areaName}. Please check if the name is correct.")
                );
            }

            // Return OK result.
            return TypedResults.Ok($"The area {areaName} has been deleted from the system successfully.");
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Conflict($"The Area with name {areaName} cannot be deleted due to a concurrency conflict: {ex.Message}");
        }
    }
}