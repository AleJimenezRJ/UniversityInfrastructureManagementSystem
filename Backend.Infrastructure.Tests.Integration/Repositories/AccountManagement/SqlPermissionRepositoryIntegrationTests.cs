using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.AccountManagement;

/// <summary>
/// Integration tests for the <see cref="SqlPermissionRepository"/> class.
/// </summary>

[Collection("Database collection")]
public class SqlPermissionRepositoryIntegrationTests
{
    /// <summary>
    /// The service provider used to resolve dependencies for the tests.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlPermissionRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture"> Fixture providing the integration test services.</param>
    public SqlPermissionRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests the GetAllPermissionsAsync method of the SqlPermissonRepository to ensure it returns all permissions correctly.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllPermissionsAsync_Should_Return_All_Permissions()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

        // Arrange
        var permission1 = new Permission("View access");
        var permission2 = new Permission("Edit access");

        context.Permissions.AddRange(permission1, permission2);
        await context.SaveChangesAsync();

        // Act
        var permissions = await repo.GetAllPermissionsAsync();

        // Assert
        Assert.NotNull(permissions);
        Assert.True(permissions.Count >= 2);
        Assert.Contains(permissions, p => p.Description == "View access");
        Assert.Contains(permissions, p => p.Description == "Edit access");
    }

    /// <summary>
    /// Tests that GetAllPermissionsAsync does not throw an exception when there are no permissions in the database.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllPermissionsAsync_Should_Not_Throw_When_Empty()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

        // Clean table
        context.Permissions.RemoveRange(context.Permissions);
        await context.SaveChangesAsync();

        // Act
        var permissions = await repo.GetAllPermissionsAsync();

        // Assert
        Assert.NotNull(permissions);
        Assert.Empty(permissions);
    }

    /// <summary>
    /// Tests that GetAllPermissionsAsync returns some records when there are permissions in the database.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>

    [Fact]
    public async Task GetAllPermissionsAsync_Should_Return_Some_Records()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

        context.Permissions.Add(new Permission("Any permission"));
        await context.SaveChangesAsync();

        // Act
        var permissions = await repo.GetAllPermissionsAsync();

        // Assert
        Assert.NotNull(permissions);
        Assert.NotEmpty(permissions);
    }
}
