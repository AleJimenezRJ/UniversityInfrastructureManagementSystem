using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handler class for retrieving a paginated list of buildings.
/// </summary>
public static class GetBuildingPaginatedListHandler
{
    /// <summary>
    /// Handles the HTTP request to get a paginated list of buildings for a given building ID.
    /// Validates query parameters, retrieves data from the service, and maps the result to a response DTO.
    /// </summary>
    /// <param name="buildingServices">Service used to access building-related data.</param>
    /// <param name="pageSize">The number of items per page. Must be between 1 and 20. Default is 10.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// <returns>
    /// A <see cref="Results{T1, T2, T3}"/> object that contains:
    /// <list type="bullet">
    /// <item><description><see cref="Ok{T}"/> with a <see cref="GetBuildingPaginatedListResponse"/> if the request is successful.</description></item>
    /// <item><description><see cref="NotFound{T}"/> if no buildings are found for the provided building ID.</description></item>
    /// <item><description><see cref="BadRequest{T}"/> with a list of <see cref="ValidationError"/> objects if validation fails.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<GetBuildingPaginatedListResponse>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IBuildingsServices buildingServices,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0,
        [FromQuery] string? searchText = null)
    {
        var errors = new List<ValidationError>();

        if (pageSize < 1)
            errors.Add(new ValidationError("PageSize", "Page size must be greater than or equal to 1"));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            var paginatedList = await buildingServices.GetBuildingsListPaginatedAsync(pageSize, pageIndex, searchText);
            // Convert to DTOs
            var dtoList = paginatedList.Select(static building => BuildingDtoMappers.ToDto(building)).ToList();

            // Create response with pagination metadata
            var response = new GetBuildingPaginatedListResponse(
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