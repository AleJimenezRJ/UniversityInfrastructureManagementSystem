using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

public static class GetUniversityByNameHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve an specific university.
    /// </summary>
    /// <param name="universityServices"> The services of the university entity</param>
    /// <returns>
    /// - 200 OK with a university if successful.
    /// - Error details with problem information.
    /// </returns>
    public static async Task<Results<Ok<GetUniversityByNameResponse>, NotFound<string>>> HandleAsync(
        [FromServices] IUniversityServices universityServices,
        [FromRoute] string universityName)
    {
        try
        {
            var university = await universityServices.GetByNameAsync(universityName);

            if (university is null)
            {
                return TypedResults.NotFound("The requested university was not found.");
            }

            var universityDto = UniversityDtoMappers.ToDto(university);
            var response = new GetUniversityByNameResponse(universityDto);
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"An error occurred while retrieving the university: {ex.Message}");
        }
    }
}
