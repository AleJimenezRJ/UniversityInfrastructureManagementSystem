using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides a handler for retrieving a list of floors within a specific building.
/// </summary>
public static class GetFloorListHandler
{
    /// <summary>
    /// Handles the request to retrieve all floors associated with a specific building.
    /// </summary>
    /// <param name="floorServices">The service used to interact with floor data.</param>
    /// <param name="routeBuildingId">The unique identifier of the building whose floors are to be retrieved.</param>
    /// <returns></returns>
    public static async Task<Results<Ok<GetFloorListResponse>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IFloorServices floorServices,
        [FromRoute(Name = "buildingId")] int routeBuildingId)
    {
        var errors = new List<ValidationError>();

        // Validates FloorNumber
        if (!Id.TryCreate(routeBuildingId, out var buildingId))
            errors.Add(new ValidationError("BuildingId", "Invalid building id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            var floorList = await floorServices.GetFloorsListAsync(
                routeBuildingId);
            var dtoList = floorList!.Select(static floor => FloorDtoMapper.ToDto(floor)).ToList();

            var response = new GetFloorListResponse(dtoList);
            return TypedResults.Ok(response);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }
}
