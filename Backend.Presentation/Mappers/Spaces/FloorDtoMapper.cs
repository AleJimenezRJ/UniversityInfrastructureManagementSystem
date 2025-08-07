using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

/// <summary>
/// Mapper class for converting between Floor entities and Floor DTOs./// </summary>

internal static class FloorDtoMapper
{
    /// <summary>
    /// Converts a Floor entity to a Floor DTO.
    /// </summary>
    /// <param name="floorEntity">Object of floor entity</param>
    /// <returns>A floor Dto</returns>
    internal static FloorDto ToDto(Floor floorEntity)
    {
        return new FloorDto(floorEntity.FloorId, floorEntity.Number.Value);
    }

    /// <summary>
    /// Converts a Floor DTO to a Floor entity.
    /// </summary>
    /// <param name="dto">Dto of floor</param>
    /// <returns>A floor entity</returns>
    internal static Floor ToEntity(FloorDto dto)
    {
        var errors = new List<ValidationError>();

        if (!FloorNumber.TryCreate(dto.FloorNumber, out var validFloorNumber))
            errors.Add(new ValidationError("FloorNumber", "The FloorNumber must be an integer greater than zero."));

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new Floor(validFloorNumber!);

    }
}
