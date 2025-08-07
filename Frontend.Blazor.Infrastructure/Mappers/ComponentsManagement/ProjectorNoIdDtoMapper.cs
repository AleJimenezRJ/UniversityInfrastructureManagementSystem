using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using DomainValidationError = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions.ValidationError;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

/// <summary>
/// Provides mapping methods to convert <see cref="Projector"/> domain entities
/// into <see cref="ProjectorNoIdDto"/> data transfer objects.
/// </summary>
public class ProjectorNoIdDtoMapper : LearningComponentNoIdDtoMapper<Projector, ProjectorNoIdDto>
{
    /// <summary>
    /// Converts a <see cref="Projector"/> entity to a <see cref="ProjectorNoIdDto"/> DTO.
    /// </summary>
    /// <param name="projector">The projector entity to convert.</param>
    /// <returns>A <see cref="ProjectorNoIdDto"/> populated with the values from the entity.</returns>
    public override ProjectorNoIdDto ToDto(Projector projector)
    {
        return new ProjectorNoIdDto
        {
            Orientation = projector.Orientation.Value,
            Position = new PositionDto
            {
                X = projector.Position.X,
                Y = projector.Position.Y,
                Z = projector.Position.Z
            },
            Dimensions = new DimensionsDto
            {
                Width = projector.Dimensions.Width,
                Length = projector.Dimensions.Length,
                Height = projector.Dimensions.Height
            },
            ProjectionArea = new ProjectionAreaDto
            {
                ProjectedHeight = projector.ProjectionArea!.Height,
                ProjectedWidth = projector.ProjectionArea.Length
            },
            ProjectedContent = projector.ProjectedContent
        };
    }

    public override Projector ToEntity(ProjectorNoIdDto dto)
    {
        return new Projector(
            dto.ProjectedContent!,
            Area2D.Create(
                dto.ProjectionArea!.ProjectedHeight!.Value,
                dto.ProjectionArea.ProjectedWidth!.Value
            ),
            Orientation.Create(dto.Orientation),
            Coordinates.Create(
                dto.Position!.X!.Value,
                dto.Position.Y!.Value,
                dto.Position.Z!.Value
            ),
            Dimension.Create(
                dto.Dimensions!.Width!.Value,
                dto.Dimensions.Length!.Value,
                dto.Dimensions.Height!.Value
            )
        );
    }
}
