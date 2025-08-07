using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;


// ==================================================================================
// USER STORY: CPD-LC-002-007 – Refactor of polymorphism
// TASK: T2 – Implementation of WhiteboardDtoNoIdMapper
// PARTICIPANTS: Luis Fonseca Chinchilla – C03035 (Observer), Ericka Araya Hidalgo – C20553 (Driver)
// COMMENT: Mapper responsible for transforming between Whiteboard entity and its DTO.
// ==================================================================================

/// <summary>
/// Mapper responsible for converting between <see cref="Whiteboard"/> domain entities and <see cref="WhiteboardNoIdDto"/> data transfer objects (DTOs without ID).
/// Implements the mapping logic for both directions: entity to DTO and DTO to entity, for creation scenarios where the ID is not present.
/// </summary>
/// <remarks>
/// Part of the learning component mapping refactor (CPD-LC-002-007), this class enables polymorphic mapping for whiteboard components without IDs.
/// </remarks>
public class WhiteboardNoIdDtoMapper : LearningComponentNoIdDtoMapper<Whiteboard, WhiteboardNoIdDto>
{
    /// <summary>
    /// Converts a <see cref="Whiteboard"/> entity to its corresponding <see cref="WhiteboardNoIdDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Whiteboard"/> entity to convert.</param>
    /// <returns>A <see cref="WhiteboardNoIdDto"/> representing the entity, without an ID.</returns>
    public override WhiteboardNoIdDto ToDto(Whiteboard entity)
    {
        return new WhiteboardNoIdDto(
            entity.Orientation.Value,
            new PositionDto(entity.Position.X, entity.Position.Y, entity.Position.Z),
            new DimensionsDto(entity.Dimensions.Width, entity.Dimensions.Length, entity.Dimensions.Height),
            entity.MarkerColor!.Value!
            );
    }

    /// <summary>
    /// Converts a <see cref="WhiteboardNoIdDto"/> to its corresponding <see cref="Whiteboard"/> entity.
    /// Performs validation and throws a <see cref="ValidationException"/> if any property is invalid.
    /// </summary>
    /// <param name="dto">The <see cref="WhiteboardNoIdDto"/> to convert.</param>
    /// <returns>A <see cref="Whiteboard"/> entity constructed from the DTO.</returns>
    /// <exception cref="ValidationException">Thrown if any property in the DTO is invalid.</exception>
    public override Whiteboard ToEntity(WhiteboardNoIdDto dto)
    {
        var errors = new List<ValidationError>();

        // Orientation
        if (!Orientation.TryCreate(dto.Orientation, out var orientation))
            errors.Add(new ValidationError("Orientation", $"Invalid orientation: {dto.Orientation}"));

        // Dimensions
        if (!Dimension.TryCreate(dto.Dimensions.Width, dto.Dimensions.Length, dto.Dimensions.Height, out var dimensions))
            errors.Add(new ValidationError("Dimensions", "Invalid dimensions"));

        // Position
        if (!Coordinates.TryCreate(dto.Position.X, dto.Position.Y, dto.Position.Z, out var position))
            errors.Add(new ValidationError("Position", "Invalid position coordinates"));

        //Colors
        if (!Colors.TryCreate(dto.MarkerColor, out var markerColor))
            errors.Add(new ValidationError("Color", $"Invalid color: {dto.MarkerColor}"));

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return new Whiteboard(
            markerColor!,
            orientation!,
            position!,
            dimensions!
        );
    }
}