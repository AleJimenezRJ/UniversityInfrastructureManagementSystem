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
/// Handler responsible for retrieving a projector by its ID.
/// </summary>
public static class GetProjectorHandler
{
    /// <summary>
    /// Retrieves all projectors and returns them as DTOs.
    /// </summary>
    /// <param name="projectorService">Injected service for projector data.</param>
    /// <returns>A 200 OK result with a list of projectors.</returns>
    public static async Task<Results<Ok<GetProjectorResponse>, Conflict, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] IProjectorServices projectorService,
        [FromServices] GlobalMapper mapper)

    {
        var projector = await projectorService.GetProjectorAsync();
        var dtoList = new List<ProjectorDto?>();
        List<string> errorMessages = new();

        try
        {
            dtoList = projector
             .Select(x => mapper.ToDto(x) as ProjectorDto)
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

        var response = new GetProjectorResponse(dtoList!);
        return TypedResults.Ok(response);
    }
}
