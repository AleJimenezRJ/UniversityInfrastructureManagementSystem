using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

/// <summary>
/// Provides extension methods to map <see cref="ProjectorDto"/> objects to <see cref="Projector"/> entities.
/// </summary>
public class ProjectorDtoMapper : LearningComponentDtoMapper<Projector, ProjectorDto>
{
    /// <summary>
    /// Maps a <see cref="ProjectorDto"/> instance to a <see cref="Projector"/> domain entity.
    /// </summary>
    /// <param name="dto">The DTO to be mapped.</param>
    /// <returns>A <see cref="Projector"/> domain entity based on the values of the DTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown when a required field in the DTO is null.</exception>
    public override Projector ToEntity(ProjectorDto dto)
    {
        return new Projector(
            dto.ProjectedContent!, 
            Area2D.Create(
                dto.ProjectionArea!.ProjectedHeight!.Value, 
                dto.ProjectionArea.ProjectedWidth!.Value   
            ),
            Id.Create(dto.Id).ValueInt!.Value,             
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

    /// <summary>
    /// Converts a <see cref="Projector"/> entity to a <see cref="ProjectorDto"/>.
    /// </summary>
    /// <param name="projector">The <see cref="Projector"/> entity instance to be mapped to a DTO.</param>
    /// <returns>A <see cref="ProjectorDto"/> containing the mapped information from the supplied <see cref="Projector"/> instance.</returns>
    public override ProjectorDto ToDto(Projector projector)
    {
        return new ProjectorDto
        {
            Id = projector.ComponentId,
            Orientation = projector.Orientation.Value,
            Dimensions = new DimensionsDto
            {
                Width = projector.Dimensions.Width,
                Length = projector.Dimensions.Length,
                Height = projector.Dimensions.Height
            },
            Position = new PositionDto
            {
                X = projector.Position.X,
                Y = projector.Position.Y,
                Z = projector.Position.Z
            },
            ProjectedContent = projector.ProjectedContent,
            ProjectionArea = new ProjectionAreaDto
            {
                ProjectedHeight= projector.ProjectionArea!.Height,
                ProjectedWidth = projector.ProjectionArea.Length
            }
        };
    }
}
