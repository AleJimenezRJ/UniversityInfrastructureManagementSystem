using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve a area by its ID.
/// </summary>
public static class GetAreaByNameHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve an area.
    /// </summary>
    /// <param name="areaServices"> The services of the area entity</param>
    /// <returns>
    /// - 200 OK with a list of areas if successful.
    /// - Error details with problem information.
    /// </returns>
    public static async Task<Results<Ok<GetAreaByNameResponse>, NotFound<string>>> HandleAsync(
        [FromServices] IAreaServices areaServices,
        [FromRoute] string areaName)
    {
        try
        {
            var area = await areaServices.GetByNameAsync(areaName);

            if (area is null)
            {
                return TypedResults.NotFound("The requested area was not found.");
            }

            var listAreaDto = AreaDtoMappers.ToDto(area);
            var response = new GetAreaByNameResponse(listAreaDto);
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"An error occurred while retrieving the area: {ex.Message}");
        }
    }
}
