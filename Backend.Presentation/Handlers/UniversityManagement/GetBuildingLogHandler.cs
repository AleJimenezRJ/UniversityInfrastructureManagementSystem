using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;


/// <summary>
/// Provides functionality to handle requests for retrieving building logs.
/// </summary>
/// <remarks>This handler retrieves a list of building logs and returns them as a collection of DTOs.  If no logs
/// are found, a message indicating the absence of logs is returned.</remarks>
public static class GetBuildingLogHandler
{
    /// <summary>
    /// Handles the retrieval of building logs and returns the results as a list of DTOs or a message indicating no logs
    /// are available.
    /// </summary>
    /// <remarks>This method retrieves building logs using the provided <paramref
    /// name="buildingLogServices"/>.  If no logs are found, it returns a message indicating the absence of logs.
    /// Otherwise, it maps the logs to DTOs and returns them.</remarks>
    /// <param name="buildingLogServices">The service used to retrieve building logs. This parameter is provided via dependency injection.</param>
    /// <returns>A <see cref="Results{T1, T2}"/> object containing either: <list type="bullet"> <item> <description> An <see
    /// cref="Ok{T}"/> result with a list of <see cref="BuildingLogDto"/> objects representing the building logs.
    /// </description> </item> <item> <description> An <see cref="Ok{T}"/> result with a string message indicating that
    /// no building logs are available. </description> </item> </list></returns>
    public static async Task<Results<Ok<List<BuildingLogDto>>, Ok<string>>> HandleAsync(
    [FromServices] IBuildingLogServices buildingLogServices)
    {
        var logs = await buildingLogServices.ListBuildingLogsAsync();

        if (logs == null || logs.Count == 0)
        {
            return TypedResults.Ok("There are no registered changes to the buildings.");
        }

        var dtoList = BuildingLogDtoMappers.ToDto(logs);
        return TypedResults.Ok(dtoList);
    }

}

