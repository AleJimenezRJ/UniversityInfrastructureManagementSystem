using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration;

/// <summary>
/// Provides a reusable fixture for integration tests, initializing
/// the application's infrastructure and ensuring a clean database context.
/// </summary>
public class IntegrationTestFixture
{
    /// <summary>
    /// The configured service provider used to resolve application services.
    /// </summary>
    public ServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestFixture"/> class.
    /// Sets up configuration, registers infrastructure services, and prepares a clean test database.
    /// </summary>
    public IntegrationTestFixture()
    {
        // Build configuration using user secrets for sensitive settings like connection strings
        var config = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTestFixture>() // Retrieves secrets from the UserSecrets stored in this project. Needs to be configured before use, same as in Backend.Api
            .Build();

        var services = new ServiceCollection();

        services.AddLogging();

        // Register infrastructure layer (including DbContext with actual connection string)
        services.AddInfrastructureLayerServices(config);

        // Mock audit logger so it doesn't actually write anything during tests
        var mockAuditLogger = new Mock<IUserAuditLoggerService>();
        mockAuditLogger
            .Setup(logger => logger.LogAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        services.AddSingleton<IUserAuditLoggerService>(mockAuditLogger.Object);

        ServiceProvider = services.BuildServiceProvider();

        // Ensure database is reset for a clean test environment
        using var scope = ServiceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        // Create stored procedure for user audit logging
        db.Database.ExecuteSqlRaw(
            """
            IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spInsertUserAudit')
                DROP PROCEDURE spInsertUserAudit;
            EXEC('
            CREATE PROCEDURE spInsertUserAudit
                @UserName NVARCHAR(50),
                @FirstName NVARCHAR(50),
                @LastName NVARCHAR(50),
                @Email NVARCHAR(100),
                @Phone VARCHAR(20),
                @IdentityNumber VARCHAR(20),
                @BirthDate DATE,
                @Action NVARCHAR(20)
            AS
            BEGIN
                INSERT INTO UserAudit (UserName, FirstName, LastName, Email, Phone, IdentityNumber, BirthDate, ModifiedAt, Action)
                VALUES (@UserName, @FirstName, @LastName, @Email, @Phone, @IdentityNumber, @BirthDate, GETUTCDATE(), @Action);
            END;
            ')
            """
        );
    }
}