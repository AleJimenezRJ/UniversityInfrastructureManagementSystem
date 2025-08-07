using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides a handler for retrieving a paginated list of learning spaces within a specific floor.
/// </summary>
public static class GetLearningSpacePaginatedListHandler
{
    /// <summary>
    /// Handles the request to retrieve learning spaces associated with a specific floor with pagination.
    /// </summary>
    /// <param name="learningSpaceServices">The service used to interact with learning space data.</param>
    /// <param name="routeFloorId">The unique identifier of the floor whose learning spaces are to be retrieved.</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <param name="pageIndex">The index of the current page (default is 0).</param>
    /// <param name="seachText">Optional search text to filter learning spaces by name or type.</param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// - An <see cref="Ok{T}"/> result with a <see cref="GetLearningSpacePaginatedListResponse"/> if learning spaces are found.
    /// - A <see cref="NotFound{T}"/> result with an error message if no learning spaces are found.
    /// - A <see cref="BadRequest{T}"/> result with an error message if the provided parameters are invalid.
    /// </returns>
    public static async Task<Results<Ok<GetLearningSpacePaginatedListResponse>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [FromRoute(Name = "floorId")] int routeFloorId,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0,
        [FromQuery] string seachText = "")
    {
        var errors = new List<ValidationError>();

        if (!Id.TryCreate(routeFloorId, out var floorId))
            errors.Add(new ValidationError("FloorId", "Invalid floor id format."));

        if (pageSize < 1 || pageSize > 20)
            errors.Add(new ValidationError("PageSize", "Page size must be greater than or equal to 1 and less than or equal to 20."));

        if (pageIndex < 0)
            errors.Add(new ValidationError("PageIndex", "Page index must be greater than or equal to 0."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            var paginatedList = await learningSpaceServices.GetLearningSpacesListPaginatedAsync(routeFloorId, pageSize, pageIndex, seachText.Trim());

            // Convert to DTOs
            var dtoList = paginatedList.Select(static space => LearningSpaceDtoMapper.ToDtoList(space)).ToList();

            // Create response with pagination metadata
            var response = new GetLearningSpacePaginatedListResponse(
                dtoList,
                paginatedList.PageSize,
                paginatedList.PageIndex,
                paginatedList.TotalCount,
                paginatedList.TotalPages);

            return TypedResults.Ok(response);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }
}
