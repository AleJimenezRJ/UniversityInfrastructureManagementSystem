namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

/// <summary>
/// Data Transfer Object for Building entity without the ID parameter.
/// </summary>
public record class AddBuildingDto(
    string Name,
    double X,
    double Y,
    double Z,
    double Width,
    double Length,
    double Height,
    string Color,
    AddBuildingAreaDto Area
);
