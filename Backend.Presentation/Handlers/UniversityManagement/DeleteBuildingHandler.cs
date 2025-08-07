using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Provides a handler for deleting a building.
/// </summary>
public static class DeleteBuildingHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to delete building by id.
    /// </summary>
    /// <returns>
    /// A result indicating success or failure (NotFound with an error message).
    /// </returns>
    public static async Task<Results<Ok<string>, NotFound<DeleteBuildingResponse>, Conflict<string>>> HandleAsync(
         [FromServices] IBuildingsServices buildingsServices,
         [FromRoute] int BuildingId)
    {
        // Create a request object with the provided ID.
        var request = new DeleteBuildingRequest(BuildingId);

        // Attempt to delete the building using the service.
        try
        {
            var result = await buildingsServices.DeleteBuildingAsync(request.BuildingId);

            // Return NotFound if the deletion fails.
            if (!result)
            {
                return TypedResults.NotFound(
                    new DeleteBuildingResponse($"Error deleting building with id {BuildingId}. Please check if the id is correct."));
            }

            // Return OK result.
            return TypedResults.Ok($"The building with ID {BuildingId} has been deleted from the system successfully.");
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Conflict($"The building with ID {BuildingId} cannot be deleted due to a concurrency conflict: {ex.Message}");
        }
    }
}
