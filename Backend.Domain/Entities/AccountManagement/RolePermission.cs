namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Represents the association between a <see cref="Role"/> and a <see cref="Permission"/>.
/// This entity is used to define which permissions are assigned to a specific role.
/// </summary>
public class RolePermission
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated permission.
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Gets or sets the associated <see cref="Role"/> entity.
    /// </summary>
    public Role Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets the associated <see cref="Permission"/> entity.
    /// </summary>
    public Permission Permission { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolePermission"/> class with the specified role and permission identifiers.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    /// <summary>
    /// Protected parameterless constructor for ORM and serialization purposes.
    /// </summary>
    protected RolePermission()
    {
        RoleId = default;
        PermissionId = default;
        Role = null!;
        Permission = null!;
    }
}
