namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Entity representing the relationship between a user and a role.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Gets or sets the unique identifier for the user role relationship.
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Gets or sets the associated user entity.
    /// </summary>
    public User User { get; set; } = null!;
    /// <summary>
    /// Gets or sets the unique identifier for the role associated with the user.
    /// </summary>
    public int RoleId { get; set; }
    /// <summary>
    /// Gets or sets the associated role entity.
    /// </summary>
    public Role Role { get; set; } = null!;

    /// <summary>
    /// Constructor for the UserRole entity
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    /// <summary>
    /// Private parameterless constructor to the entity
    /// </summary>
    protected UserRole()
    {
        User = null!;
        Role = null!;
        UserId = default;
        RoleId = default;
    }
}
