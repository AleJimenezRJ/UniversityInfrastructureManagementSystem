namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

/// <summary>
/// Data Transfer Object for University including its name and country.
public record class UniversityDto(
    string Name,
    string Country
);
