using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="UserWithPersonDtoMapper"/> class, specifically testing the mapping
/// functionality from <see cref="CreateUserWithPersonDto"/> to the corresponding entity.
/// </summary>
public class UserWithPersonDtoMapperTests
{
    /// <summary>
    /// Represents a valid data transfer object for creating a user with associated personal information.
    /// </summary>
    private static readonly CreateUserWithPersonDto ValidDto = new(
        UserName: "testuser",
        Email: "test.user@example.com",
        FirstName: "Test",
        LastName: "User",
        Phone: "7000-1234",
        BirthDate: new DateOnly(1990, 1, 1),
        IdentityNumber: "1-1111-1111",
        Roles: new List<string> { "Editor", "Viewer" }
    );

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps the user name from the DTO
    /// to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapUserNameCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.UserName.Value.Should().Be("testuser");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps the email address from the
    /// DTO to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapEmailCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.Email.Value.Should().Be("test.user@example.com");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps the phone number from the
    /// DTO to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPhoneCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.Phone.Value.Should().Be("7000-1234");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps the identity number from the
    /// DTO to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapIdentityNumberCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.IdentityNumber.Value.Should().Be("1-1111-1111");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps the birth date from the DTO
    /// to the entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapBirthDateCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.BirthDate.Value.Should().Be(new DateOnly(1990, 1, 1));
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method correctly maps roles from the DTO to the
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapRolesCorrectly()
    {
        var entity = UserWithPersonDtoMapper.ToEntity(ValidDto);

        entity.Roles.Should().Contain("Editor").And.Contain("Viewer");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method throws a <see cref="ValidationException"/>
    /// when provided with invalid value objects.
    /// </summary>
    /// <param name="userName">The user name to test, which may be invalid.</param>
    /// <param name="email">The email address to test, which may be invalid.</param>
    /// <param name="phone">The phone number to test, which may be invalid.</param>
    /// <param name="identity">The identity number to test, which may be invalid.</param>
    /// <param name="expectedMessage">The expected exception message indicating the validation error.</param>
    [Theory]
    [InlineData("", "test.user@example.com", "7000-1234", "1-1111-1111", "Invalid UserName format")]
    [InlineData("testuser", "invalidemail", "7000-1234", "1-1111-1111", "Invalid email format")]
    [InlineData("testuser", "test.user@example.com", "70001234", "1-1111-1111", "Invalid phone number")]
    [InlineData("testuser", "test.user@example.com", "7000-1234", "12345678", "The identity number format is invalid")]
    public void ToEntity_ShouldThrowValidationException_OnInvalidValueObjects(
        string userName, string email, string phone, string identity, string expectedMessage)
    {
        var invalidDto = ValidDto with
        {
            UserName = userName,
            Email = email,
            Phone = phone,
            IdentityNumber = identity
        };

        var act = () => UserWithPersonDtoMapper.ToEntity(invalidDto);

        act.Should().Throw<ValidationException>()
           .WithMessage("*" + expectedMessage + "*");
    }

    /// <summary>
    /// Tests that the <see cref="UserWithPersonDtoMapper.ToEntity"/> method sets the <c>Roles</c> property to an empty
    /// collection when the <c>Roles</c> property of the DTO is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldSetEmptyRoles_WhenRolesIsNull()
    {
        var dtoWithoutRoles = ValidDto with { Roles = null };

        var entity = UserWithPersonDtoMapper.ToEntity(dtoWithoutRoles);

        entity.Roles.Should().NotBeNull();
        entity.Roles.Should().BeEmpty();
    }
}
