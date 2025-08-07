using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP PUT request to update an existing building in the university management system.
/// </summary>
public static class PutBuildingHandler
{
    /// <summary>
    /// Updates an existing building based on the provided building ID and request data.
    /// </summary>
    /// <param name="buildingService">The service responsible for building-related operations.</param>
    /// <param name="areaService">The service used to validate and retrieve area information.</param>
    /// <param name="BuildingId">The ID of the building to be updated, provided as a route parameter.</param>
    /// <param name="request">The request payload containing updated building data including name and area information.</param>
    /// <returns>
    /// A <see cref="Results{T1,T2,T3,T4,T5}"/> object which may be one of the following:
    /// <list type="bullet">
    ///   <item><description><see cref="Ok{T}"/> - If the building was successfully updated.</description></item>
    ///   <item><description><see cref="Conflict{T}"/> - If a conflict occurred, such as a duplicate building name or failure during persistence.</description></item>
    ///   <item><description><see cref="BadRequest{T}"/> - If the request data is invalid or contains validation errors, including references to non-existent areas or a missing building.</description></item>
    ///   <item><description><see cref="Conflict{T}"/> - If a duplicate entity exception is thrown during update.</description></item>
    ///   <item><description><see cref="BadRequest{T}"/> - If the request is invalid due to model validation issues.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<PutBuildingResponse>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IBuildingsServices buildingService,
        [FromServices] IAreaServices areaService,
        [FromRoute] int BuildingId,
        [FromBody] AddBuildingDto request)
    {
        var errors = new List<ValidationError>();

        var validBuilding = await buildingService.DisplayBuildingAsync(BuildingId);
        if (validBuilding is null)
        {
            errors.Add(new ValidationError("Building", "The building with the requested ID was not found"));
        }

        // Fetch the area entity
        var areaEntity = await areaService.GetByNameAsync(request.Area.Name);
        if (areaEntity is null)
        {
            errors.Add(new ValidationError("Area", "The area name is not valid"));
        }

        Building? buildingEntity;
        try
        {
            buildingEntity = BuildingDtoMappers.ToEntity(request, areaEntity!);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(ex.Errors.ToList());
        }
        if (buildingEntity is null || errors.Count > 0)
        {
            return TypedResults.BadRequest(errors);
        }

        try
        {
            var success = await buildingService.UpdateBuildingAsync(buildingEntity!, BuildingId);

            if (!success)
            {
                return TypedResults.Conflict(new ErrorResponse(new List<string> { "Building could not be updated." }));
            }
            return TypedResults.Ok(new PutBuildingResponse(BuildingDtoMappers.ToSecondDto(buildingEntity!)));
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(new ErrorResponse(new List<string> { ex.Message }));
        }
    }
}
