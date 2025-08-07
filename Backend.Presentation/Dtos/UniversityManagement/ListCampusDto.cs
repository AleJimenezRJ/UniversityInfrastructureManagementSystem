namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

/// <summary>
/// Data Transfer Object for the Campus entity with its name and location attributes.
/// </summary>
public record class ListCampusDto(
    string Name,
    string Location,
    UniversityDto University
);
