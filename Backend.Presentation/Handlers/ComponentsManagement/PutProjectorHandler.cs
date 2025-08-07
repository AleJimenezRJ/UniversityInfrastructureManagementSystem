using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handles the update process for a Projector entity.
/// This handler validates the incoming request, including orientation,
/// dimensions, position, and projection area. 
/// Returns appropriate HTTP responses depending on success or failure.
/// </summary>
public static class PutProjectorHandler
{
    /// <summary>
    /// Handles the HTTP PUT request to update an existing projector.
    /// </summary>
    /// <param name="projectorService"></param>
    /// <param name="request"></param>
    /// <param name="learningSpaceId"></param>
    /// <param name="learningComponentId"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<PutProjectorResponse>, Conflict, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IProjectorServices projectorService,
        [FromServices] GlobalMapper globalMapper,
        [FromBody] PutProjectorRequest request,
        [FromRoute(Name = "learningSpaceId")] int learningSpaceId,
        [FromRoute(Name = "learningComponentId")] int learningComponentId)
    {
        var errorMessages = new List<ValidationError>();

        Projector? projectorEntity = null;
        try
        {
            // Map DTO to domain entity
            projectorEntity = globalMapper.ToEntity(request.Projector) as Projector;
        }
        catch (ValidationException exception)
        {
            errorMessages.AddRange(exception.Errors);
        }


        // If there are errors, return BadRequest
        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        // Attempt to update
        var updateSucceeded = await projectorService.UpdateProjectorAsync(
            learningSpaceId!,
            learningComponentId,
            projectorEntity!);

        if (!updateSucceeded)
            return TypedResults.Conflict();
        // Successful response
        return TypedResults.Ok(new PutProjectorResponse(request.Projector));
    }

}