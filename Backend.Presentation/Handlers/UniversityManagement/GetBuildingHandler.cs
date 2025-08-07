using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve all buildings.
/// </summary>
public static class GetBuildingHandler
{
    public static async Task<Results<Ok<GetBuildingResponse>, ProblemHttpResult>> HandleAsync(
        [FromServices] IBuildingsServices buildingsServices)
    {
        try
        {
            var buildings = await buildingsServices.ListBuildingAsync();

            var listBuildingDtos = buildings
                .Select(BuildingDtoMappers.ToDto)
                .ToList();

            var response = new GetBuildingResponse(listBuildingDtos);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred while retrieving buildings: {ex.Message}");
        }
    }
}
