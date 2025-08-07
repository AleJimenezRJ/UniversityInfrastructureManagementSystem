using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="SqlPersonRepository"/> class.
/// </summary>
public class SqlPersonRepositoryTests
{
    /// <summary>
    /// Mock database context for testing the <see cref="SqlPersonRepository"/> class.
    /// </summary>
    private readonly Mock<ThemeParkDataBaseContext> _mockDbContext = new();

    /// <summary>
    /// Mock logger for the <see cref="SqlPersonRepository"/> class.
    /// </summary>
    private readonly Mock<ILogger<SqlPersonRepository>> _mockLogger = new();

    /// <summary>
    /// A helper method to create an instance of the <see cref="SqlPersonRepository"/> class with mocked dependencies.
    /// </summary>
    /// <returns> An instance of <see cref="SqlPersonRepository"/>.</returns>
    private SqlPersonRepository CreateRepository() =>
        new(_mockDbContext.Object, _mockLogger.Object);

    /// <summary>
    /// Tests the CreatePersonAsync method of the SqlPersonRepository to ensure it throws a DomainException
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreatePersonAsync_ShouldThrowDuplicatedEntityException_WhenEmailOrIdentityNumberAlreadyExists()
    {
        // Arrange
        var identity = IdentityNumber.Create("1-2345-6789");
        var email = Email.Create("john@ucr.ac.cr");

        var existing = new Person(email, "John", "Doe", Phone.Create("8888-8888"),
            BirthDate.Create(new DateOnly(1990, 1, 1)), identity, 1);
        var peopleList = new List<Person> { existing };
        var peopleDbSet = peopleList.AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(db => db.Persons).Returns(peopleDbSet.Object);
        var repository = CreateRepository();

        var newPerson = new Person(email, "Jane", "Doe", Phone.Create("8888-0000"),
            BirthDate.Create(new DateOnly(1995, 5, 5)), identity, 2);

        // Act
        Func<Task> act = async () => await repository.CreatePersonAsync(newPerson);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("*already exists*");
    }

    /// <summary>
    /// Tests the CreatePersonAsync method of the SqlPersonRepository to ensure it creates a new person successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        const string identityStr = "1-1111-1111";
        var identity = IdentityNumber.Create(identityStr);

        var expectedPerson = new Person(
            Email.Create("test@ucr.ac.cr"),
            "Test", "User",
            Phone.Create("8888-0000"),
            BirthDate.Create(new DateOnly(2000, 1, 1)),
            identity,
            10
        );

        var mockPersons = new List<Person> { expectedPerson }.AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(c => c.Persons).Returns(mockPersons.Object);

        var repository = new SqlPersonRepository(_mockDbContext.Object, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(identityStr);

        // Assert
        result.Should().NotBeNull();
    }

    /// <summary>
    /// Tests the GetByIdAsync method of the SqlPersonRepository to ensure it throws NotFoundException when the person does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task DeletePersonAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
    {
        // Arrange
        var people = new List<Person>().AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(db => db.Persons).Returns(people.Object);
        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.DeletePersonAsync("1-0000-0000");

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("*was not found*");
    }

    /// <summary>
    /// Tests the GetAllAsync method of the SqlPersonRepository to ensure it returns a list of people when they exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllAsync_ShouldReturnList_WhenPeopleExist()
    {
        // Arrange
        var person = new Person(Email.Create("a@ucr.ac.cr"), "A", "B", Phone.Create("2222-3333"), BirthDate.Create(new DateOnly(1995, 1, 1)), IdentityNumber.Create("1-5555-0000"), 5);
        var people = new List<Person> { person }.AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(db => db.Persons).Returns(people.Object);
        var repository = CreateRepository();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests the UpdatePersonAsync method of the SqlPersonRepository to ensure it updates a person's information successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task UpdatePersonAsync_ShouldThrowNotFoundException_WhenPersonNotFound()
    {
        // Arrange
        var people = new List<Person>().AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(db => db.Persons).Returns(people.Object);
        var repository = CreateRepository();

        var updated = new Person(Email.Create("new@ucr.ac.cr"), "New", "Person", Phone.Create("0000-0000"), BirthDate.Create(new DateOnly(1999, 9, 9)), IdentityNumber.Create("1-0000-0000"), 1);

        // Act
        Func<Task> act = async () => await repository.UpdatePersonAsync("1-0000-0000", updated);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

}
