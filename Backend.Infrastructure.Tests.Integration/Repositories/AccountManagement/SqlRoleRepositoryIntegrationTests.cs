using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.AccountManagement;

/// <summary>
/// Integration tests for the <see cref="SqlRoleRepository"/> class.
/// </summary>
[Collection("Database collection")]
public class SqlRoleRepositoryIntegrationTests
{
    /// <summary>
    /// The service provider used to resolve dependencies for the tests.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlRoleRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture"> Fixture providing the integration test services.</param>
    public SqlRoleRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests the GetAllRolesAsync method of the SqlRoleRepository to ensure it returns default roles correctly.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllRolesAsync_Should_Return_Default_Roles()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

        // Add default roles if they do not exist
        if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
            context.Roles.Add(new Role("Admin"));

        if (!await context.Roles.AnyAsync(r => r.Name == "Viewer"))
            context.Roles.Add(new Role("Viewer"));

        await context.SaveChangesAsync();

        // Act
        var roles = await repo.GetAllRolesAsync();

        // Assert
        Assert.NotNull(roles);
        Assert.Contains(roles, r => r.Name == "Admin");
        Assert.Contains(roles, r => r.Name == "Viewer");
    }

    /// <summary>
    /// Tests that GetAllRolesAsync does not throw an exception when there are no roles in the database.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllRolesAsync_Should_Return_Empty_When_No_Roles()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

        // Delete all roles to ensure the database is empty
        context.Roles.RemoveRange(context.Roles);
        await context.SaveChangesAsync();

        // Act
        var roles = await repo.GetAllRolesAsync();

        // Assert
        Assert.NotNull(roles);
        Assert.Empty(roles);
    }

    /// <summary>
    /// Tests the GetRoleByIdAsync method of the SqlRoleRepository to ensure it returns a role by its ID correctly.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteRoleAsync_Should_Throw_When_Not_Found()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            repo.DeleteRoleAsync(roleId: 99999));
    }

    /// <summary>
    /// Tests the DeleteRoleAsync method of the SqlRoleRepository to ensure it deletes an existing role correctly.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteRoleAsync_Should_Delete_Existing_Role()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

        // Arrange: add a temporary role to delete
        var role = new Role("TempRole");
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var roleId = await context.Roles
            .Where(r => r.Name == "TempRole")
            .Select(r => r.Id)
            .FirstAsync();

        // Act
        var result = await repo.DeleteRoleAsync(roleId);

        // Assert
        Assert.True(result);
        Assert.False(await context.Roles.AnyAsync(r => r.Id == roleId));
    }
}
