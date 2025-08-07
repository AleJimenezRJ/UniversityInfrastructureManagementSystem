namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

/// <summary>
/// This class represents the Administrative Unit entity.
/// </summary>
public class AdministrativeUnit
{
    public string Name { get; }

    /// <summary>
    /// <param name="type">Type should be: Faculty, School, etc.</param>
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Initializes a new instance of Administrative Unit class with specified properties.
    /// <param name="name">Unique name (ID) for the Administrative Unit</param>
    /// <param name="type"></param>
    /// </summary>
    public AdministrativeUnit(string name, string type)
    {
        Name = name;
        Type = type;
    }

}
