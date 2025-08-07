using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve all Campuses.
/// </summary>
public static class GetCampusHandler
{
    public static async Task<Results<Ok<GetCampusResponse>, ProblemHttpResult>> HandleAsync(
        [FromServices] ICampusServices CampusServices)
    {
        try
        {
            var campuses = await CampusServices.ListCampusAsync();

            var campusDtos = campuses
                .Select(CampusDtoMappers.ToDto)
                .ToList();

            var response = new GetCampusResponse(campusDtos);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred while retrieving campuses: {ex.Message}");
        }
    }
}
