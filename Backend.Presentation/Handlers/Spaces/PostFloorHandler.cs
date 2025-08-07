using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

public static class PostFloorHandler
{
    /// <summary>
    /// Handles the creation of a floor using the provided request data.
    /// </summary>
    /// <param name="floorServices">Service that provides business logic for managing floors.</param>
    /// <param name="request">The request containing the floor data.</param>
    /// <param name="routeBuildingId">The unique identifier of the building in the route.</param>
    /// <returns>
    /// A result that can be:
    /// - <see cref="Ok{T}"/> with a success message if the creation was successful.
    /// - <see cref="Conflict{T}"/> with an error message if the floor couldn't be created.
    /// - <see cref="BadRequest{T}"/> with validation errors if the request data is invalid.
    /// </returns>
    public static async Task<Results<Ok<PostFloorResponse>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IFloorServices floorServices,
        [FromRoute(Name = "buildingId")] int routeBuildingId)
    {
        var errors = new List<ValidationError>();

        // Validates routeBuildingId
        if (!Id.TryCreate(routeBuildingId, out var buildingId))
            errors.Add(new ValidationError("BuildingId", "Invalid building id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            // Try to create the floor
            await floorServices.CreateFloorAsync(routeBuildingId);
            return TypedResults.Ok(new PostFloorResponse("The floor was created successfully."));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
