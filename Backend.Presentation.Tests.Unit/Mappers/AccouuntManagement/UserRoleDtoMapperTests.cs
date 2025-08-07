using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="UserRoleDtoMapper"/> class, ensuring correct mapping between <see
/// cref="UserRole"/> entities and <see cref="UserRoleDto"/> data transfer objects.
/// </summary>
public class UserRoleDtoMapperTests
{
    private static readonly UserRole ExampleEntity = new(userId: 1, roleId: 10);
    private static readonly UserRoleDto ExampleDto = new(1, 10);

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToDto"/> method correctly maps the <c>UserId</c> property from the
    /// source entity to the DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapUserIdCorrectly()
    {
        var dto = UserRoleDtoMapper.ToDto(ExampleEntity);

        dto.UserId.Should().Be(ExampleEntity.UserId);
    }

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToDto"/> method correctly maps the <c>RoleId</c> property from the
    /// entity to the DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapRoleIdCorrectly()
    {
        var dto = UserRoleDtoMapper.ToDto(ExampleEntity);

        dto.RoleId.Should().Be(ExampleEntity.RoleId);
    }

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToEntity"/> method correctly maps the <c>UserId</c> property from
    /// the DTO to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapUserIdCorrectly()
    {
        var entity = UserRoleDtoMapper.ToEntity(ExampleDto);

        entity.UserId.Should().Be(ExampleDto.UserId);
    }

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToEntity"/> method correctly maps the <c>RoleId</c> from the DTO to
    /// the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapRoleIdCorrectly()
    {
        var entity = UserRoleDtoMapper.ToEntity(ExampleDto);

        entity.RoleId.Should().Be(ExampleDto.RoleId);
    }

    /// <summary>
    /// Verifies that the <see cref="UserRoleDtoMapper.ToDtoList"/> method returns a list of DTOs with the correct
    /// count.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var entities = new List<UserRole>
        {
            new(userId: 1, roleId: 10),
            new(userId: 2, roleId: 20),
            new(userId: 3, roleId: 30)
        };

        var dtos = UserRoleDtoMapper.ToDtoList(entities);

        dtos.Should().HaveCount(3);
    }

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToDtoList"/> method correctly maps the user ID of the first <see
    /// cref="UserRole"/> to the resulting DTO.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstUserIdCorrectly()
    {
        var dtos = UserRoleDtoMapper.ToDtoList(new List<UserRole>
        {
            new(userId: 1, roleId: 10)
        });

        dtos[0].UserId.Should().Be(1);
    }

    /// <summary>
    /// Tests that the <see cref="UserRoleDtoMapper.ToDtoList"/> method correctly maps the first role ID from a list of
    /// <see cref="UserRole"/> objects.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstRoleIdCorrectly()
    {
        var dtos = UserRoleDtoMapper.ToDtoList(new List<UserRole>
        {
            new(userId: 1, roleId: 10)
        });

        dtos[0].RoleId.Should().Be(10);
    }

    /// <summary>
    /// Tests that <see cref="UserRoleDtoMapper.ToDtoList"/> returns an empty list when the input list is empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var dtos = UserRoleDtoMapper.ToDtoList(new List<UserRole>());

        dtos.Should().BeEmpty();
    }
}
