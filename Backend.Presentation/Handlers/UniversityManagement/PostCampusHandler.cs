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
/// Handles HTTP POST requests for creating new Campus entities.
/// </summary>
public static class PostCampusHandler
{
    /// <summary>
    /// Processes the creation of a new campus, including validation of the associated university and entity mapping.
    /// </summary>
    /// <param name="campusService">Service responsible for managing campus data operations.</param>
    /// <param name="universityService">Service responsible for university data retrieval and validation.</param>
    /// <param name="request">The campus data transfer object received from the client.</param>
    /// <returns>
    /// A <see cref="Results{T1, T2, T3}"/> object that can represent:
    /// <list type="bullet">
    /// <item><description><see cref="Ok{T}"/> if the campus was successfully created.</description></item>
    /// <item><description><see cref="Conflict{T}"/> if a campus with similar details already exists or cannot be added.</description></item>
    /// <item><description><see cref="BadRequest{T}"/> if the input is invalid or the specified university does not exist.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<PostCampusResponse>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] ICampusServices campusService,
        [FromServices] IUniversityServices universityService,
        [FromBody] AddCampusDto request)
    {
        List<string> errorMessages = new();

        Campus? campusEntity = null;

        // Check if the specified university exists
        var universityEntity = await universityService.GetByNameAsync(request.University.Name);
        if (universityEntity is null)
        {
            errorMessages.Add("The specified university does not exist.");
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }

        // Try to map the DTO to the domain entity
        try
        {
            campusEntity = CampusDtoMappers.ToEntity(request, universityEntity);
        }
        catch (ValidationException ex)
        {
            errorMessages.Add(ex.Message);
        }

        // Return BadRequest if there are any mapping errors
        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }
        // Try to add the new campus
        try
        {
            var success = await campusService.AddCampusAsync(campusEntity!);

            if (!success)
            {
                return TypedResults.Conflict(new ErrorResponse(new List<string> { "Campus could not be added." }));
            }

            return TypedResults.Ok(new PostCampusResponse(request));
        }
        // Handle duplication errors separately
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(new ErrorResponse(new List<string> { ex.Message }));
        }
    }
}
