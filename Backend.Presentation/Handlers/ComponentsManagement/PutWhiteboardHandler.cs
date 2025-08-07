using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handles the update process for a Whiteboard entity.
/// This handler performs validation on the incoming request,
/// checks for required fields, ensures valid orientation,
/// dimensions, and position values. It interacts with the 
/// service layer to update the whiteboard entity.
/// </summary>
/// <param name="whiteboardService">The service used to manage whiteboard-related operations.</param>
/// <param name="request">The request containing the updated details of the whiteboard.</param>
/// <returns>
/// A task that represents the asynchronous operation. The task result contains:
/// <list>
/// <item><see cref="Ok{T}"/> with a <see cref="PutWhiteboardResponse"/> if the update is successful.</item>
/// <item><see cref="NotFound{T}"/> if the whiteboard does not exist.</item>
/// <item><see cref="BadRequest{T}"/> if the request is invalid, such as missing required fields or invalid values.</item>
/// </list>
/// </returns>
public static class PutWhiteboardHandler
{
    /// <summary>
    /// Handles the HTTP PUT request to update an existing whiteboard.
    /// </summary>
    /// <param name="whiteboardService"></param>
    /// <param name="request"></param>
    /// <param name="learningSpaceId"></param>
    /// <param name="learningComponentId"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<PutWhiteboardResponse>, Conflict, BadRequest<List<ValidationError>>>> HandleAsync(
           [FromServices] IWhiteboardServices whiteboardService,
           [FromServices] GlobalMapper mapper,
           [FromBody] PutWhiteboardRequest request,
           [FromRoute(Name = "learningSpaceId")] int learningSpaceId,
           [FromRoute(Name = "learningComponentId")] int learningComponentId)
    {
        var errorMessages = new List<ValidationError>();

        Whiteboard? whiteboardEntity = null;

        // Try to map the DTO to the domain entity and catch validation errors
        try
        {
            whiteboardEntity = mapper.ToEntity(request.Whiteboard) as Whiteboard;
        }
        catch (ValidationException exception)
        {
            errorMessages.AddRange(exception.Errors);
        }

        // If there are any validation errors, return a BadRequest
        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(errorMessages);
        }

        // Attempt to update the whiteboard
        var updateSucceeded = await whiteboardService.UpdateWhiteboardAsync(
            learningSpaceId!,
            learningComponentId,
            whiteboardEntity!);

        if (!updateSucceeded)
            return TypedResults.Conflict();

        return TypedResults.Ok(new PutWhiteboardResponse(request.Whiteboard));
    }
}