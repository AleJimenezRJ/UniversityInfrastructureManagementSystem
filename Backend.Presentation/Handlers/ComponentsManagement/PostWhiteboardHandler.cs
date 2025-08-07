using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handles the HTTP POST request to create a new whiteboard.
/// </summary>
public static class PostWhiteboardHandler
{
    /// <summary>
    /// Handles the creation of a new whiteboard using the provided request data.
    /// </summary>
    /// <param name="whiteboardService">Service used to handle whiteboard operations.</param>
    /// <param name="request">The request containing the whiteboard data.</param>
    /// <returns>A result indicating success, conflict, or validation error.</returns>
    public static async Task<Results<Ok<PostWhiteboardResponse>, Conflict, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IWhiteboardServices whiteboardService,
        [FromServices] GlobalMapper globalMapper,
        [FromBody] PostWhiteboardRequest request,
        [FromRoute(Name = "learningSpaceId")] int learningSpaceId)
    {
        var errorMessages = new List<ValidationError>();

        // Convert DTO to entity
        Whiteboard? whiteboardEntity = null;
        try
        {
            whiteboardEntity = globalMapper.ToEntity(request.Whiteboard) as Whiteboard;
        }
        catch (ValidationException exception)
        {
            errorMessages.AddRange(exception.Errors);
        }

        // Return validation errors
        if (errorMessages.Count > 0)
            return TypedResults.BadRequest(errorMessages);

        // Attempt to create
        var additionSucceeded = await whiteboardService.AddWhiteboardAsync(
            learningSpaceId!,
            whiteboardEntity!);

        if (!additionSucceeded)
            return TypedResults.Conflict();

        return TypedResults.Ok(new PostWhiteboardResponse(request.Whiteboard));
    }
}