namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a role within the system, which can be assigned to users and associated with permissions.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of user-role relationships associated with this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Gets or sets the collection of role-permission relationships associated with this role.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public Role(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Protected parameterless constructor for ORM and serialization purposes.
    /// </summary>
    protected Role()
    {
        Id = default;
        Name = null!;
    }

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
