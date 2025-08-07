namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

/// <summary>
/// Data Transfer Object for the Area entity. It only contains the name of the area and the reference to the campus it belongs to.
/// </summary>
public record class AddAreaDto(
    string Name,
    AddAreaCampusDto Campus // only the name of the campus
);
