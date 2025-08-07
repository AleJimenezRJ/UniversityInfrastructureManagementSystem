using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve a building by its ID.
/// </summary>
public static class GetBuildingByIdHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve a building.
    /// </summary>
    /// <param name="buildingsServices"> The services of the building entity</param>
    /// <returns>
    /// - 200 OK with a list of buildings if successful.
    /// - Error details with problem information.
    /// </returns>
    public static async Task<Results<Ok<GetBuildingByNameResponse>, NotFound<string>>> HandleAsync(
        [FromServices] IBuildingsServices buildingsServices,
        [FromRoute] int BuildingId)
    {
        try
        {
            var building = await buildingsServices.DisplayBuildingAsync(BuildingId);

            if (building is null)
            {
                return TypedResults.NotFound("The requested building was not found.");
            }

            var listBuildingDto = BuildingDtoMappers.ToDto(building);
            var response = new GetBuildingByNameResponse(listBuildingDto);
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"An error occurred while retrieving the building: {ex.Message}");
        }
    }
}
