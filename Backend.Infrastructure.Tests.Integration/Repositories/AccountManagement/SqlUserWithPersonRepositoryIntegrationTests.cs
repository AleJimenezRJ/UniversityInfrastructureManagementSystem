using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.AccountManagement;

/// <summary>
/// Integration tests for the SQL implementation of IUserWithPersonRepository.
/// </summary>
[Collection("Database collection")]
public class SqlUserWithPersonRepositoryIntegrationTests
{
    /// <summary>
    /// Service provider for resolving dependencies in the tests.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUserWithPersonRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture"> Fixture providing the integration test services.</param>
    public SqlUserWithPersonRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that CreateUserWithPersonAsync successfully creates a new user and person in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateUserWithPersonAsync_Should_Create_User_And_Person()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange
        var user = new UserWithPerson(
            userName: UserName.Create("new.user"),
            firstName: "New",
            lastName: "User",
            email: Email.Create("newuser@example.com"),
            phone: Phone.Create("6000-0000"),
            identityNumber: IdentityNumber.Create("1-2345-6789"),
            birthDate: BirthDate.Create(new DateOnly(1990, 1, 1)),
            userId: 0,
            personId: 0
        );
        user.Roles.Add("Admin");

        // Act
        var result = await repo.CreateUserWithPersonAsync(user);

        // Assert
        Assert.True(result);
        var savedUser = await context.Users
            .Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.UserName == UserName.Create("new.user"));
        Assert.NotNull(savedUser);
        Assert.Equal("newuser@example.com", savedUser.Person.Email.Value);
    }

    /// <summary>
    /// Tests that CreateUserWithPersonAsync throws a DuplicatedEntityException
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateUserWithPersonAsync_Should_Throw_When_Email_Or_IdentityNumber_Already_Exist()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange: create original user
        var existingUser = new UserWithPerson(
            userName: UserName.Create("original.user"),
            firstName: "Original",
            lastName: "User",
            email: Email.Create("duplicate@example.com"),
            phone: Phone.Create("6000-0005"),
            identityNumber: IdentityNumber.Create("1-9999-9999"),
            birthDate: BirthDate.Create(new DateOnly(1990, 1, 1)),
            userId: 0,
            personId: 0
        );
        existingUser.Roles.Add("Admin");
        await repo.CreateUserWithPersonAsync(existingUser);

        // Act: try to create a new user with the same email and identity number
        var duplicatedUser = new UserWithPerson(
            userName: UserName.Create("another.user"),
            firstName: "Duplicate",
            lastName: "User",
            email: Email.Create("duplicate@example.com"),
            phone: Phone.Create("6000-0006"),
            identityNumber: IdentityNumber.Create("1-9999-9999"),
            birthDate: BirthDate.Create(new DateOnly(1995, 5, 5)),
            userId: 0,
            personId: 0
        );
        duplicatedUser.Roles.Add("Viewer");

        // Assert
        await Assert.ThrowsAsync<DuplicatedEntityException>(() =>
            repo.CreateUserWithPersonAsync(duplicatedUser));
    }

    /// <summary>
    /// Tests that CreateUserWithPersonAsync throws a DuplicatedEntityException
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateUserWithPersonAsync_Should_Throw_When_UserName_Already_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange: first user with a specific username
        var firstUser = new UserWithPerson(
            userName: UserName.Create("duplicate.user"),
            firstName: "First",
            lastName: "User",
            email: Email.Create("first@example.com"),
            phone: Phone.Create("6000-0009"),
            identityNumber: IdentityNumber.Create("1-1111-1111"),
            birthDate: BirthDate.Create(new DateOnly(1990, 6, 1)),
            userId: 0,
            personId: 0
        );
        firstUser.Roles.Add("Admin");
        await repo.CreateUserWithPersonAsync(firstUser);

        // second user with the same username
        var secondUser = new UserWithPerson(
            userName: UserName.Create("duplicate.user"),
            firstName: "Second",
            lastName: "User",
            email: Email.Create("second@example.com"),
            phone: Phone.Create("6000-0010"),
            identityNumber: IdentityNumber.Create("1-1111-1112"),
            birthDate: BirthDate.Create(new DateOnly(1991, 6, 2)),
            userId: 0,
            personId: 0
        );
        secondUser.Roles.Add("Viewer");

        // Assert
        await Assert.ThrowsAsync<DuplicatedEntityException>(() =>
            repo.CreateUserWithPersonAsync(secondUser));
    }

    /// <summary>
    /// Tests that GetUserWithPersonAsync successfully retrieves a user with their associated person details.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteUserWithPersonAsync_Should_Delete_User_And_Person()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange: create and save user
        var user = new UserWithPerson(
            userName: UserName.Create("delete.user"),
            firstName: "ToDelete",
            lastName: "User",
            email: Email.Create("delete@example.com"),
            phone: Phone.Create("6000-0001"),
            identityNumber: IdentityNumber.Create("1-2345-9999"),
            birthDate: BirthDate.Create(new DateOnly(1985, 5, 15)),
            userId: 0,
            personId: 0
        );
        user.Roles.Add("Admin");
        await repo.CreateUserWithPersonAsync(user);

        var userId = await context.Users
            .Where(u => u.UserName == UserName.Create("delete.user"))
            .Select(u => u.Id)
            .FirstAsync();

        var personId = await context.Users
            .Include(u => u.Person)
            .Where(u => u.Id == userId)
            .Select(u => u.Person.Id)
            .FirstAsync();

        // Act
        var result = await repo.DeleteUserWithPersonAsync(userId, personId);

        // Assert
        Assert.True(result);
        Assert.False(await context.Users.AnyAsync(u => u.Id == userId));
        Assert.False(await context.Persons.AnyAsync(p => p.Id == personId));
    }

    /// <summary>
    /// Tests that DeleteUserWithPersonAsync throws NotFoundException when the user or person does not exist.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteUserWithPersonAsync_Should_Throw_When_User_Not_Found()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            repo.DeleteUserWithPersonAsync(userId: 99999, personId: 99999));
    }

    /// <summary>
    /// Tests that UpdateUserWithPersonAsync successfully updates an existing user and their associated person data.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>

    [Fact]
    public async Task UpdateUserWithPersonAsync_Should_Update_Data_Correctly()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange: create original user
        var user = new UserWithPerson(
            userName: UserName.Create("update.user"),
            firstName: "Old",
            lastName: "Name",
            email: Email.Create("update@example.com"),
            phone: Phone.Create("6000-0002"),
            identityNumber: IdentityNumber.Create("1-2345-8888"),
            birthDate: BirthDate.Create(new DateOnly(1992, 4, 10)),
            userId: 0,
            personId: 0
        );
        user.Roles.Add("Admin");
        await repo.CreateUserWithPersonAsync(user);

        var createdUser = await context.Users
            .Include(u => u.Person)
            .FirstAsync(u => u.UserName == UserName.Create("update.user"));

        // Act: update data
        var updated = new UserWithPerson(
            userName: UserName.Create("updated.user"),
            firstName: "New",
            lastName: "Name",
            email: Email.Create("updated@example.com"),
            phone: Phone.Create("6000-0003"),
            identityNumber: IdentityNumber.Create("1-2345-0000"),
            birthDate: BirthDate.Create(new DateOnly(1993, 1, 1)),
            userId: createdUser.Id,
            personId: createdUser.Person.Id
        );
        updated.Roles.Add("Viewer");

        var result = await repo.UpdateUserWithPersonAsync(updated);

        // Assert
        Assert.True(result);
        var savedUser = await context.Users.Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.Id == createdUser.Id);

        Assert.NotNull(savedUser);
        Assert.Equal("updated.user", savedUser.UserName.Value);
        Assert.Equal("updated@example.com", savedUser.Person.Email.Value);
    }

    /// <summary>
    /// Tests that UpdateUserWithPersonAsync throws NotFoundException when the user does not exist.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task UpdateUserWithPersonAsync_Should_Throw_When_User_Not_Found()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        var ghostUser = new UserWithPerson(
            userName: UserName.Create("ghost.user"),
            firstName: "Ghost",
            lastName: "User",
            email: Email.Create("ghost@example.com"),
            phone: Phone.Create("6000-0011"),
            identityNumber: IdentityNumber.Create("1-0000-0000"),
            birthDate: BirthDate.Create(new DateOnly(1980, 1, 1)),
            userId: 99999,
            personId: 99999
        );
        ghostUser.Roles.Add("Ghost");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            repo.UpdateUserWithPersonAsync(ghostUser));
    }
    /// <summary>
    /// Tests that GetUserIdByEmailAsync successfully retrieves the user ID for an existing email address.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetUserIdByEmailAsync_Should_Return_UserId_When_Email_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange
        var email = "findme@example.com";
        var user = new UserWithPerson(
            userName: UserName.Create("find.user"),
            firstName: "Find",
            lastName: "Me",
            email: Email.Create(email),
            phone: Phone.Create("6000-0004"),
            identityNumber: IdentityNumber.Create("1-2345-0001"),
            birthDate: BirthDate.Create(new DateOnly(1994, 2, 2)),
            userId: 0,
            personId: 0
        );
        user.Roles.Add("Viewer");
        await repo.CreateUserWithPersonAsync(user);

        // Act
        var result = await repo.GetUserIdByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.True(result > 0);
    }

    /// <summary>
    /// Tests that GetUserIdByEmailAsync returns null when the email does not exist in the database.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetUserIdByEmailAsync_Should_Return_Null_When_Email_Does_Not_Exist()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        var result = await repo.GetUserIdByEmailAsync("nonexistent@email.com");

        Assert.Null(result);
    }


    /// <summary>
    /// Tests that GetAllUserWithPersonAsync successfully retrieves all users with their associated person details.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUserWithPersonAsync_Should_Return_All_Users()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Arrange: insertar dos usuarios
        var user1 = new UserWithPerson(
            UserName.Create("user.one"),
            "User",
            "One",
            Email.Create("one@example.com"),
            Phone.Create("6000-0012"),
            IdentityNumber.Create("1-2222-2222"),
            BirthDate.Create(new DateOnly(1995, 1, 1)),
            0, 0
        );
        user1.Roles.Add("Admin");

        var user2 = new UserWithPerson(
            UserName.Create("user.two"),
            "User",
            "Two",
            Email.Create("two@example.com"),
            Phone.Create("6000-0013"),
            IdentityNumber.Create("1-2222-3333"),
            BirthDate.Create(new DateOnly(1996, 2, 2)),
            0, 0
        );
        user2.Roles.Add("Viewer");

        await repo.CreateUserWithPersonAsync(user1);
        await repo.CreateUserWithPersonAsync(user2);

        // Act
        var allUsers = await repo.GetAllUserWithPersonAsync();

        // Assert
        Assert.NotNull(allUsers);
        Assert.True(allUsers.Count >= 2);
        Assert.Contains(allUsers, u => u.UserName.Value == "user.one");
        Assert.Contains(allUsers, u => u.UserName.Value == "user.two");
    }

    /// <summary>
    /// Tests that GetAllUserWithPersonAsync includes assigned roles for each user.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUserWithPersonAsync_Should_Include_Assigned_Roles()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserWithPersonRepository>();

        // Ensure roles exist in DB
        if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
            context.Roles.Add(new Role("Admin"));

        if (!await context.Roles.AnyAsync(r => r.Name == "Viewer"))
            context.Roles.Add(new Role("Viewer"));

        await context.SaveChangesAsync();

        // Arrange
        var user = new UserWithPerson(
            UserName.Create("multi.role.user"),
            "Multi",
            "Role",
            Email.Create("multi@example.com"),
            Phone.Create("6000-0100"),
            IdentityNumber.Create("1-5555-5555"),
            BirthDate.Create(new DateOnly(1990, 10, 10)),
            0, 0
        );
        user.Roles.Add("Admin");
        user.Roles.Add("Viewer");

        await repo.CreateUserWithPersonAsync(user);

        // Act
        var allUsers = await repo.GetAllUserWithPersonAsync();
        var targetUser = allUsers.FirstOrDefault(u => u.UserName.Value == "multi.role.user");

        // Assert
        Assert.NotNull(targetUser);
        Assert.Contains("Admin", targetUser.Roles);
        Assert.Contains("Viewer", targetUser.Roles);
    }

}
