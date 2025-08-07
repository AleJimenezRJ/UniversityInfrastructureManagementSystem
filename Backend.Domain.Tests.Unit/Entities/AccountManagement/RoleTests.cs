// Participantes: Isabella Rodriguez Sanchez y Gael Alpizar Alfaro
// User Story: SPT-RM-003-002 Delete Role #122
// User Story: SPT-RM-003-003 List All Roles #164
// Tarea técnica: Write unit and integration tests

using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="Role"/> entity.
/// </summary>
public class RoleTests
{
    /// <summary>
    /// The name used for testing the Role constructor.
    /// </summary>
    private const string RoleName = "TestRole";

    /// <summary>
    /// Tests the constructor of <see cref="Role"/> to ensure it assigns the Name property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithName_AssignsNameCorrectly()
    {
        // Arrange

        // Act
        // CUT: Component under test
        var role = new Role(RoleName);

        // Assert
        role.Name.Should().Be(RoleName,
            because: "the constructor should correctly assign the Name property");
    }

    /// <summary>
    /// Tests the constructor of <see cref="Role"/> to ensure it initializes UserRoles as an empty list.
    /// </summary>
    [Fact]
    public void Constructor_WithName_InitializesUserRolesAsEmpty()
    {
        // Arrange

        // Act
        // CUT: Component under test
        var role = new Role(RoleName);

        // Assert
        role.UserRoles.Should().BeEmpty(
            because: "the constructor should initialize UserRoles as an empty list");
    }

    /// <summary>
    /// Tests the constructor of <see cref="Role"/> to ensure it initializes RolePermissions as an empty list.
    /// </summary>
    [Fact]
    public void Constructor_WithName_InitializesRolePermissionsAsEmpty()
    {
        // Arrange

        // Act
        // CUT: Component under test
        var role = new Role(RoleName);

        // Assert
        role.RolePermissions.Should().BeEmpty(
            because: "the constructor should initialize RolePermissions as an empty list");
    }

    /// <summary>
    /// Tests the parameterless constructor of <see cref="Role"/> to ensure it sets the Id property to default value (0).
    /// </summary>
    [Fact]
    public void ParameterlessConstructor_SetsIdToDefault()
    {
        // Arrange
        var role = (Role)Activator.CreateInstance(typeof(Role), nonPublic: true)!;

        // Assert
        role.Id.Should().Be(0, because: "default constructor should assign default value 0 to Id");
    }

    /// <summary>
    /// Tests the parameterless constructor of <see cref="Role"/> to ensure it sets the Name property to null.
    /// </summary>
    [Fact]
    public void ParameterlessConstructor_SetsNameToNull()
    {
        // Arrange
        var role = (Role)Activator.CreateInstance(typeof(Role), nonPublic: true)!;

        // Assert
        role.Name.Should().BeNull(because: "reference type properties should be null by default");
    }

    /// <summary>
    /// Tests the parameterless constructor of <see cref="Role"/> to ensure it initializes UserRoles as an empty collection.
    /// </summary>
    [Fact]
    public void ParameterlessConstructor_InitializesUserRolesAsEmpty()
    {
        // Arrange
        var role = (Role)Activator.CreateInstance(typeof(Role), nonPublic: true)!;

        // Assert
        role.UserRoles.Should().BeEmpty(because: "collections should be initialized as empty");
    }

    /// <summary>
    /// Tests the parameterless constructor of <see cref="Role"/> to ensure it initializes RolePermissions as an empty collection.
    /// </summary>
    [Fact]
    public void ParameterlessConstructor_InitializesRolePermissionsAsEmpty()
    {
        // Arrange
        var role = (Role)Activator.CreateInstance(typeof(Role), nonPublic: true)!;

        // Assert
        role.RolePermissions.Should().BeEmpty(because: "collections should be initialized as empty");
    }
}
