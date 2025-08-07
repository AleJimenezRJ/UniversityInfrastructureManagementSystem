using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;


/// <summary>
/// Represents a user account in the system, associated with a person entity.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int Id { get; set; } = 0;

    /// <summary>
    /// Gets or sets the username for the user account.
    /// </summary>
    public UserName UserName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated person entity.
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the associated person entity.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of roles associated with the user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with the specified values.
    /// </summary>
    /// <param name="id">The unique identifier for the user.</param>
    /// <param name="userName">The username for the user account.</param>
    /// <param name="personId">The identifier of the associated person entity.</param>
    public User(UserName userName, int personId, int id = 0)
    {
        Id = id;
        PersonId = personId;
        UserName = userName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// Private parameterless constructor for ORM and serialization purposes.
    /// </summary>
    protected User()
    {
        UserName = null!;
    }

}
