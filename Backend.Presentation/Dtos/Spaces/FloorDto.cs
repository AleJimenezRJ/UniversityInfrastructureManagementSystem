namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a floor.
/// </summary>
/// <param name="FloorId">
/// The unique identifier of floor.
/// </param>
/// <param name="FloorNumber">
/// The number of the floor.
/// </param>
public record class FloorDto(
    int FloorId,
    int FloorNumber
);
