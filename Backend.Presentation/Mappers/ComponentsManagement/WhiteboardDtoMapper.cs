using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

// ==================================================================================
// USER STORY: CPD-LC-002-007 – Refactor of polymorphism
// TASK: T2 – Implementation of WhiteboardDtoMapper
// PARTICIPANTS: Luis Fonseca Chinchilla – C03035 (Driver), Ericka Araya Hidalgo – C20553 (Observer)
// COMMENT: Mapper responsible for transforming between Whiteboard entity and its DTO.
// ==================================================================================

/// <summary>
/// Mapper responsible for converting between <see cref="Whiteboard"/> domain entities and <see cref="WhiteboardDto"/> data transfer objects.
/// Implements the mapping logic for both directions: entity to DTO and DTO to entity.
/// </summary>
/// <remarks>
/// Part of the learning component mapping refactor (CPD-LC-002-007), this class enables polymorphic mapping for whiteboard components.
/// </remarks>
public class WhiteboardDtoMapper : LearningComponentDtoMapper<Whiteboard, WhiteboardDto>
{
    /// <summary>
    /// Converts a <see cref="Whiteboard"/> entity to its corresponding <see cref="WhiteboardDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Whiteboard"/> entity to convert.</param>
    /// <returns>A <see cref="WhiteboardDto"/> representing the entity.</returns>

    public override WhiteboardDto ToDto(Whiteboard entity)
    {
        return new WhiteboardDto(
            entity.ComponentId,
            entity.Orientation.Value,
            new PositionDto(entity.Position.X, entity.Position.Y, entity.Position.Z),
            new DimensionsDto(entity.Dimensions.Width, entity.Dimensions.Length, entity.Dimensions.Height),
            entity.MarkerColor!.Value
            );
    }

    /// <summary>
    /// Converts a <see cref="WhiteboardDto"/> to its corresponding <see cref="Whiteboard"/> entity.
    /// Performs validation and throws a <see cref="ValidationException"/> if any property is invalid.
    /// </summary>
    /// <param name="dto">The <see cref="WhiteboardDto"/> to convert.</param>
    /// <returns>A <see cref="Whiteboard"/> entity constructed from the DTO.</returns>
    /// <exception cref="ValidationException">Thrown if any property in the DTO is invalid.</exception>

    public override Whiteboard ToEntity(WhiteboardDto dto)
    {
        var errors = new List<ValidationError>();

        // Id
        if (!Id.TryCreate(dto.Id, out var learningComponentId))
            errors.Add(new ValidationError("Id", $"Invalid learning component id: {dto.Id}"));

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

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new Whiteboard(
            markerColor!,
            learningComponentId!.ValueInt!.Value,
            orientation!,
            position!,
            dimensions!
        );
    }
}