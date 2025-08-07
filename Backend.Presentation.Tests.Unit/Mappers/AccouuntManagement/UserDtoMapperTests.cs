using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="UserDtoMapper"/> class, ensuring correct mapping between user entities and
/// DTOs.
/// </summary>
public class UserDtoMapperTests
{
    /// <summary>
    /// Represents a sample user entity for testing purposes.
    /// </summary>
    private static readonly User SampleEntity = new(
        UserName.Create("testuser"),
        personId: 1001
    )
    {
        Id = 10
    };

    /// <summary>
    /// Represents a sample data transfer object for creating a user.
    /// </summary>
    private static readonly CreateUserDto SampleCreateDto = new(
        UserName: "testuser",
        PersonId: 1001
    );

    /// <summary>
    /// Represents a sample data transfer object for modifying user information.
    /// </summary>
    private static readonly ModifyUserDto SampleModifyDto = new(
        UserName: "updateduser"
    );

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDto"/> method correctly maps the <c>Id</c> property from the source
    /// entity to the resulting DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapIdCorrectly()
    {
        var dto = UserDtoMapper.ToDto(SampleEntity);
        dto.Id.Should().Be(SampleEntity.Id);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDto"/> method correctly maps the <c>UserName</c> property from the
    /// source entity to the DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapUserNameCorrectly()
    {
        var dto = UserDtoMapper.ToDto(SampleEntity);
        dto.UserName.Should().Be(SampleEntity.UserName.Value);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDto"/> method correctly maps the <c>PersonId</c> from the source
    /// entity to the DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapPersonIdCorrectly()
    {
        var dto = UserDtoMapper.ToDto(SampleEntity);
        dto.personId.Should().Be(SampleEntity.PersonId);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToEntity"/> method correctly maps the <c>UserName</c> property from a
    /// <c>UserCreateDto</c> to a <c>UserEntity</c>.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapUserNameCorrectly()
    {
        var entity = UserDtoMapper.ToEntity(SampleCreateDto);
        entity.UserName.Value.Should().Be(SampleCreateDto.UserName);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToEntity"/> method correctly maps the <c>PersonId</c> from the DTO to
    /// the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonIdCorrectly()
    {
        var entity = UserDtoMapper.ToEntity(SampleCreateDto);
        entity.PersonId.Should().Be(SampleCreateDto.PersonId);
    }

    /// <summary>
    /// Verifies that the <see cref="UserDtoMapper.ToDtoList"/> method returns a list of DTOs with the correct count.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var entities = new List<User>
        {
            SampleEntity,
            new User(UserName.Create("anotheruser"), 1002) { Id = 20 }
        };

        var dtos = UserDtoMapper.ToDtoList(entities);

        dtos.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDtoList"/> method correctly maps the ID of the first user.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstUserIdCorrectly()
    {
        var dtos = UserDtoMapper.ToDtoList(new List<User> { SampleEntity });
        dtos[0].Id.Should().Be(10);
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDtoList"/> method correctly maps the first user's name.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstUserNameCorrectly()
    {
        var dtos = UserDtoMapper.ToDtoList(new List<User> { SampleEntity });
        dtos[0].UserName.Should().Be("testuser");
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDtoList"/> method correctly maps the first user's person ID.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstPersonIdCorrectly()
    {
        var dtos = UserDtoMapper.ToDtoList(new List<User> { SampleEntity });
        dtos[0].personId.Should().Be(1001);
    }

    /// <summary>
    /// Tests that <see cref="UserDtoMapper.ToDtoList"/> returns an empty list when the input list is empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var dtos = UserDtoMapper.ToDtoList(new List<User>());
        dtos.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.UpdateEntity"/> method updates the <see cref="User.UserName"/> property
    /// of a <see cref="User"/> entity when the provided <see cref="ModifyUserDto"/> contains a new user name.
    /// </summary>
    [Fact]
    public void UpdateEntity_ShouldUpdateUserName_WhenModifyUserDtoHasNewUserName()
    {
        var entity = new User(UserName.Create("olduser"), 2001) { Id = 30 };

        UserDtoMapper.UpdateEntity(entity, SampleModifyDto);

        entity.UserName.Value.Should().Be("updateduser");
    }

    /// <summary>
    /// Tests that the <see cref="UserDtoMapper.ToDto"/> method throws a <see cref="ValidationException"/> when the <see
    /// cref="User"/> entity has a null user name.
    /// </summary>
    [Fact]
    public void ToDto_ShouldThrowValidationException_WhenUserNameIsNull()
    {
        var invalidEntity = new User(null!, 9999) { Id = 40 };

        Action act = () => UserDtoMapper.ToDto(invalidEntity);

        act.Should().Throw<ValidationException>()
            .WithMessage("The Users entity cannot have null attributes.");
    }
}
