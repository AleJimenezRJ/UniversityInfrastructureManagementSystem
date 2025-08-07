using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handles requests to retrieve all whiteboards.
/// </summary>
public static class GetWhiteboardHandler
{
    /// <summary>
    /// Retrieves all whiteboards and returns them as DTOs.
    /// </summary>
    /// <param name="whiteboardService">Injected service for whiteboard data.</param>
    /// <returns>A 200 OK result with whiteboard data, or 404 if none are found.</returns>
    public static async Task<Results<Ok<GetWhiteboardResponse>, Conflict, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] IWhiteboardServices whiteboardService,
        [FromServices] GlobalMapper mapper)
    {
        var whiteboard = await whiteboardService.GetWhiteboardAsync();
        var dtoList = new List<WhiteboardDto?>();

        List<string> errorMessages = new();

        try
        {
            dtoList = whiteboard
             .Select(x => mapper.ToDto(x) as WhiteboardDto)
             .ToList();
        }
        catch (NotSupportedException)
        {
            return TypedResults.Conflict();
        }

        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(
                new ErrorResponse(errorMessages));
        }

        var response = new GetWhiteboardResponse(dtoList!);
        return TypedResults.Ok(response);
    }
}


