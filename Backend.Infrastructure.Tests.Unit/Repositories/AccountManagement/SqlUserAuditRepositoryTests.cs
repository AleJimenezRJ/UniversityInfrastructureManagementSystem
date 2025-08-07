using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MockQueryable.Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// Participants: Elizabeth Huang C23913
/// The PBIs that this test class are related to are: #291 and #290

/// Technical tasks to complete for the UserAudit entity:
/// - Ensure test database is set up with necessary data
/// - Implement application layer logic to retrieve user audits
/// - Implement method in the audit repository to fetch user audits (infrastructure layer)
/// - Handler empty audit list scenario
/// - Write unit tests for the UserAuditService class

/// <summary>
/// Unit tests for the <see cref="SqlUserAuditRepository"/> class.
/// </summary>
public class SqlUserAuditRepositoryTests
{
    /// <summary>
    /// Tests that ListUserAuditAsync returns a list of user audits when called.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListUserAuditAsync_ShouldReturnAuditsOrderedByModifiedAt()
    {
        // Arrange
        var audits = new List<UserAudit>
        {
            new() { AuditId = 1, UserName = "u1", ModifiedAt = DateTime.UtcNow.AddMinutes(-2), Action = "Login" },
            new() { AuditId = 2, UserName = "u2", ModifiedAt = DateTime.UtcNow.AddMinutes(-1), Action = "Logout" }
        };

        var mockDbSet = audits.AsQueryable().BuildMockDbSet();
        var mockContext = new Mock<ThemeParkDataBaseContext>();
        mockContext.Setup(c => c.UserAudits).Returns(mockDbSet.Object);

        var repository = new SqlUserAuditRepository(mockContext.Object, Mock.Of<ILogger<SqlUserAuditRepository>>());

        // Act
        var result = await repository.ListUserAuditAsync();

        // Assert
        result.Should().HaveCount(2)
            .And.BeInDescendingOrder(a => a.ModifiedAt);
    }

    /// <summary>
    /// Tests that ListUserAuditAsync returns an empty list when no audits exist in the repository.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListUserAuditAsync_ShouldReturnEmptyList_OnDbError()
    {
        // Arrange
        var mockContext = new Mock<ThemeParkDataBaseContext>();
        mockContext.Setup(c => c.UserAudits).Throws(new Exception("Database unavailable"));

        var logger = new Mock<ILogger<SqlUserAuditRepository>>();
        var repository = new SqlUserAuditRepository(mockContext.Object, logger.Object);

        // Act
        var result = await repository.ListUserAuditAsync();

        // Assert
        result.Should().BeEmpty(because: "an error occurred and it should return an empty list");
    }

    /// <summary>
    /// Generates a list of user audits for testing purposes.
    /// </summary>
    /// <param name="count">The number of user audits to generate.</param>
    /// <returns>
    /// A list of <see cref="UserAudit"/> objects with sequential IDs and sample data.
    /// </returns>
    private static List<UserAudit> GenerateAudits(int count)
    {
        return Enumerable.Range(1, count).Select(i => new UserAudit
        {
            AuditId = i,
            UserName = $"user{i}",
            FirstName = $"First{i}",
            LastName = $"Last{i}",
            Email = $"user{i}@test.com",
            Phone = $"8888{i}",
            IdentityNumber = $"ID{i}",
            BirthDate = new DateTime(1990, 1, 1),
            ModifiedAt = DateTime.UtcNow.AddMinutes(-i),
            Action = "TestAction"
        }).ToList();
    }

    /// <summary>Add commentMore actions
    /// Tests that GetPaginatedUserAuditAsync returns the correct page of user audits.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetPaginatedUserAuditAsync_ShouldReturnCorrectPage()
    {
        // Arrange
        // Generate 30 audits for testing
        var audits = GenerateAudits(30);
        var mockDbSet = audits.AsQueryable().BuildMockDbSet();

        var mockContext = new Mock<ThemeParkDataBaseContext>();
        mockContext.Setup(c => c.UserAudits).Returns(mockDbSet.Object);

        var repository = new SqlUserAuditRepository(mockContext.Object, Mock.Of<ILogger<SqlUserAuditRepository>>());

        // Act
        // Request the first page with 10 items per page
        var page = await repository.GetPaginatedUserAuditAsync(pageSize: 10, pageNumber: 1);

        // Assert
        // Verify that the page contains the correct number of items and metadata
        page.Should().HaveCount(10);
        page.TotalCount.Should().Be(30);
        page.First().AuditId.Should().Be(11);
    }

    /// <summary>
    /// Tests that GetPaginatedUserAuditAsync throws a DomainException when an error occurs during retrieval.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetPaginatedUserAuditAsync_ShouldThrowDomainException_OnDbError()
    {
        // Arrange
        var mockContext = new Mock<ThemeParkDataBaseContext>();
        mockContext.Setup(c => c.UserAudits).Throws(new Exception("DB down"));

        var logger = new Mock<ILogger<SqlUserAuditRepository>>();
        var repository = new SqlUserAuditRepository(mockContext.Object, logger.Object);

        // Act
        Func<Task> act = async () => await repository.GetPaginatedUserAuditAsync(10, 0);

        // Assert
        // The act should throw a DomainException with a specific message
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*retrieving user audits*");
    }
}