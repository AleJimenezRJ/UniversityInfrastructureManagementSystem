using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides a Handler to be able to delete a floor in a building. 
/// </summary>
public static class DeleteFloorHandler
{
    /// <summary>
    /// Handles the HTTP request to delete a specific floor from a building based on the provided route parameters.
    /// </summary>
    /// <param name="floorServices">The service responsible for performing floor-related operations.</param>
    /// <param name="routeFloorId">The unique identifier of the floor to be deleted.</param>
    /// <returns>
    /// An <see cref="IResult"/> representing the outcome of the operation:
    /// <list type="bullet">
    ///   <item><description><c>200 OK</c> with a success message if the floor is deleted successfully.</description></item>
    ///   <item><description><c>400 Bad Request</c> if the input validation fails, incorrect format or invalid values.</description></item>
    ///   <item><description><c>404 Not Found</c> if the specified floor does not exist.</description></item>
    ///   <item><description><c>409 Conflict</c> if an exception occurs during the operation.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<NoContent, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IFloorServices floorServices,
        [FromRoute(Name = "floorId")] int routeFloorId)
    {
        var errors = new List<ValidationError>();

        // Validates FloorNumber
        if (!Id.TryCreate(routeFloorId, out var floorId))
            errors.Add(new ValidationError("FloorId", "Invalid floor id format."));

        // If there are any validation errors, return them
        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        try
        {
            await floorServices.DeleteFloorAsync(routeFloorId);
            // Returns NoContent if the deletion was successful
            return TypedResults.NoContent();
        }
        catch (NotFoundException ex)
        {
            // Returns NotFound if the learning space does not exist
            return TypedResults.NotFound(ex.Message);
        }
        catch (ConcurrencyConflictException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
