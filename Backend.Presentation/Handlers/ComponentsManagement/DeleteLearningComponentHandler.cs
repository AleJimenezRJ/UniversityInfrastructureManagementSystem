using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Provides functionality for handling the deletion of a learning component in the system.
/// </summary>
public static class DeleteLearningComponentHandler
{
    /// <summary>
    /// Handles the deletion of a learning component with the specified ID, returning appropriate HTTP results.
    /// </summary>
    /// <param name="componentService">The service used to interact with learning components.</param>
    /// <param name="id">The identifier of the learning component to be deleted.</param>
    /// <returns>
    /// A <see cref="Results"/> object containing a <see cref="NoContent"/> result if the deletion is successful,
    /// or a <see cref="NotFound{DeleteLearningComponentResponse}"/> result if the component with the specified ID does not exist.
    /// </returns>
    public static async Task<Results<NoContent, NotFound<DeleteLearningComponentResponse>>> HandleAsync(
        [FromServices] ILearningComponentServices componentService,
        [FromRoute(Name = "learningComponentId")] int id)
    {
        // Create a request object with the provided ID.
        var request = new DeleteLearningComponentRequest(id);

        // Attempt to delete the whiteboard using the service.
        var result = await componentService.DeleteLearningComponentAsync(request.Id);

        // Return NotFound if the deletion fails.
        if (!result)
            return TypedResults.NotFound(
                new DeleteLearningComponentResponse($"Error deleting whiteboard with id {id}. Please check if the id is correct.")
            );

        // Return NoContent if the deletion is successful.
        return TypedResults.NoContent();
    }
}