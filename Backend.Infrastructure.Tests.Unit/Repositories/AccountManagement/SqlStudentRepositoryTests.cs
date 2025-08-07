using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
///  Assembly containing unit tests for the <see cref="SqlStudentRepository"/> class.
/// </summary>
public class SqlStudentRepositoryTests
{
    /// <summary>
    /// Mock database context for testing the repository methods.
    /// </summary>
    private readonly Mock<ThemeParkDataBaseContext> _mockDbContext = new();

    /// <summary>
    /// Mock logger for logging operations and errors in the repository.
    /// </summary>
    private readonly Mock<ILogger<SqlStudentRepository>> _mockLogger = new();

    /// <summary>
    /// Creates an instance of <see cref="SqlStudentRepository"/> for testing purposes.
    /// </summary>
    /// <returns> An instance of <see cref="SqlStudentRepository"/>.</returns>
    private SqlStudentRepository CreateRepository() =>
        new(_mockDbContext.Object, _mockLogger.Object);

    /// <summary>
    /// Tests that CreateStudentAsync throws DuplicatedEntityException when a student with the same StudentId already exists.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStudentAsync_ShouldThrowDuplicatedEntityException_WhenStudentIdExists()
    {
        // Arrange
        var person = new Person(
            Email.Create("test@ucr.ac.cr"),
            "Maria", "Lopez",
            Phone.Create("8888-8888"),
            BirthDate.Create(new DateOnly(2000, 4, 1)),
            IdentityNumber.Create("1-9999-0000"),
            0
        );

        var student = new Student("B12345", Email.Create("institutional@ucr.ac.cr"), 0)
        {
            Person = person
        };

        var existingStudent = new Student("B12345", Email.Create("institutional@ucr.ac.cr"), 0)
        {
            Person = person
        };

        _mockDbContext.Setup(db => db.Persons)
            .Returns(new List<Person>().AsQueryable().BuildMockDbSet().Object);
        _mockDbContext.Setup(db => db.Students)
            .Returns(new List<Student> { existingStudent }.AsQueryable().BuildMockDbSet().Object);

        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.CreateStudentAsync(student);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("*existing StudentId*");
    }

    /// <summary>
    /// Tests that CreateStudentAsync reuses an existing Person when the Person already exists in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStudentAsync_ShouldReusePerson_WhenPersonAlreadyExists()
    {
        // Arrange
        var identity = IdentityNumber.Create("1-1111-1111");
        var email = Email.Create("jane@ucr.ac.cr");

        var existingPerson = new Person(email, "Jane", "Doe", Phone.Create("8888-0000"),
            BirthDate.Create(new DateOnly(1995, 1, 1)), identity, 7);

        var student = new Student("S2023001", Email.Create("inst@ucr.ac.cr"), 0)
        {
            Person = existingPerson
        };

        _mockDbContext.Setup(db => db.Persons)
            .Returns(new List<Person> { existingPerson }.AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(db => db.Students)
            .Returns(new List<Student>().AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(db => db.Students.Add(It.IsAny<Student>()));
        _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

        var repository = CreateRepository();

        // Act
        var result = await repository.CreateStudentAsync(student);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that CreateStudentAsync creates a new Person when the Person does not exist in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStudentAsync_ShouldCreatePerson_WhenPersonDoesNotExist()
    {
        // Arrange
        var person = new Person(
            Email.Create("new@ucr.ac.cr"),
            "Carlos", "Nuñez",
            Phone.Create("7777-7777"),
            BirthDate.Create(new DateOnly(2001, 6, 15)),
            IdentityNumber.Create("1-7777-0000"),
            0
        );

        var student = new Student("B76543", Email.Create("inst@ucr.ac.cr"), 0)
        {
            Person = person
        };

        _mockDbContext.Setup(db => db.Persons)
            .Returns(new List<Person>().AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(db => db.Students)
            .Returns(new List<Student>().AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(db => db.Persons.Add(It.IsAny<Person>()));
        _mockDbContext.Setup(db => db.Students.Add(It.IsAny<Student>()));
        _mockDbContext.SetupSequence(db => db.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .ReturnsAsync(1);

        var repository = CreateRepository();

        // Act
        var result = await repository.CreateStudentAsync(student);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that ListStudentsAsync returns a list of students when students exist in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ListStudentsAsync_ShouldReturnList_WhenStudentsExist()
    {
        // Arrange
        var person = new Person(
            Email.Create("estudiante@ucr.ac.cr"),
            "Esteban", "Rojas",
            Phone.Create("1111-1111"),
            BirthDate.Create(new DateOnly(1999, 9, 9)),
            IdentityNumber.Create("1-5555-0000"),
            4
        );

        var student = new Student("A10203", Email.Create("inst@ucr.ac.cr"), 4)
        {
            Person = person
        };

        _mockDbContext.Setup(db => db.Students)
            .Returns(new List<Student> { student }.AsQueryable().BuildMockDbSet().Object);

        var repository = CreateRepository();

        // Act
        var result = await repository.ListStudentsAsync();

        // Assert
        result.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that ListStudentsAsync returns an empty list when no students exist in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ListStudentsAsync_ShouldReturnEmptyList_WhenNoStudentsExist()
    {
        // Arrange
        var studentDbSet = new List<Student>().AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(db => db.Students).Returns(studentDbSet.Object);

        var repository = CreateRepository();

        // Act
        var result = await repository.ListStudentsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that CreateStudentAsync throws DomainException when the DbContext is null.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStudentAsync_ShouldThrowDomainException_WhenDbContextIsNull()
    {
        // Arrange
        var person = new Person(
            Email.Create("fail@ucr.ac.cr"),
            "Laura", "Ramirez",
            Phone.Create("1234-5678"),
            BirthDate.Create(new DateOnly(2002, 2, 2)),
            IdentityNumber.Create("1-8888-0000"),
            0
        );

        var student = new Student("B12399", Email.Create("laura@ucr.ac.cr"), 0)
        {
            Person = person
        };

        _mockDbContext.Setup(db => db.Persons)
            .Throws<ArgumentNullException>();

        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.CreateStudentAsync(student);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*An unexpected error occurred while creating the student*");
    }

    /// <summary>
    /// Tests that CreateStudentAsync throws DomainException when a DbUpdateException occurs during save.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateStudentAsync_ShouldThrowDomainException_WhenDbUpdateFails()
    {
        // Arrange
        var person = new Person(
            Email.Create("failupdate@ucr.ac.cr"),
            "Roberto", "Zamora",
            Phone.Create("3333-3333"),
            BirthDate.Create(new DateOnly(1990, 1, 1)),
            IdentityNumber.Create("1-0000-9999"),
            0
        );

        var student = new Student("B99999", Email.Create("rzamora@ucr.ac.cr"), 0)
        {
            Person = person
        };

        var personDbSet = new List<Person>().AsQueryable().BuildMockDbSet();
        var studentDbSet = new List<Student>().AsQueryable().BuildMockDbSet();

        _mockDbContext.Setup(db => db.Persons).Returns(personDbSet.Object);
        _mockDbContext.Setup(db => db.Students).Returns(studentDbSet.Object);

        _mockDbContext.Setup(db => db.Persons.Add(It.IsAny<Person>()));
        _mockDbContext.Setup(db => db.Students.Add(It.IsAny<Student>()));
        _mockDbContext.Setup(db => db.SaveChangesAsync(default))
            .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateException("Save failed"));

        var repository = CreateRepository();

        // Act
        Func<Task> act = async () => await repository.CreateStudentAsync(student);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*while creating the student*");
    }
}
