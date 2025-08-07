using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handles the HTTP POST request to create a new projector.
/// </summary>
public static class PostProjectorHandler
{
    /// <summary>
    /// Handles the creation of a new projector using the provided request data.
    /// </summary>
    /// <param name="projectorService"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<PostProjectorResponse>, Conflict, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IProjectorServices projectorService,
        [FromServices] GlobalMapper globalMapper,
        [FromBody] PostProjectorRequest request,
        [FromRoute(Name = "learningSpaceId")] int learningSpaceId)
    {
        var errorMessages = new List<ValidationError>();

        // Convert DTO to entity
        Projector? projectorEntity = null;
        try
        {
            projectorEntity = globalMapper.ToEntity(request.Projector) as Projector;
        }
        catch (ValidationException exception)
        {
            errorMessages.AddRange(exception.Errors);
        }

        // Return validation errors
        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        // Attempt to create
        var additionSucceeded = await projectorService.AddProjectorAsync(
            learningSpaceId!,
            projectorEntity!);

        if (!additionSucceeded)
            return TypedResults.Conflict();

        return TypedResults.Ok(new PostProjectorResponse(request.Projector));
    }
}