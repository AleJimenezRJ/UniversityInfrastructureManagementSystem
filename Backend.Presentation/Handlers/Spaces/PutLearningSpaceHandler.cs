using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Handler responsible for processing HTTP PUT requests to update an existing learning space.
/// </summary>
public static class PutLearningSpaceHandler
{
    /// <summary>
    /// Handles the update of a learning space using the provided request data.
    /// </summary>
    /// <param name="learningSpaceServices">The service to interact with learning spaces.</param>
    /// <param name="routeLearningSpaceId">The unique identifier of the learning space to retrieve.</param>
    /// <returns>
    /// A result that can be:
    /// - <see cref="Ok{T}"/> with a success message if the update was successful.
    /// - <see cref="Conflict{T}"/> with an error message if the learning space was not updated.
    /// - <see cref="BadRequest{T}"/> with validation errors if the request data is invalid.
    /// </returns
    public static async Task<Results<Ok<PutLearningSpaceResponse>, Conflict<string>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [FromBody] PutLearningSpaceRequest request,
        [FromRoute(Name = "learningSpaceId")] int routeLearningSpaceId)
    {
        var errors = new List<ValidationError>();

        // Validate LearningSpaceName
        if (!Id.TryCreate(routeLearningSpaceId, out var learningSpaceId))
            errors.Add(new ValidationError("LearningSpaceId", "Invalid learning space Id format."));

        // Validates the entity from the request body
        LearningSpace? learningSpaceEntity = null;
        try
        {
            learningSpaceEntity = LearningSpaceDtoMapper.ToEntity(request.LearningSpace);
        }
        catch (ValidationException exception)
        {
            errors.AddRange(exception.Errors);
        }

        // If there are any validation errors, return them
        if (errors.Count > 0)
        {
            return TypedResults.BadRequest(errors);
        }

        try
        {
            await learningSpaceServices.UpdateLearningSpaceAsync(
                routeLearningSpaceId,
                learningSpaceEntity!);

            return TypedResults.Ok(new PutLearningSpaceResponse(request.LearningSpace, "The learning space was updated successfully"));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
