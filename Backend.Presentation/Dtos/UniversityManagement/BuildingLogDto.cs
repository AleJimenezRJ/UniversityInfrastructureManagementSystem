namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;


/// <summary>
/// Data Transfer Object that represents a historical log entry for a building.
/// Contains positional, dimensional, and metadata information about changes made to a building.
/// </summary>
/// <param name="BuildingLogInternalId">Unique internal identifier for the log entry.</param>
/// <param name="Name">Name of the building at the time of the logged change.</param>
/// <param name="X">X coordinate of the building.</param>
/// <param name="Y">Y coordinate of the building.</param>
/// <param name="Z">Z coordinate of the building.</param>
/// <param name="Width">Width of the building.</param>
/// <param name="Length">Length of the building.</param>
/// <param name="Height">Height of the building.</param>
/// <param name="Color">Color of the building.</param>
/// <param name="AreaName">Name of the area where the building is located.</param>
/// <param name="ModifiedAt">Timestamp indicating when the modification or action took place.</param>
/// <param name="Action">Type of action performed (e.g., "Created", "Modified", "Deleted").</param>
public record class BuildingLogDto(
    int BuildingLogInternalId,
    string Name,
    decimal X,
    decimal Y,
    decimal Z,
    decimal Width,
    decimal Length,
    decimal Height,
    string Color,
    string AreaName,
    DateTime ModifiedAt,
    string Action
  );
