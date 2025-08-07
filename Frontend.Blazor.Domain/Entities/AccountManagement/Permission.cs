namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a permission that can be assigned to roles within the system.
/// Each permission has a unique identifier and a description.
/// Permissions are associated with roles through the RolePermission entity.
/// </summary>
public class Permission
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    public int Id { get; set; } = 0;

    /// <summary>
    /// Gets or sets the description of the permission.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Navigation property for the collection of RolePermission entities
    /// that associate this permission with roles.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Permission"/> class with the specified id and description.
    /// </summary>
    /// <param name="description">The description of the permission.</param>
    /// <param name="id">The unique identifier for the permission.</param>
    public Permission(string description, int id = 0)
    {
        Id = id;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Permission"/> class.
    /// This constructor is intended for use by Entity Framework.
    /// </summary>
    public Permission()
    {
        Id = default;
        Description = null!;
    }
}
