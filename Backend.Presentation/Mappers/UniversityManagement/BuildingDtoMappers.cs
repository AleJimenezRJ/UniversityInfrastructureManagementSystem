using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping functions between <see cref="Building"/> domain entities and <see cref="ListBuildingDto"/> data transfer objects.
/// </summary>
internal static class BuildingDtoMappers
{
    /// <summary>
    /// Converts a <see cref="Building"/> domain entity to a <see cref="ListBuildingDto"/>.
    /// </summary>
    internal static ListBuildingDto ToDto(Building entity)
    {
        return new ListBuildingDto(
            entity.BuildingInternalId,
            entity.Name!.Name,
            entity.BuildingCoordinates!.X,
            entity.BuildingCoordinates.Y,
            entity.BuildingCoordinates.Z,
            entity.Dimensions!.Width,
            entity.Dimensions.Length,
            entity.Dimensions.Height,
            entity.Color!.Value,
            AreaDtoMappers.ToDto(entity.Area)
        );
    }

    internal static AddBuildingDto ToSecondDto(Building entity)
    {
        return new AddBuildingDto(
            entity.Name!.Name,
            entity.BuildingCoordinates!.X,
            entity.BuildingCoordinates.Y,
            entity.BuildingCoordinates.Z,
            entity.Dimensions!.Width,
            entity.Dimensions.Length,
            entity.Dimensions.Height,
            entity.Color!.Value,
            AreaDtoMappers.ToSecondDto(entity.Area)
        );
    }

    /// <summary>
    /// Converts a <see cref="ListBuildingDto"/> to a <see cref="Building"/> domain entity. 
    /// This is used for when the ID is used by the service to identify the building.
    /// </summary>
    /// <param name="dto">The data transfer object containing all the building information.</param>
    /// <returns>
    /// A <see cref="Building"/> instance created from the values in <paramref name="dto"/>.
    /// </returns>
    internal static Building ToEntity(ListBuildingDto dto, Area area)
    {
        var errors = new List<ValidationError>();

        if (!EntityName.TryCreate(dto.Name, out var name))
            errors.Add(new ValidationError("Name", "The building name is invalid."));

        if (!Coordinates.TryCreate(dto.X, dto.Y, dto.Z, out var coordinates))
            errors.Add(new ValidationError("Coordinates", "The building coordinates are invalid."));

        if (!Dimension.TryCreate(dto.Width, dto.Length, dto.Height, out var dimensions))
            errors.Add(new ValidationError("Dimensions", "The building dimensions are invalid."));

        if (!Colors.TryCreate(dto.Color, out var color))
            errors.Add(new ValidationError("Color", "The color is invalid."));

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new Building(name!, coordinates!, dimensions!, color!, area);
    }

    /// <summary>
    /// Converts a <see cref="AddBuildingDto"/> to a <see cref="Building"/> domain entity. 
    /// This is used for when the ID is auto-generated and we don't want the user inputting IDs or trying to edit them.
    /// </summary>
    /// <param name="dto">The data transfer object containing building information WITHOUT the ID, which is auto-generated.</param>
    /// <returns>
    /// A <see cref="Building"/> instance created from the values in <paramref name="dto"/>.
    /// </returns>
    internal static Building ToEntity(AddBuildingDto dto, Area area)
    {
        var errors = new List<ValidationError>();

        if (!EntityName.TryCreate(dto.Name, out var name))
            errors.Add(new ValidationError("Name", "The building name is invalid."));

        if (!Coordinates.TryCreate(dto.X, dto.Y, dto.Z, out var coordinates))
            errors.Add(new ValidationError("Coordinates", "The building coordinates are invalid."));

        if (!Dimension.TryCreate(dto.Width, dto.Length, dto.Height, out var dimensions))
            errors.Add(new ValidationError("Dimensions", "The building dimensions are invalid."));

        if (!Colors.TryCreate(dto.Color, out var color))
            errors.Add(new ValidationError("Color", "The color is invalid."));

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new Building(name!, coordinates!, dimensions!, color!, area);
    }
}
