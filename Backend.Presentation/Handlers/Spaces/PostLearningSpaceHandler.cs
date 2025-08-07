using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Handles the HTTP POST request to create a new learning space.
/// </summary>
public static class PostLearningSpaceHandler
{
    /// <summary>
    /// Handles the creation of a learning space using the provided request data.
    /// </summary>
    /// <param name="learningSpaceServices">Service that provides business logic for managing learning spaces.</param>
    /// <param name="request">The request containing the learning space data.</param>
    /// <param name="routeFloorId">The unique identifier of the floor in the route.</param>
    /// <returns>
    /// A result that can be:
    /// - <see cref="Ok{T}"/> with a success message if the creation was successful.
    /// - <see cref="NotFound{T}"/> if the specified floor does not exist."/>
    /// - <see cref="Conflict{T}"/> with an error message if the learning couldn't be created.
    /// - <see cref="BadRequest{T}"/> with validation errors if the request data is invalid.
    /// - <see cref="ProblemHttpResult"/> if an unexpected error occurs.

    /// </returns>
    public static async Task<Results<Ok<PostLearningSpaceResponse>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>, ProblemHttpResult>> HandleAsync(
         [FromServices] ILearningSpaceServices learningSpaceServices,
         [FromBody] PostLearningSpaceRequest request,
         [FromRoute(Name = "floorId")] int routeFloorId)
    {
        if (request == null)
            return TypedResults.BadRequest(new List<ValidationError> { new ValidationError("Request", "Request body is required.") });

        var errors = new List<ValidationError>();

        // Validates FloorId
        if (!Id.TryCreate(routeFloorId, out var floorId))
            errors.Add(new ValidationError("FloorId", "Invalid floor Id format."));

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
            // Try to create the learning space
            await learningSpaceServices.CreateLearningSpaceAsync(
                routeFloorId,
                learningSpaceEntity!);
            return TypedResults.Ok(new PostLearningSpaceResponse(request.LearningSpace, "The learning space was created succesfully."));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
