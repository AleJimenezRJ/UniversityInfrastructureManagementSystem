using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="PersonDtoMapper"/> class, verifying the correct mapping between <see
/// cref="Person"/> entities and <see cref="PersonDto"/> data transfer objects.
/// </summary>
public class PersonDtoMapperTests
{
    /// <summary>
    /// Represents a sample person with predefined attributes for testing or demonstration purposes.
    /// </summary>
    private static readonly Person SamplePerson = new(
        Email.Create("person.user@example.com"),
        "Person",
        "User",
        Phone.Create("7000-1234"),
        BirthDate.Create(new DateOnly(1995, 5, 15)),
        IdentityNumber.Create("1-2222-3333"),
        id: 101
    );

    /// <summary>
    /// Represents a sample data transfer object (DTO) for a person.
    /// </summary>
    private static readonly PersonDto SampleDto = new(
        Id: 101,
        Email: "person.user@example.com",
        FirstName: "Person",
        LastName: "User",
        Phone: "7000-1234",
        BirthDate: new DateOnly(1995, 5, 15),
        IdentityNumber: "1-2222-3333"
    );

    /// <summary>
    /// Represents a sample data transfer object for creating a new person.
    /// </summary>
    private static readonly CreatePersonDto SampleCreateDto = new(
        Email: "new.person@example.com",
        FirstName: "New",
        LastName: "Person",
        Phone: "8000-5678",
        BirthDate: new DateOnly(2000, 1, 1),
        IdentityNumber: "2-3333-4444"
    );

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the ID property from a <c>Person</c>
    /// object to a <c>PersonDto</c> object.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapId() => PersonDtoMapper.ToDto(SamplePerson).Id.Should().Be(101);
    
    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the email address from a <c>Person</c>
    /// object to a <c>PersonDto</c> object.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapEmail() => PersonDtoMapper.ToDto(SamplePerson).Email.Should().Be("person.user@example.com");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the first name of a <see
    /// cref="SamplePerson"/> to the <see cref="PersonDto.FirstName"/> property.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapFirstName() => PersonDtoMapper.ToDto(SamplePerson).FirstName.Should().Be("Person");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the last name of a <see
    /// cref="SamplePerson"/> to a <see cref="PersonDto"/>.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapLastName() => PersonDtoMapper.ToDto(SamplePerson).LastName.Should().Be("User");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the phone number from a <see
    /// cref="SamplePerson"/> to the resulting DTO.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapPhone() => PersonDtoMapper.ToDto(SamplePerson).Phone.Should().Be("7000-1234");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the <c>BirthDate</c> property.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapBirthDate() => PersonDtoMapper.ToDto(SamplePerson).BirthDate.Should().Be(new DateOnly(1995, 5, 15));

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method correctly maps the identity number from a
    /// <c>Person</c> object to a <c>PersonDto</c>.
    /// </summary>
    [Fact] 
    public void ToDto_ShouldMapIdentityNumber() => PersonDtoMapper.ToDto(SamplePerson).IdentityNumber.Should().Be("1-2222-3333");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the ID from the DTO to the entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapId() => PersonDtoMapper.ToEntity(SampleDto).Id.Should().Be(101);

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the email address from a DTO to an
    /// entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapEmail() => PersonDtoMapper.ToEntity(SampleDto).Email.Value.Should().Be("person.user@example.com");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the first name from a DTO to an
    /// entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapFirstName() => PersonDtoMapper.ToEntity(SampleDto).FirstName.Should().Be("Person");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the last name from a <see
    /// cref="SampleDto"/>.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapLastName() => PersonDtoMapper.ToEntity(SampleDto).LastName.Should().Be("User");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the phone number from a DTO to an
    /// entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapPhone() => PersonDtoMapper.ToEntity(SampleDto).Phone.Value.Should().Be("7000-1234");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the <c>BirthDate</c> property from
    /// the DTO to the entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapBirthDate() => PersonDtoMapper.ToEntity(SampleDto).BirthDate.Value.Should().Be(new DateOnly(1995, 5, 15));

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the identity number from the DTO to
    /// the entity.
    /// </summary>
    [Fact] 
    public void ToEntity_ShouldMapIdentityNumber() => PersonDtoMapper.ToEntity(SampleDto).IdentityNumber.Value.Should().Be("1-2222-3333");

    /// <summary>
    /// Verifies that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the ID from a create DTO to the
    /// entity.
    /// </summary>
    [Fact] 
    public void ToEntity_FromCreateDto_ShouldMapId() => PersonDtoMapper.ToEntity(SampleCreateDto).Id.Should().Be(0);

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToEntity"/> method correctly maps the email address from a create DTO
    /// to an entity.
    /// </summary>
    [Fact] 
    public void ToEntity_FromCreateDto_ShouldMapEmail() => PersonDtoMapper.ToEntity(SampleCreateDto).Email.Value.Should().Be("new.person@example.com");

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDtoList"/> method returns a list with the correct count of DTOs.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var people = new List<Person> { SamplePerson };
        PersonDtoMapper.ToDtoList(people).Should().HaveCount(1);
    }

    /// <summary>
    /// Verifies that the <see cref="PersonDtoMapper.ToDtoList"/> method correctly maps the ID of the first person in
    /// the list.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstPersonId()
    {
        var people = new List<Person> { SamplePerson };
        PersonDtoMapper.ToDtoList(people)[0].Id.Should().Be(101);
    }

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDtoList"/> method returns an empty list when the input list is
    /// empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        PersonDtoMapper.ToDtoList(new List<Person>()).Should().BeEmpty();
    }

    /// <summary>
    /// Tests that the <see cref="PersonDtoMapper.ToDto"/> method throws a <see cref="ValidationException"/> when the
    /// <see cref="Person"/> entity has null attributes.
    /// </summary>
    [Fact]
    public void ToDto_ShouldThrowValidationException_WhenPersonHasNullAttributes()
    {
        var invalidPerson = new Person(null!, "Test", "User", null!, null!, null!, id: 103);
        Action act = () => PersonDtoMapper.ToDto(invalidPerson);
        act.Should().Throw<ValidationException>()
           .WithMessage("The person entity cannot have null attributes.");
    }
}
