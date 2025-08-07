using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Blazor.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Blazor.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="RoleDtoMapper"/> class, verifying the correct mapping between <see
/// cref="Role"/> and <see cref="RoleDto"/> objects.
/// </summary>
public class RoleDtoMapperTests
{
    private static readonly Role SampleRole = new Role("Administrator") { Id = 1 };
    private static readonly RoleDto SampleDto = new(1, "Administrator");

    /// <summary>
    /// Tests that the <see cref="RoleDtoMapper.ToDto"/> method correctly maps the <c>Id</c> property from a <c>Role</c>
    /// object to a <c>RoleDto</c> object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapIdCorrectly()
    {
        var dto = RoleDtoMapper.ToDto(SampleRole);
        dto.Id.Should().Be(SampleRole.Id);
    }

    /// <summary>
    /// Tests that the <see cref="RoleDtoMapper.ToDto"/> method correctly maps the <c>Name</c> property from a
    /// <c>Role</c> object to a <c>RoleDto</c> object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapNameCorrectly()
    {
        var dto = RoleDtoMapper.ToDto(SampleRole);
        dto.Name.Should().Be(SampleRole.Name);
    }

    /// <summary>
    /// Verifies that the <see cref="RoleDtoMapper.ToDtoList"/> method returns a list of DTOs with the correct count.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var roles = new List<Role>
        {
            new Role("Administrator") { Id = 1 },
            new Role("Editor") { Id = 2 },
            new Role("Viewer") { Id = 3 }
        };

        var dtos = RoleDtoMapper.ToDtoList(roles);

        dtos.Should().HaveCount(3);
    }

    /// <summary>
    /// Tests that the <see cref="RoleDtoMapper.ToDtoList"/> method correctly maps the first role's ID.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstRoleIdCorrectly()
    {
        var roles = new List<Role> { SampleRole };
        var dtos = RoleDtoMapper.ToDtoList(roles);

        dtos[0].Id.Should().Be(1);
    }

    /// <summary>
    /// Tests that the <see cref="RoleDtoMapper.ToDtoList"/> method correctly maps the first role's name to
    /// "Administrator".
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstRoleNameCorrectly()
    {
        var roles = new List<Role> { SampleRole };
        var dtos = RoleDtoMapper.ToDtoList(roles);

        dtos[0].Name.Should().Be("Administrator");
    }

    /// <summary>
    /// Verifies that the <see cref="RoleDtoMapper.ToDtoList"/> method returns an empty list when the input list is
    /// empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var emptyRoles = new List<Role>();
        var dtos = RoleDtoMapper.ToDtoList(emptyRoles);

        dtos.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that the <see cref="SampleDto.ToEntity"/> method correctly maps the <c>Id</c> property from the DTO to the
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapIdCorrectly()
    {
        var entity = SampleDto.ToEntity();
        entity.Id.Should().Be(SampleDto.Id);
    }

    /// <summary>
    /// Tests that the <see cref="SampleDto.ToEntity"/> method correctly maps the <c>Name</c> property.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapNameCorrectly()
    {
        var entity = SampleDto.ToEntity();
        entity.Name.Should().Be(SampleDto.Name);
    }
}
