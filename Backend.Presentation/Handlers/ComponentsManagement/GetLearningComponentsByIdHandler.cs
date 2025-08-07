using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Handler for retrieving learning components for a specific learning space.
/// </summary>
public static class GetLearningComponentsByIdHandler
{
    /// <summary>
    /// Retrieves learning components based on building, floor, and learning space identifiers.
    /// </summary>
    /// <param name="learningComponentService">Injected service for retrieving components.</param>
    /// <param name="routeLearningSpaceId">The learning space identifier.</param>
    /// <param name="stringSearch">The string to filter the results on.</param>
    /// <param name="pageSize">The quantity of learning components to be fetched</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>Filtered list of components in the specified learning space, or 404 if none found.</returns>
    public static async Task<Results<Ok<GetLearningComponentsByIdResponse>, Conflict, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningComponentServices learningComponentService,
        [FromServices] GlobalMapper globalMapper,
        [FromRoute(Name = "learningSpaceId")] int routeLearningSpaceId,
        [FromQuery] string stringSearch = "",
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var errorMessages = new List<ValidationError>();
        
        if (pageSize is < 1 or > 20)
            errorMessages.Add(new ValidationError("PageSize", "Page size must be greater than or equal to 1 and less than or equal to 20."));

        if (pageIndex < 0)
            errorMessages.Add(new ValidationError("PageIndex", "Page index must be greater than or equal to 0."));
        
        // Validate LearningSpaceID
        if (!Id.TryCreate(routeLearningSpaceId, out var learningSpaceId))
        {
            errorMessages.Add(new ValidationError("Learning Space Id", "Invalid learning space id format."));
        }

        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(errorMessages);
        }

        var dtoList = new List<LearningComponentDto>();

        var components = await learningComponentService.GetLearningComponentsByIdAsync(routeLearningSpaceId, pageSize, pageIndex, stringSearch);

        try
        {
            dtoList = components.Select(x => globalMapper.ToDto(x)).ToList();
        }
        catch (ValidationException exception)
        {
            errorMessages.AddRange(exception.Errors);
        }
        catch (NotSupportedException ex)
        {
            errorMessages.Add(new ValidationError("Mapper", ex.Message));
        }

        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(errorMessages);
        }

        return TypedResults.Ok(new GetLearningComponentsByIdResponse(dtoList, components.PageSize, components.PageIndex, components.TotalCount, components.TotalPages));
    }
}