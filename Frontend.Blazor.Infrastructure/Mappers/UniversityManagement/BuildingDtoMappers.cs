using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping methods to convert Building-related DTOs into domain entities and vice versa.
/// </summary>
internal static class BuildingDtoMappers
{
    /// <summary>
    /// Maps a <see cref="Building"/> entity to an <see cref="AddBuildingDto"/> used for creating a building via POST requests.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity to convert.</param>
    /// <returns>An <see cref="AddBuildingDto"/> populated with data from the building entity.</returns>
    internal static AddBuildingDto ToDto(Building building)
    {
        return new AddBuildingDto
        {
            Name = building!.Name!.Name,
            X = building!.BuildingCoordinates!.X,
            Y = building!.BuildingCoordinates!.Y,
            Z = building!.BuildingCoordinates!.Z,
            Width = building!.Dimensions!.Width,
            Length = building!.Dimensions!.Length,
            Height = building!.Dimensions!.Height,
            Color = building!.Color!.Value,
            Area = new AddBuildingAreaDto
            {
                Name = building.AreaName!.Name
            }
        };
    }

    /// <summary>
    /// Maps an <see cref="AddBuildingDto"/> to a <see cref="Building"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="AddBuildingDto"/> containing data to build a new entity.</param>
    /// <param name="area">The <see cref="Area"/> entity to which the building belongs.</param>
    /// <returns>A <see cref="Building"/> entity initialized with data from the DTO and area.</returns>
    internal static Building ToEntity(AddBuildingDto dto, Area area)
    {
        return new Building(
            EntityName.Create(dto.Name),
            Coordinates.Create(dto.X, dto.Y, dto.Z),
            Dimension.Create(dto.Width, dto.Length, dto.Height),
            Colors.Create(dto.Color!),
            area.Name
        );
    }

    /// <summary>
    /// Maps a <see cref="ListBuildingDto"/> to a <see cref="Building"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="ListBuildingDto"/> containing building data.</param>
    /// <param name="area">The <see cref="Area"/> entity the building is associated with.</param>
    /// <returns>A <see cref="Building"/> entity created from the DTO and associated area.</returns>
    internal static Building ToEntity(ListBuildingDto dto, Area area)
    {
        return new Building(
            buildingInternalId: dto.Id,
            name: EntityName.Create(dto.Name),
            coordinates: Coordinates.Create(dto.X, dto.Y, dto.Z),
            dimensions: Dimension.Create(dto.Width, dto.Length, dto.Height),
            color: Colors.Create(dto.Color!),
            area: area
        );
    }

    /// <summary>
    /// Maps a <see cref="ListBuildingDto"/> to a <see cref="Building"/> entity,
    /// including the creation of related <see cref="Area"/>, <see cref="Campus"/>, and <see cref="University"/> entities.
    /// </summary>
    /// <param name="dto">The <see cref="ListBuildingDto"/> containing nested area, campus, and university data.</param>
    /// <returns>A fully initialized <see cref="Building"/> entity including all related domain structures.</returns>
    internal static Building ToEntity(ListBuildingDto dto)
    {
        var university = UniversityDtoMappers.ToEntity(dto.Area!.Campus!.University!);
        var campus = new Campus(
            name: EntityName.Create(dto.Area.Campus.Name),
            location: EntityLocation.Create(dto.Area.Campus.Location),
            university: university
        );
        var area = new Area(
            name: EntityName.Create(dto.Area.Name),
            campus: campus
        );

        return new Building(
            buildingInternalId: dto.Id,
            name: EntityName.Create(dto.Name),
            coordinates: Coordinates.Create(dto.X, dto.Y, dto.Z),
            dimensions: Dimension.Create(dto.Width, dto.Length, dto.Height),
            color: Colors.Create(dto.Color!),
            area: area
        );
    }

    /// <summary>
    /// Maps a collection of <see cref="ListBuildingDto"/> objects to a collection of <see cref="Building"/> entities.
    /// </summary>
    /// <param name="dtos">The collection of <see cref="ListBuildingDto"/> instances.</param>
    /// <returns>An <see cref="IEnumerable{Building}"/> containing mapped building entities.</returns>
    internal static IEnumerable<Building> ToEntities(IEnumerable<ListBuildingDto> dtos)
    {
        return dtos.Select(ToEntity);
    }
}
