using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using DomainValidationError = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions.ValidationError;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

/// <summary>
/// Provides extension methods to map <see cref="WhiteboardDto"/> data transfer objects
/// to <see cref="Whiteboard"/> domain entities.
/// </summary>
public class WhiteboardDtoMapper : LearningComponentDtoMapper<Whiteboard, WhiteboardDto>
{
    /// <summary>
    /// Converts a <see cref="WhiteboardDto"/> instance into a <see cref="Whiteboard"/> domain entity.
    /// </summary>
    /// <param name="dto">The data transfer object to convert.</param>
    /// <returns>A new instance of <see cref="Whiteboard"/> populated with values from the DTO.</returns>
    public override Whiteboard ToEntity(WhiteboardDto dto)
    {
        return new Whiteboard(
            Colors.Create(dto.MarkerColor),
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
    /// Converts a Whiteboard domain entity into a corresponding WhiteboardDto object.
    /// </summary>
    /// <param name="whiteboard">The Whiteboard domain entity to be converted.</param>
    /// <returns>A WhiteboardDto object that represents the provided Whiteboard entity.</returns>
    public override WhiteboardDto ToDto(Whiteboard whiteboard)
    {
        return new WhiteboardDto
        {
            Id = whiteboard.ComponentId,
            Orientation = whiteboard.Orientation.Value,
            Dimensions = new DimensionsDto
            {
                Width = whiteboard.Dimensions.Width,
                Length = whiteboard.Dimensions.Length,
                Height = whiteboard.Dimensions.Height
            },
            Position = new PositionDto
            {
                X = whiteboard.Position.X,
                Y = whiteboard.Position.Y,
                Z = whiteboard.Position.Z
            },
            MarkerColor = whiteboard.MarkerColor!.Value,
        };
    }
}
