using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides a handler for retrieving a paginated list of floors within a specific building.
/// </summary>
public static class GetFloorPaginatedListHandler
{
    /// <summary>
    /// Handles the request to retrieve floors associated with a specific building with pagination.
    /// </summary>
    /// <param name="floorsServices">The service used to interact with floors data.</param>
    /// <param name="routeBuildingId">The unique identifier of the building whose floors are to be retrieved.</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <param name="pageIndex">The index of the current page (default is 0).</param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// - An <see cref="Ok{T}"/> result with a <see cref="GetFloorPaginatedListResponse"/> if floors are found.
    /// - A <see cref="NotFound{T}"/> result with an error message if no floors are found.
    /// - A <see cref="BadRequest{T}"/> result with an error message if the provided parameters are invalid.
    /// </returns>
    public static async Task<Results<Ok<GetFloorPaginatedListResponse>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IFloorServices floorsServices,
        [FromRoute(Name = "buildingId")] int routeBuildingId,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var errors = new List<ValidationError>();

        if (!Id.TryCreate(routeBuildingId, out var buildingId))
            errors.Add(new ValidationError("BuildingId", "Invalid building id format."));

        if (pageSize < 1 || pageSize > 20)
            errors.Add(new ValidationError("PageSize", "Page size must be greater than or equal to 1 and less than or equal to 20."));

        if (pageIndex < 0)
            errors.Add(new ValidationError("PageIndex", "Page index must be greater than or equal to 0."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            var paginatedList = await floorsServices.GetFloorsListPaginatedAsync(routeBuildingId, pageSize, pageIndex);
            
            // Convert to DTOs
            var dtoList = paginatedList.Select(static floor => FloorDtoMapper.ToDto(floor)).ToList();

            // Create response with pagination metadata
            var response = new GetFloorPaginatedListResponse(
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
