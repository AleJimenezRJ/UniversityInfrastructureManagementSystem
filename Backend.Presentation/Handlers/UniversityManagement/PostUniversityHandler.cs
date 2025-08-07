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
/// Handles the HTTP POST request to create a new university.
/// </summary>
public static class PostUniversityHandler
{
    /// <summary>
    /// Creates a new university based on the data provided in the request body.
    /// </summary>
    /// <param name="universityService">The service responsible for university-related operations.</param>
    /// <param name="request">The request payload containing university details to be created.</param>
    /// <returns>
    /// A <see cref="Results{T1,T2,T3}"/> object which may be one of the following:
    /// <list type="bullet">
    ///   <item><description><see cref="Ok{T}"/> - If the university was successfully created.</description></item>
    ///   <item><description><see cref="Conflict{T}"/> - If a conflict occurred, such as a duplicate university or failure during persistence.</description></item>
    ///   <item><description><see cref="BadRequest{T}"/> - If the provided request data is invalid or violates validation rules.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<PostUniversityResponse>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>>> HandleAsync(
    [FromServices] IUniversityServices universityService,
    [FromBody] UniversityDto request)
    {
        List<string> errorMessages = new();

        University? universityEntity = null;

        try
        {
            universityEntity = UniversityDtoMappers.ToEntity(request);
        }
        catch (ValidationException ex)
        {
            errorMessages.Add(ex.Message);
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }

        try
        {
            var success = await universityService.AddUniversityAsync(universityEntity!);

            if (!success)
            {
                return TypedResults.Conflict(new ErrorResponse(new List<string> { "University could not be added." }));
            }

            return TypedResults.Ok(new PostUniversityResponse(request));
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(new ErrorResponse(new List<string> { ex.Message }));
        }
    }
}