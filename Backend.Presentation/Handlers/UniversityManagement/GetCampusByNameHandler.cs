using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

public static class GetCampusByNameHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve an specific campus.
    /// </summary>
    /// <param name="campusServices"> The services of the campus entity</param>
    /// <returns>
    /// - 200 OK with a campus if successful.
    /// - Error details with problem information.
    /// </returns>
    public static async Task<Results<Ok<GetCampusByNameResponse>, NotFound<string>>> HandleAsync(
        [FromServices] ICampusServices campusServices,
        [FromRoute] string campusName)
    {
        try
        {
            var campus = await campusServices.GetByNameAsync(campusName);

            if (campus is null)
            {
                return TypedResults.NotFound("The requested campus was not found.");
            }

            var listCampusDto = CampusDtoMappers.ToDto(campus);
            var response = new GetCampusByNameResponse(listCampusDto);
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"An error occurred while retrieving the campus: {ex.Message}");
        }
    }
}
