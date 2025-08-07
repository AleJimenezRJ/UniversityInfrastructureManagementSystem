using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// Participants: Elizabeth Huang C23913 & Esteban Baires C10844
/// The PBIs that this test class are related to are: #291 and #290

/// Technical tasks to complete for the UserAudit entity:
/// - Ensure test database is set up with necessary data
/// - Implement application layer logic to retrieve user audits
/// - Implement method in the audit repository to fetch user audits (infrastructure layer)
/// - Handler empty audit list scenario
/// - Write unit tests for the UserAuditService class

/// <summary>
/// Unit tests for the <see cref="UserAuditService"/> class.
/// </summary>
public class UserAuditServiceTests
{
    private readonly Mock<IUserAuditRepository> _userAuditRepositoryMock;
    private readonly UserAuditService _serviceUnderTest;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuditServiceTests"/> class.
    /// </summary>
    public UserAuditServiceTests()
    {
        _userAuditRepositoryMock = new Mock<IUserAuditRepository>();
        _serviceUnderTest = new UserAuditService(_userAuditRepositoryMock.Object);
    }

    /// <summary>
    /// Creates a test audit object with default values for testing purposes.
    /// </summary>
    /// <param name="action"> The action performed in the audit, default is "Create".</param>
    /// <returns>
    /// Returns a <see cref="UserAudit"/> object initialized with test data.
    /// </returns>
    private UserAudit CreateTestAudit(string action = "Create")
    {
        return new UserAudit
        {
            AuditId = 1,
            UserName = "jdoe",
            FirstName = "John",
            LastName = "Doe",
            Email = "jdoe@example.com",
            Phone = "1234-5678",
            IdentityNumber = "0-1010-0101",
            BirthDate = new DateTime(2000, 1, 1),
            ModifiedAt = DateTime.UtcNow,
            Action = action
        };
    }

    /// <summary>
    /// Tests that ListUserAuditAsync returns a list of user audits when called.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListUserAuditAsync_ShouldReturnAuditList()
    {
        // Arrange
        var audits = new List<UserAudit>
        {
            CreateTestAudit("Create"),
            CreateTestAudit("Delete")
        };

        _userAuditRepositoryMock.Setup(r => r.ListUserAuditAsync()).ReturnsAsync(audits);

        // Act
        var result = await _serviceUnderTest.ListUserAuditAsync();

        // Assert
        result.Should().BeEquivalentTo(audits);
        _userAuditRepositoryMock.Verify(r => r.ListUserAuditAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that ListUserAuditAsync returns an empty list when no audits exist in the repository.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListUserAuditAsync_ShouldReturnEmptyList_WhenNoAuditsExist()
    {
        // Arrange
        var emptyList = new List<UserAudit>();
        _userAuditRepositoryMock.Setup(r => r.ListUserAuditAsync()).ReturnsAsync(emptyList);

        // Act
        var result = await _serviceUnderTest.ListUserAuditAsync();

        // Assert
        result.Should().BeEmpty();
        _userAuditRepositoryMock.Verify(r => r.ListUserAuditAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that GetPaginatedUserAuditAsync returns a paginated list of user audits when called with valid parameters.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetPaginatedUserAuditAsync_ShouldReturnPaginatedList()
    {
        // Arrange
        int pageSize = 10, pageNumber = 1;

        var audits = new List<UserAudit> { CreateTestAudit("Update") };
        var paginated = new PaginatedList<UserAudit>(audits, totalCount: 1, pageIndex: pageNumber, pageSize: pageSize);

        _userAuditRepositoryMock
            .Setup(r => r.GetPaginatedUserAuditAsync(pageSize, pageNumber))
            .ReturnsAsync(paginated);

        // Act
        var result = await _serviceUnderTest.GetPaginatedUserAuditAsync(pageSize, pageNumber);

        // Assert
        result.Should().BeEquivalentTo(paginated);
        _userAuditRepositoryMock.Verify(r => r.GetPaginatedUserAuditAsync(pageSize, pageNumber), Times.Once);
    }

    /// <summary>
    /// Tests that GetPaginatedUserAuditAsync returns an empty paginated list when no audits exist.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetPaginatedUserAuditAsync_ShouldReturnEmptyPaginatedList_WhenNoAuditsExist()
    {
        // Arrange
        int pageSize = 10, pageNumber = 1;
        var emptyPaginatedList = new PaginatedList<UserAudit>(
            new List<UserAudit>(), totalCount: 0, pageIndex: pageNumber, pageSize: pageSize
        );

        _userAuditRepositoryMock
            .Setup(r => r.GetPaginatedUserAuditAsync(pageSize, pageNumber))
            .ReturnsAsync(emptyPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetPaginatedUserAuditAsync(pageSize, pageNumber);

        // Assert
        result.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        _userAuditRepositoryMock.Verify(r => r.GetPaginatedUserAuditAsync(pageSize, pageNumber), Times.Once);
    }
}