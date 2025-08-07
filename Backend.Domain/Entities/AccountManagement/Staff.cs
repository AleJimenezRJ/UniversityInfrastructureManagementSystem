using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a staff member in the system.
/// </summary>
public class Staff
{
    /// <summary>
    /// Gets or sets the unique identification number or code for the staff member.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the Person associated with this staff.
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the staff's institutional email address.
    /// </summary>
    public Email InstitutionalEmail { get; set; }

    /// <summary>
    /// Gets or sets the Person object associated with this staff.
    /// This property is initialized as non-null (using null-forgiving operator).
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the Staff class with the specified parameters.
    /// </summary>
    /// <param name="staffId"> The staff's unique identification number or code.</param>
    /// <param name="institutionalEmail"> The staff's institutional email address.</param>
    /// <param name="personId"> The unique identifier for the associated Person.</param>
    /// <param name="type"> The type of staff member (e.g., "Professor", "Administrator").</param>
    public Staff(Email institutionalEmail, int personId, string type)
    {
        InstitutionalEmail = institutionalEmail;
        PersonId = personId;
        Type = type;
    }

    /// <summary>
    /// Private parameterless constructor to the entity
    /// </summary>
    protected Staff()
    {
        InstitutionalEmail = null!;
        PersonId = 0;
        Type = null!;
    }
}