using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handler for retrieving all learning components (whiteboards, projectors, etc.).
/// </summary>
public static class GetLearningComponentHandler
{
    /// <summary>
    /// Handles the request to retrieve all learning components in the system.
    /// </summary>
    /// <param name="learningComponentService">Service to access learning component data.</param>
    /// <param name="pageSize">The quantity of learning components to be fetched</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>
    /// A list of <see cref="GetLearningComponentResponse"/> wrapped in a 200 OK result,
    /// or 404 NotFound if no components are available.
    /// </returns>
    public static async Task<Results<Ok<GetLearningComponentResponse>, Conflict, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] ILearningComponentServices learningComponentService,
        [FromServices] GlobalMapper mapper,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        List<string> errorMessages = [];
        
        if (pageSize is < 1 or > 20)
        {
            errorMessages.Add("Page size must be greater than or equal to 1 and less than or equal to 20.");
        }
        if (pageIndex < 0)
            errorMessages.Add("Page index must be greater than or equal to 0.");
        
        var components = await learningComponentService.GetLearningComponentAsync(pageSize, pageIndex);
        var dtoList = new List<LearningComponentDto>();

        try
        {
            dtoList = components.Select(x => mapper.ToDto(x)).ToList();
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

        var response = new GetLearningComponentResponse(dtoList);
        return TypedResults.Ok(response);
    }
}


    