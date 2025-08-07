using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

public static class DeleteLearningSpaceHandler
{
    /// <summary>
    /// Handles the HTTP DELETE request to delete a learning space by name, building, and floor.
    /// </summary>
    /// <param name="routeLearningSpaceId">Id from route of the learning space</param>
    /// <returns>
    /// Returns one of the following results:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="NoContent"/>: The learning space was successfully deleted.</description>
    /// </item>
    /// <item>
    /// <description><see cref="NotFound{T}"/>: The specified learning space was not found.</description>
    /// </item>
    /// <item>
    /// <description><see cref="BadRequest{T}"/>: The request was invalid due to input validation errors.</description>
    /// </item>
    /// <item>
    /// <description><see cref="Conflict{T}"/>: A conflict occurred during the deletion process.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static async Task<Results<NoContent, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [FromRoute(Name = "learningSpaceId")] int routeLearningSpaceId)
    {
        var errors = new List<ValidationError>();

        // Validates FloorNumber
        if (!Id.TryCreate(routeLearningSpaceId, out var learningSpaceId))
            errors.Add(new ValidationError("LearningSpaceId", "Invalid floor id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            await learningSpaceServices.DeleteLearningSpaceAsync(routeLearningSpaceId);
            // Returns NoContent if the deletion was successful
            return TypedResults.NoContent();
        }
        catch (NotFoundException ex)
        {
            // Returns NotFound if the learning space does not exist
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            // Returns Conflict if a concurrency conflict occurred
            return TypedResults.Conflict(ex.Message);
        }
    }
}
