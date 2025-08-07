using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles HTTP POST requests for creating new Area entities within a university campus context.
/// </summary>
public static class PostAreaHandler
{
    /// <summary>
    /// Processes the creation of a new Area, validating input and mapping DTOs to domain entities.
    /// </summary>
    /// <param name="areaService">The service used to handle area-related operations.</param>
    /// <param name="campusService">The service used to handle campus-related queries and validations.</param>
    /// <param name="request">The DTO containing the area data from the client.</param>
    /// <returns>
    /// A <see cref="Results{T1, T2, T3}"/> object which can be:
    /// - <see cref="Ok{T}"/> if the area was successfully created.
    /// - <see cref="Conflict"/> if the area could not be created due to a conflict (e.g., duplicate entry).
    /// - <see cref="BadRequest{T}"/> if the input data is invalid or related entities do not exist.
    /// </returns>
    public static async Task<Results<Ok<PostAreaResponse>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] IAreaServices areaService,
        [FromServices] ICampusServices campusService,
        [FromBody] AddAreaDto request)
    {
        List<string> errorMessages = new();

        // Validate that the referenced campus exists
        var campusEntity = await campusService.GetByNameAsync(request.Campus.Name);
        if (campusEntity is null)
        {
            errorMessages.Add("The specified campus does not exist.");
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }

        Area? areaEntity = null;

        // Attempt to map the DTO to a domain entity
        try
        {
            areaEntity = AreaDtoMappers.ToEntity(request, campusEntity);
        }
        catch (ValidationException ex)
        {
            errorMessages.Add(ex.Message);
        }

        // Check for mapping errors or null entity
        if (areaEntity is null || errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }
        // Try to add the new area
        try
        {
            var success = await areaService.AddAreaAsync(areaEntity!);

            if (!success)
            {
                return TypedResults.Conflict(new ErrorResponse(new List<string> { "Area could not be added." }));
            }

            return TypedResults.Ok(new PostAreaResponse(request));
        }
        // Handle duplication errors separately
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(new ErrorResponse(new List<string> { ex.Message }));
        }
    }
}
