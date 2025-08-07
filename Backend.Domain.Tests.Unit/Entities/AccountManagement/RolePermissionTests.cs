// Participantes: Isabella Rodriguez Sanchez y Gael Alpizar Alfaro
// User Story: SPT-PA-004-001 View Permissions Assigned to Role #124
// Tarea técnica: Write unit and integration tests


using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

/// <summary>
/// A test class for the <see cref="RolePermission"/> entity.
/// </summary>
public class RolePermissionTests
{
    /// <summary>
    /// The role and permission instances used for testing.
    /// </summary>
    private readonly Role _role;

    /// <summary>
    /// The permission instance used for testing.
    /// </summary>
    private readonly Permission _permission;

    /// <summary>
    /// The <see cref="RolePermission"/> instance used for testing.
    /// </summary>
    private readonly RolePermission _rolePermission;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolePermissionTests"/> class.
    /// </summary>
    public RolePermissionTests()
    {
        _role = new Role("Admin");
        _permission = new Permission("ManageUsers");
        _rolePermission = new RolePermission(1, 100);
    }

    /// <summary>
    /// Tests the constructor of <see cref="RolePermission"/> to ensure it sets the RoleId correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidIds_SetsRoleId()
    {
        // Arrange
        var expectedRoleId = 1;
        var permissionId = 100;

        // Act
        var rolePermission = new RolePermission(expectedRoleId, permissionId);

        // Assert
        rolePermission.RoleId.Should().Be(expectedRoleId, "because the RoleId should be set in the constructor");
    }

    /// <summary>
    /// Tests the constructor of <see cref="RolePermission"/> to ensure it sets the PermissionId correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidIds_SetsPermissionId()
    {
        // Arrange
        var roleId = 1;
        var expectedPermissionId = 100;

        // Act
        var rolePermission = new RolePermission(roleId, expectedPermissionId);

        // Assert
        rolePermission.PermissionId.Should().Be(expectedPermissionId, "because the PermissionId should be set in the constructor");
    }

    /// <summary>
    /// Tests the Role property of <see cref="RolePermission"/> to ensure it can be assigned a Role instance.
    /// </summary>
    [Fact]
    public void RoleProperty_CanBeAssigned()
    {
        // Arrange

        // Act
        _rolePermission.Role = _role;

        // Assert
        _rolePermission.Role.Should().Be(_role, "because the Role reference should be correctly assigned");
    }

    /// <summary>
    /// Tests the Permission property of <see cref="RolePermission"/> to ensure it can be assigned a Permission instance.
    /// </summary>
    [Fact]
    public void PermissionProperty_CanBeAssigned()
    {
        // Arrange

        // Act
        _rolePermission.Permission = _permission;

        // Assert
        _rolePermission.Permission.Should().Be(_permission, "because the Permission reference should be correctly assigned");
    }

    /// <summary>
    /// Tests the default constructor of <see cref="RolePermission"/> to ensure it initializes Role and Permission to null.
    /// </summary>
    [Fact]
    public void DefaultConstructor_SetsRoleToNull()
    {
        // Arrange
        var rolePermission = (RolePermission?)Activator.CreateInstance(
            typeof(RolePermission),
            nonPublic:true);

        // Assert
        rolePermission?.Role.Should().BeNull(because: "the Role reference should be null by default");
    }

    /// <summary>
    /// Tests the default constructor of <see cref="RolePermission"/> to ensure it initializes Permission to null.
    /// </summary>
    [Fact]
    public void DefaultConstructor_SetsPermissionToNull()
    {
        // Arrange
        var rolePermission = (RolePermission?)Activator.CreateInstance(
            typeof(RolePermission),
            nonPublic: true);

        // Assert
        rolePermission?.Permission.Should().BeNull(because: "the Permission reference should be null by default");
    }
}