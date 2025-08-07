using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

// ==================================================================================
// USER STORY: CPD-LC-002-007 – Refactor of polymorphism
// TASK: T2 – Implementation of ProjectorDtoNoIdMapper
// PARTICIPANTS: Luis Fonseca Chinchilla – C03035 (Observer), Ericka Araya Hidalgo – C20553 (Driver)
// COMMENT: Mapper responsible for transforming between Projector entity and its DTO.
// ==================================================================================


/// <summary>
/// Mapper responsible for converting between <see cref="Projector"/> domain entities and <see cref="ProjectorNoIdDto"/> data transfer objects (DTOs without ID).
/// Implements the mapping logic for both directions: entity to DTO and DTO to entity, for creation scenarios where the ID is not present.
/// </summary>
/// <remarks>
/// Part of the learning component mapping refactor (CPD-LC-002-007), this class enables polymorphic mapping for projector components without IDs.
/// </remarks>
public class ProjectorNoIdDtoMapper : LearningComponentNoIdDtoMapper<Projector, ProjectorNoIdDto>
{
    /// <summary>
    /// Converts a <see cref="Projector"/> entity to its corresponding <see cref="ProjectorNoIdDto"/>.
    /// </summary>
    /// <param name="projector">The <see cref="Projector"/> entity to convert.</param>
    /// <returns>A <see cref="ProjectorNoIdDto"/> representing the entity, without an ID.</returns>
    public override ProjectorNoIdDto ToDto(Projector projector)
    {
        return new ProjectorNoIdDto(
            projector.Orientation.Value,
            new PositionDto(projector.Position.X, projector.Position.Y, projector.Position.Z),
            new DimensionsDto(projector.Dimensions.Width, projector.Dimensions.Length, projector.Dimensions.Height),
            new ProjectionAreaDto(projector.ProjectionArea!.Height, projector.ProjectionArea.Length),
            projector.ProjectedContent!
        );
    }

    /// <summary>
    /// Converts a <see cref="ProjectorNoIdDto"/> to its corresponding <see cref="Projector"/> entity.
    /// Performs validation and throws a <see cref="ValidationException"/> if any property is invalid.
    /// </summary>
    /// <param name="dto">The <see cref="ProjectorNoIdDto"/> to convert.</param>
    /// <returns>A <see cref="Projector"/> entity constructed from the DTO.</returns>
    /// <exception cref="ValidationException">Thrown if any property in the DTO is invalid.</exception>
    public override Projector ToEntity(ProjectorNoIdDto dto)
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

        // Projection Area
        if (!Area2D.TryCreate(dto.ProjectionArea.ProjectedHeight, dto.ProjectionArea.ProjectedWidth, out var projectionArea))
            errors.Add(new ValidationError("Projection Area", "Invalid projection area dimensions"));

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new Projector(
            dto.ProjectedContent,
            projectionArea!,
            orientation!,
            position!,
            dimensions!
        );
    }

}