using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.AccountManagement;

/// <summary>
/// Contains integration tests for the <see cref="SqlUserRoleRepository"/> class.
/// </summary>
[Collection("Database collection")]
public class SqlUserRoleRepositoryIntegrationTests
{
    /// <summary>
    /// The service provider used to resolve dependencies for the tests.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUserRoleRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture"> Fixture providing the integration test services.</param>
    public SqlUserRoleRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Creates a new user in the database for testing purposes.
    /// </summary>
    /// <param name="context"> The database context to use for creating the user.</param>
    /// <param name="username"> The username for the new user.</param>
    /// <param name="email"> The email address for the new user.</param>
    /// <param name="identity"> The identity number for the new user.</param>
    /// <returns> A task that represents the asynchronous operation, containing the created <see cref="User"/>.</returns>
    private async Task<User> CreateUserAsync(ThemeParkDataBaseContext context, string username, string email, string identity)
    {
        var person = new Person(
            Email.Create(email),
            "Test",
            "User",
            Phone.Create("6000-0000"),
            BirthDate.Create(new DateOnly(1990, 1, 1)),
            IdentityNumber.Create(identity)
        );
        context.Persons.Add(person);
        await context.SaveChangesAsync();

        var user = new User(UserName.Create(username), person.Id);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Tests the AssignRoleAsync method of the SqlUserRoleRepository to ensure it correctly assigns a role to a user.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task AssignRoleAsync_Should_Assign_Role_To_User()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

        var role = new Role("Editor");
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var user = await CreateUserAsync(context, "role.test.user", "role@example.com", "1-1000-1000");

        var result = await repo.AssignRoleAsync(user.Id, role.Id);

        Assert.True(result);
        Assert.True(await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id));
    }

    /// <summary>
    /// Tests the AssignRoleAsync method of the SqlUserRoleRepository to ensure it throws an exception
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task AssignRoleAsync_Should_Throw_When_Role_Already_Assigned()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

        // Verify that the role does not exist before creating it
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (role is null)
        {
            role = new Role("Admin");
            context.Roles.Add(role);
            await context.SaveChangesAsync();
        }

        var user = await CreateUserAsync(context, "dup.role.user", "dup@example.com", "1-4000-4000");

        var userRole = new UserRole(user.Id, role.Id);
        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<DuplicatedEntityException>(() =>
            repo.AssignRoleAsync(user.Id, role.Id));
    }

    /// <summary>
    /// Tests the GetByUserAndRoleAsync method of the SqlUserRoleRepository to ensure it retrieves the correct user-role association.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation, containing the <see cref="UserRole"/>.</returns>

    [Fact]
    public async Task RemoveAsync_Should_Delete_UserRole_Association()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

        var role = new Role("Edit");
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var user = await CreateUserAsync(context, "remove.role.user", "remove@example.com", "1-2000-2000");

        var userRole = new UserRole(user.Id, role.Id);
        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync();

        var result = await repo.RemoveAsync(userRole);

        Assert.True(result);
        Assert.False(await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id));
    }

    /// <summary>
    /// Tests the RemoveAsync method of the SqlUserRoleRepository to ensure it throws an exception
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task RemoveAsync_Should_Throw_When_Association_Not_Found()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

        var nonExistent = new UserRole(userId: 99999, roleId: 99999);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            repo.RemoveAsync(nonExistent));
    }

    /// <summary>
    /// Tests the GetRolesByUserIdAsync method of the SqlUserRoleRepository to ensure it retrieves all roles assigned to a user.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation, containing a list of <see cref="Role"/>.</returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_Should_Return_Assigned_Roles()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();
        var role1 = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

        if (role1 == null)
        {
            role1 = new Role("Admin");
            context.Roles.Add(role1);
        }

        var role2 = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Viewer");
        if (role2 == null)
        {
            role2 = new Role("Viewer");
            context.Roles.Add(role2);
        }

        await context.SaveChangesAsync();

        var user = await CreateUserAsync(context, "getroles.user", "roles@example.com", "1-3000-3000");

        if (!await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role1.Id))
            context.UserRoles.Add(new UserRole(user.Id, role1.Id));
        if (!await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role2.Id))
            context.UserRoles.Add(new UserRole(user.Id, role2.Id));
        await context.SaveChangesAsync();

        // Act
        var roles = (await repo.GetRolesByUserIdAsync(user.Id) ?? Enumerable.Empty<Role>()).ToList();

        // Assert
        Assert.NotNull(roles);
        Assert.Contains(roles, r => r.Name == "Admin");
        Assert.Contains(roles, r => r.Name == "Viewer");
    }



    /// <summary>
    /// Tests the GetRolesByUserIdAsync method of the SqlUserRoleRepository to ensure it returns an empty list when no roles are assigned to the user.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation, containing an empty list of roles.</returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_Should_Return_Empty_When_No_Roles_Assigned()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

        var user = await CreateUserAsync(context, "no.roles.user", "nobody@example.com", "1-5000-5000");

        var roles = await repo.GetRolesByUserIdAsync(user.Id);

        Assert.NotNull(roles);
        Assert.Empty(roles);
    }
}
