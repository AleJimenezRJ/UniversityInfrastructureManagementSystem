
// User Story: ID 376  SQL-LS-001-014 Filter Learning Spaces by Partial Name 
// Tasks Implemented: Search filter by name and type
// Description:
// Implemented search functionality that filters based on the text entered by the user,
// matching both the name and the type of the Learning Space.
// This logic is handled in the backend by querying the database and returning
// a list of learning spaces that meet the criteria.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides a handler for retrieving a list of learning spaces within a specific building.
/// </summary>
public static class GetLearningSpaceListHandler
{
    /// <summary>
    /// Handles the request to retrieve all learning spaces associated with a specific building.
    /// </summary>
    /// <param name="learningSpaceServices">The service used to interact with learning space data.</param>
    /// <param name="routeFloorId">The unique identifier of the floor whose learning spaces are to be retrieved.</param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// - An <see cref="Ok{T}"/> result with a <see cref="GetLearningSpaceListResponse"/> if learning spaces are found.
    /// - A <see cref="NotFound{T}"/> result with an error message if no learning spaces are found.
    /// - A <see cref="BadRequest{T}"/> result with an error message in if the provide parameter is invalid.
    /// </returns>
    public static async Task<Results<Ok<GetLearningSpaceListResponse>, NotFound<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [FromRoute(Name = "floorId")] int routeFloorId)
    {
        var errors = new List<ValidationError>();

        // Validates FloorId
        if (!Id.TryCreate(routeFloorId, out var floorId))
            errors.Add(new ValidationError("FloorId", "Invalid floor id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            var learningSpaceList = await learningSpaceServices.GetLearningSpacesListAsync(routeFloorId);
            // Happy path
            var dtoList = learningSpaceList!.Select(static space => LearningSpaceDtoMapper.ToDtoList(space)).ToList();
            var response = new GetLearningSpaceListResponse(dtoList);
            return TypedResults.Ok(response);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }
}

