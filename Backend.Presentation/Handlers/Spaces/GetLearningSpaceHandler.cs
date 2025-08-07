using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

public static class GetLearningSpaceHandler
{
    /// <summary>
    /// Handles the request to retrieve a learning space by its name.
    /// </summary>
    /// <param name="learningSpaceServices">The service to interact with learning spaces.</param>
    /// <param name="routelearningSpaceId">The unique identifier of the learning space to retrieve.</param>
    /// <returns>
    /// An <see cref="IResult"/> containing the learning space, a bad request in case of invalid data
    /// or a not found response.
    /// </returns>
    public static async Task<Results<Ok<GetLearningSpaceResponse>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [FromRoute(Name = "learningSpaceId")] int routeLearningSpaceId)
    {
        var errors = new List<ValidationError>();

        // Validates LearningId
        if (!Id.TryCreate(routeLearningSpaceId, out var learningSpaceId))
            errors.Add(new ValidationError("LearningSpaceId", "Invalid learning space id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            // Attempt to retrieve the learning space
            var learningSpace = await learningSpaceServices.GetLearningSpaceAsync(routeLearningSpaceId);
            var response = new GetLearningSpaceResponse(LearningSpaceDtoMapper.ToDto(learningSpace!));
            return TypedResults.Ok(response);
        }
        catch (NotFoundException ex)
        {
            // Return NotFound if the learning space does not exist
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            // Return Conflict if there is a concurrency issue
            return TypedResults.Conflict(ex.Message);
        }
    }
}

