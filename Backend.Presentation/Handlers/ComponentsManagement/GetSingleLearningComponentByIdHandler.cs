using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

/// <summary>
/// Responsible for handling requests to retrieve a single learning component
/// by its unique identifier. This handler validates the provided learning
/// component identifier, interacts with the learning component service, and
/// returns the appropriate response based on the operation result.
/// </summary>
public static class GetSingleLearningComponentByIdHandler
{
    /// <summary>
    /// Handles the retrieval of a single learning component by its ID, validating the input and transforming the component into a DTO.
    /// </summary>
    /// <param name="learningComponentService">The service responsible for managing learning components.</param>
    /// <param name="routeLearningComponentId">The ID of the learning component to retrieve, provided in the route.</param>
    /// <returns>
    /// A result containing either an HTTP 200 OK response with the learning component data or an HTTP 400 Bad Request response
    /// with a list of validation errors.
    /// </returns>
    /// <exception cref="InvalidCastException">Thrown when the component type is unrecognized or cannot be cast.</exception>
    public static async Task<Results<Ok<GetSingleLearningComponentByIdResponse>, BadRequest<List<ValidationError>>>>
        HandleAsync(
            [FromServices] ILearningComponentServices learningComponentService,
            [FromServices] GlobalMapper mapper,
            [FromRoute(Name = "learningComponentId")] int routeLearningComponentId)
    {
        var errorMessages = new List<ValidationError>();
        
        // Validate learning component id
        if (!Id.TryCreate(routeLearningComponentId, out var componentId))
        {
            errorMessages.Add(new ValidationError("Learning Component Id", "Invalid learning component id format."));
            return TypedResults.BadRequest(errorMessages);
        }

        var component = await learningComponentService.GetSingleLearningComponentByIdAsync(routeLearningComponentId);

        LearningComponentDto dto = null!;

        try
        {
            dto = mapper.ToDto(component!);
        }
        catch (NotSupportedException ex)
        {
            errorMessages.Add(new ValidationError("Mapper", ex.Message));
        }

        if (errorMessages.Count > 0)
        {
            return TypedResults.BadRequest(errorMessages);
        }
        
        return TypedResults.Ok(new GetSingleLearningComponentByIdResponse(dto));
    }
}