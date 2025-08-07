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
/// Provides functionality to handle HTTP POST requests for creating new buildings within the university management system.
/// </summary>
public static class PostBuildingHandler
{
    /// <summary>
    /// Handles the creation of a new building. Validates the incoming request, ensures the referenced area exists, 
    /// maps the request DTO to a domain entity, and invokes the appropriate service to persist the building.
    /// </summary>
    /// <param name="buildingService">Service for building-related operations, including persistence.</param>
    /// <param name="areaService">Service for area-related operations, such as validation and retrieval.</param>
    /// <param name="request">The incoming request containing building information to be created.</param>
    /// <returns>
    /// An HTTP result indicating the outcome of the request:
    /// <list type="bullet">
    /// <item><description><see cref="Ok{T}"/>: The building was successfully created and returned.</description></item>
    /// <item><description><see cref="Conflict{T}"/>: A building with the same name already exists, or saving failed due to a conflict.</description></item>
    /// <item><description><see cref="BadRequest{T}"/>: The request contained invalid data or referenced a non-existent area.</description></item>
    /// <item><description><see cref="Conflict{T}"/> with <c>string</c>: A general duplication error occurred.</description></item>
    /// <item><description><see cref="BadRequest{T}"/> with <c>List&lt;ValidationError&gt;</c>: One or more validation errors were found in the request.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<PostBuildingResponse>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IBuildingsServices buildingService,
        [FromServices] IAreaServices areaService,
        [FromBody] AddBuildingDto request)
    {
        var errors = new List<ValidationError>();

        // Attempt to retrieve the area by name
        var areaEntity = await areaService.GetByNameAsync(request.Area.Name);
        if (areaEntity is null)
        {
            errors.Add(new ValidationError("Area", "The area name is not valid"));
        }

        Building? buildingEntity;
        try
        {
            // Map the incoming DTO to a domain entity and validate it
            buildingEntity = BuildingDtoMappers.ToEntity(request, areaEntity!);
        }
        catch (ValidationException ex)
        {
            // Return detailed validation errors if entity creation fails
            return TypedResults.BadRequest(ex.Errors.ToList());
        }

        if (errors.Count > 0)
        {
            return TypedResults.BadRequest(errors);
        }

        try
        {
            // Attempt to persist the new building
            var success = await buildingService.AddBuildingAsync(buildingEntity!);
            if (!success)
            {
                return TypedResults.Conflict(new ErrorResponse(new List<string> { "Building could not be added." }));
            }

            // Return the newly created building in the response
            return TypedResults.Ok(new PostBuildingResponse(BuildingDtoMappers.ToSecondDto(buildingEntity!)));
        }
        catch (DuplicatedEntityException ex)
        {
            // Handle duplicate entity scenario
            return TypedResults.Conflict(new ErrorResponse(new List<string> { ex.Message }));
        }
    }
}
