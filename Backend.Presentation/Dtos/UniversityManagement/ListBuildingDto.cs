namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

/// <summary>
/// Data Transfer Object for Building entity.
/// </summary>
public record class ListBuildingDto(
    int Id,
    string Name,
    double X,
    double Y,
    double Z,
    double Width,
    double Length,
    double Height,
    string Color,
    ListAreaDto Area
);
