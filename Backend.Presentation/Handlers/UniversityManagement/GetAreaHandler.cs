using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve all Areas.
/// </summary>
public static class GetAreaHandler
{
    public static async Task<Results<Ok<GetAreaResponse>, ProblemHttpResult>> HandleAsync(
        [FromServices] IAreaServices AreaServices)
    {
        try
        {
            var areas = await AreaServices.ListAreaAsync();

            var listAreaDtos = areas
                .Select(AreaDtoMappers.ToDto)
                .ToList();

            var response = new GetAreaResponse(listAreaDtos);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred while retrieving areas: {ex.Message}");
        }
    }
}
