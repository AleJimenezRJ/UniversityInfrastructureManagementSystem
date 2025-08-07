using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="SqlUserWithPersonRepository"/> which retrieves user-person joined data.
/// </summary>
public class SqlUserWithPersonRepositoryTests
{
    /// <summary>
    /// Tests that <see cref="SqlUserWithPersonRepository.GetAllUserWithPersonAsync"/> returns a list of users joined with their corresponding person data.
    /// </summary>
    [Fact]
    public async Task GetAllUserWithPersonAsync_ShouldReturnJoinedUserWithPersonList()
    {
        // Arrange

        List<Person> people = new()
        {
               new Person(
                    Email.Create("john.doe@ucr.ac.cr"),
                    "John",
                    "Doe",
                    Phone.Create("8888-0000"),
                    BirthDate.Create(new DateOnly(1990, 1, 1)),
                    IdentityNumber.Create("1-2345-6789"),
                    1),
               new Person(
                    Email.Create("elena.maria@ucr.ac.cr"),
                    "Elena",
                    "Maria",
                    Phone.Create("8999-1111"),
                    BirthDate.Create(new DateOnly(1985, 2, 20)),
                    IdentityNumber.Create("1-8765-4321"),
                    2),
        };

        List<User> users = new()
        {
            new User(UserName.Create("jdoe"), 1) { Person = people[0] },
            new User(UserName.Create("emaria"), 2) { Person = people[1] },
        };



        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Person>> mockPersonsDbSet = people.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);
        mockDbContext.Setup(c => c.Persons).Returns(mockPersonsDbSet.Object);

        SqlUserWithPersonRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserWithPersonRepository>>());

        // Act
        var result = await repository.GetAllUserWithPersonAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(u => u.UserName.Value == "jdoe" && u.FirstName == "John");
        result.Should().Contain(u => u.UserName.Value == "emaria" && u.FirstName == "Elena");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserWithPersonRepository.GetAllUserWithPersonAsync"/> returns an empty list when there are no users in the database.
    /// </summary>
    [Fact]
    public async Task GetAllUserWithPersonAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        List<User> users = [];
        List<Person> people = [];
        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Person>> mockPersonsDbSet = people.AsQueryable().BuildMockDbSet();

        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);
        mockDbContext.Setup(c => c.Persons).Returns(mockPersonsDbSet.Object);

        SqlUserWithPersonRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserWithPersonRepository>>());

        // Act
        var result = await repository.GetAllUserWithPersonAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="SqlUserWithPersonRepository.GetAllUserWithPersonAsync"/> ignores users that do not have a matching person entry.
    /// </summary>
    [Fact]
    public async Task GetAllUserWithPersonAsync_ShouldIgnoreUsersWithoutMatchingPerson()
    {
        // Arrange

        List<Person> people = new()
        {
               new Person(
                    Email.Create("john.doe@ucr.ac.cr"),
                    "John",
                    "Doe",
                    Phone.Create("8888-0000"),
                    BirthDate.Create(new DateOnly(1990, 1, 1)),
                    IdentityNumber.Create("1-2345-6789"),
                    1),
        };

        List<User> users = new()
        {
            new User(UserName.Create("jdoe"), 1) { Person = people[0] },
            new User(UserName.Create("unlinked"), 99)
        };

        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Person>> mockPersonsDbSet = people.AsQueryable().BuildMockDbSet();

        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);
        mockDbContext.Setup(c => c.Persons).Returns(mockPersonsDbSet.Object);

        SqlUserWithPersonRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserWithPersonRepository>>());

        // Act
        var result = await repository.GetAllUserWithPersonAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().UserName.Value.Should().Be("jdoe");
    }
}
