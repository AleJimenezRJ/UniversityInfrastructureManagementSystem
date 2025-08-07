using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="SqlStaffRepository"/> class, which manages staff-related operations.
/// </summary>
public class SqlStaffRepositoryTests
{
    /// <summary>
    /// Mock database context for testing the <see cref="SqlStaffRepository"/> class.
    /// </summary>
    private readonly Mock<ThemeParkDataBaseContext> _mockDbContext = new();

    /// <summary>
    /// Mock logger for logging operations in the <see cref="SqlStaffRepository"/> class.
    /// </summary>
    private readonly Mock<ILogger<SqlStaffRepository>> _mockLogger = new();

    /// <summary>
    /// A private method to create an instance of the <see cref="SqlStaffRepository"/> class for testing purposes.
    /// </summary>
    /// <returns> An instance of the <see cref="SqlStaffRepository"/> class.</returns>
    private SqlStaffRepository CreateRepository() =>
        new(_mockDbContext.Object, _mockLogger.Object);

    /// <summary>
    /// Tests the <see cref="SqlStaffRepository.CreateStaffAsync"/> method to ensure it throws a
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStaffAsync_ShouldThrowDuplicatedEntityException_WhenPersonOrStaffExists()
    {
        // Arrange
        var identity = IdentityNumber.Create("1-2345-6789");
        var email = Email.Create("john@ucr.ac.cr");
        var institutionalEmail = Email.Create("staff@ucr.ac.cr");

        var existingPerson = new Person(email, "John", "Doe", Phone.Create("8888-8888"),
            BirthDate.Create(new DateOnly(1990, 1, 1)), identity, 1);

        var existingStaff = new Staff(
            institutionalEmail,
            personId: 1,
            type: "Professor"
        )
        {
            Person = existingPerson
        };

        _mockDbContext.Setup(db => db.Persons)
            .Returns(new List<Person> { existingPerson }.AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(db => db.Staff)
            .Returns(new List<Staff> { existingStaff }.AsQueryable().BuildMockDbSet().Object);

        var newStaff = new Staff(
            institutionalEmail,
            personId: 1,
            type: "Professor"
        )
        {
            Person = existingPerson
        };

        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.CreateStaffAsync(newStaff);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("*same identity or email already exists*");
    }

    /// <summary>
    /// Tests the <see cref="SqlStaffRepository.ListStaffAsync"/> method to ensure it returns a list of staff members.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ListStaffAsync_ShouldReturnList_WhenStaffExists()
    {
        // Arrange
        var person = new Person(
            Email.Create("staff@ucr.ac.cr"), "Jane", "Doe", Phone.Create("1234-5678"),
            BirthDate.Create(new DateOnly(1998, 5, 2)), IdentityNumber.Create("1-1111-1111"), 1);

        var staff = new Staff(
            Email.Create("inst@ucr.ac.cr"),
            personId: 1,
            type: "Academic"
        )
        {
            Person = person
        };

        var mockStaffDbSet = new List<Staff> { staff }
            .AsQueryable()
            .BuildMockDbSet();

        _mockDbContext.Setup(db => db.Staff).Returns(mockStaffDbSet.Object);

        var repository = CreateRepository();

        // Act
        var result = await repository.ListStaffAsync();

        // Assert
        result.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests the <see cref="SqlStaffRepository.ListStaffAsync"/> method to ensure it returns an empty list when no staff members exist.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ListStaffAsync_ShouldReturnEmptyList_WhenNoStaffExists()
    {
        // Arrange
        var emptyStaffList = new List<Staff>().AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(db => db.Staff).Returns(emptyStaffList.Object);
        var repository = CreateRepository();

        // Act
        var result = await repository.ListStaffAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests the <see cref="SqlStaffRepository.CreateStaffAsync"/> method to ensure it throws a <see cref="DomainException"/> when a database update fails.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStaffAsync_ShouldThrowDomainException_WhenDbUpdateFails()
    {
        // Arrange
        var person = new Person(
            Email.Create("new@ucr.ac.cr"), "New", "User", Phone.Create("5555-5555"),
            BirthDate.Create(new DateOnly(1990, 1, 1)), IdentityNumber.Create("1-9999-9999"), 0);

        var staff = new Staff(
            Email.Create("inst@ucr.ac.cr"),
            personId: 0,
            type: "Support"
        )
        {
            Person = person
        };

        var mockPersonDbSet = new List<Person>().AsQueryable().BuildMockDbSet();
        var mockStaffDbSet = new List<Staff>().AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(db => db.Persons).Returns(mockPersonDbSet.Object);
        _mockDbContext.Setup(db => db.Staff).Returns(mockStaffDbSet.Object);

        _mockDbContext.Setup(db => db.Persons.Add(It.IsAny<Person>()));
        _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ThrowsAsync(new DbUpdateException());

        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.CreateStaffAsync(staff);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*database error occurred while creating the staff member*");
    }

    /// <summary>
    /// Tests the <see cref="SqlStaffRepository.CreateStaffAsync"/> method to ensure it returns true when valid data is provided.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStaffAsync_ShouldReturnTrue_WhenDataIsValid()
    {
        // Arrange
        var person = new Person(
            Email.Create("staff@ucr.ac.cr"), "Ana", "Torres", Phone.Create("1111-2222"),
            BirthDate.Create(new DateOnly(1992, 6, 10)), IdentityNumber.Create("1-1234-5678"), 0);

        var staff = new Staff(
            Email.Create("institutional@ucr.ac.cr"),
            personId: 0,
            type: "Administrator"
        )
        {
            Person = person
        };

        var personDbSet = new List<Person>().AsQueryable().BuildMockDbSet();
        var staffDbSet = new List<Staff>().AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(db => db.Persons).Returns(personDbSet.Object);
        _mockDbContext.Setup(db => db.Staff).Returns(staffDbSet.Object);

        _mockDbContext.Setup(db => db.Persons.Add(It.IsAny<Person>()));
        _mockDbContext.Setup(db => db.Staff.Add(It.IsAny<Staff>()));
        _mockDbContext.SetupSequence(db => db.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .ReturnsAsync(1);

        var repository = CreateRepository();

        // Act
        var result = await repository.CreateStaffAsync(staff);

        // Assert
        result.Should().BeTrue();
    }
}
