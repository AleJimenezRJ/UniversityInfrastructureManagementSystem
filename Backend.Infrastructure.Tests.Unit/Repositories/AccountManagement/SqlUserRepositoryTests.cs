using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Unit tests for <see cref="SqlUserRepository"/> which manages user creation and retrieval.
/// </summary>
public class SqlUserRepositoryTests
{
    /// <summary>
    /// Tests that CreateUserAsync throws DuplicatedEntityException when a user with the same username already exists.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateUserAsync_ShouldThrowDuplicatedEntityException_WhenUsernameAlreadyExists()
    {
        // Arrange
        var existingUser = new User(UserName.Create("existinguser"), 1);
        var users = new List<User> { existingUser }.AsQueryable().BuildMockDbSet();
        var persons = new List<Person>().AsQueryable().BuildMockDbSet();

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.Users).Returns(users.Object);
        mockDbContext.Setup(c => c.Persons).Returns(persons.Object);

        var repository = new SqlUserRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        var newUser = new User(UserName.Create("existinguser"), 2);

        // Act
        // This should throw an exception because the username already exists
        Func<Task> act = async () => await repository.CreateUserAsync(newUser);

        // Assert
        await act.Should()
            .ThrowAsync<DuplicatedEntityException>()
            .WithMessage("A user with the username 'existinguser' already exists.");
    }

    /// <summary>
    /// Tests that CreateUserAsync throws NotFoundException when the associated person ID does not exist in the database.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateUserAsync_ShouldThrowNotFoundException_WhenPersonIdIsInvalid()
    {
        // Arrange
        var users = new List<User>().AsQueryable().BuildMockDbSet();
        var persons = new List<Person>().AsQueryable().BuildMockDbSet();

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.Users).Returns(users.Object);
        mockDbContext.Setup(c => c.Persons).Returns(persons.Object);

        var repository = new SqlUserRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        var user = new User(UserName.Create("newuser"), 99);

        // Act
        Func<Task> act = async () => await repository.CreateUserAsync(user);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("The associated person with ID '99' was not found.");
    }


    /// <summary>
    /// Tests that CreateUserAsync returns true when valid parameters are given and the user is created successfully.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateUserAsync_ShouldReturnTrue_WhenUsernameIsUniqueAndPersonExists()
    {
        // Arrange
        List<User> users = [];
        List<Person> people = [];
        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Person>> mockPersonsDbSet = people.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.Users)
            .Returns(mockUsersDbSet.Object);
        mockDbContext
            .Setup(c => c.Persons)
            .Returns(mockPersonsDbSet.Object);

        SqlUserRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Add people into the mock database
        var person1 = new Person(Email.Create("elizabeth.huang@ucr.ac.cr"), "Elizabeth", "Huang", Phone.Create("9999-2222"), BirthDate.Create(new DateOnly(2003, 07, 09)), IdentityNumber.Create("1-9999-8888"), 1);
        var person2 = new Person(Email.Create("elaine.huang@ucr.ac.cr"), "Elaine", "Huang", Phone.Create("2323-2222"), BirthDate.Create(new DateOnly(1997, 08, 25)), IdentityNumber.Create("1-2399-8888"), 2);
        people.Add(person1);
        people.Add(person2);

        // Add an existing user with the same username
        var existingUser = new User(UserName.Create("tester"), 1);
        users.Add(existingUser);

        // Act
        // Creating a new user with a username that does not exist prior to this operation
        var result = await repository.CreateUserAsync(new User(UserName.Create("cyaos"), 2));

        // Assert
        result.Should().BeTrue(because: "The user is created successfully with a unique username and valid person ID.");
    }

    /// <summary>
    /// Tests that DeleteUserAsync throws NotFoundException when an invalid user ID is given.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteUserAsync_ShouldThrowNotFoundException_WhenUserIdDoesNotExist()
    {
        // Arrange
        var users = new List<User>().AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.Users).Returns(users.Object);
        var repository = new SqlUserRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Act
        Func<Task> act = async () => await repository.DeleteUserAsync(999); // nonexistent ID

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("User with ID 999 was not found.");
    }


    /// <summary>
    /// Tests that DeleteUserAsync returns true when a user is successfully deleted by their ID.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenAValidIdIsGiven()
    {
        // Arrange
        var existingUser = new User(UserName.Create("tester"), 1) { Id = 1 };

        var mockUsersDbSet = new Mock<DbSet<User>>();
        mockUsersDbSet
            .Setup(d => d.Remove(It.IsAny<User>()));

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext
            .Setup(c => c.Users)
            .Returns(mockUsersDbSet.Object);
        mockDbContext
            .Setup(c => c.Users.FindAsync(1))
            .ReturnsAsync(existingUser);
        mockDbContext
            .Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var repository = new SqlUserRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Act
        var result = await repository.DeleteUserAsync(1);

        // Assert
        result.Should().BeTrue(because: "the user with ID 1 exists and should be deletable.");
    }


    /// <summary>
    /// Tests that GetAllUsersAsync returns a list of users when there are users in the database.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAUserList_WhenUsersExist()
    {
        // Arrange
        List<User> users = [];
        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.Users)
            .Returns(mockUsersDbSet.Object);

        SqlUserRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Add an existing user to the mock database
        var existingUser = new User(UserName.Create("tester"), 1);
        users.Add(existingUser);

        // Act
        // Deleting the user with ID 1, which exists in the mock database
        var result = await repository.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull(because: "the repository should return a list")
            .And.HaveCount(1, because: "one user was added to the mock context")
            .And.ContainSingle(u => u.UserName.Value == "tester", because: "the username matches the expected one");
    }

    /// <summary>
    /// Tests that GetAllUsersAsync returns an empty list when there are no users in the database.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAnEmptyList_WhenThereAreNoUsers()
    {
        // Arrange
        List<User> users = [];
        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.Users)
            .Returns(mockUsersDbSet.Object);

        SqlUserRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Act
        // Deleting the user with ID 1, which exists in the mock database
        var result = await repository.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull(because: "the repository should return a list")
            .And.BeEmpty(because: "no users were added to the mock context");
    }

    /// <summary>
    /// Tests that ModifyUserAsync throws NotFoundException when an invalid user ID is given.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ModifyUserAsync_ShouldThrowNotFoundException_WhenInvalidIdIsGiven()
    {
        // Arrange
        List<User> users = [];
        Mock<DbSet<User>> mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.Users)
            .Returns(mockUsersDbSet.Object);

        SqlUserRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        // Act
        Func<Task> act = async () => await repository.ModifyUserAsync(2, new User(UserName.Create("elizabeth"), 2));

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("User with ID 2 was not found.");
    }


    /// <summary>
    /// Tests that ModifyUserAsync returns true when a user is successfully modified by their ID.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ModifyUserAsync_ShouldReturnTrue_WhenValidIdIsGiven()
    {
        // Arrange
        var existingUser = new User(UserName.Create("tester"), 1) { Id = 1 };
        var users = new List<User> { existingUser };
        var mockUsersDbSet = users.AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        // Explicitly mock FindAsync to return the user
        mockDbContext
            .Setup(c => c.Users.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((object[] ids) => users.FirstOrDefault(u => u.Id == (int)ids[0]));

        mockDbContext
            .Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var repository = new SqlUserRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRepository>>());

        var updatedUser = new User(UserName.Create("elizabeth"), 2);

        // Act
        var result = await repository.ModifyUserAsync(1, updatedUser);

        // Assert
        result.Should().BeTrue(because: "the user with ID 1 exists and should be modifiable.");
    }
}