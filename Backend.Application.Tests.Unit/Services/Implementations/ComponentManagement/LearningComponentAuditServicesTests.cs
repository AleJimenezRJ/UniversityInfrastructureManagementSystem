using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/// <summary>
/// Unit tests for <see cref="LearningComponentAuditServices"/> to verify correct repository interaction
/// and behavior in audit data retrieval operations.
/// </summary>
public class LearningComponentAuditServicesTests
{
    private readonly Mock<ILearningComponentAuditRepository> _auditRepositoryMock;
    private readonly LearningComponentAuditServices _serviceUnderTest;

    private readonly List<LearningComponentAudit> _auditList;
    private readonly PaginatedList<LearningComponentAudit> _paginatedAuditList;
    private readonly int _pageSize = 5;
    private readonly int _pageIndex = 0;

    /// <summary>
    /// Initializes mock data and dependencies for each test.
    /// </summary>
    public LearningComponentAuditServicesTests()
    {
        _auditRepositoryMock = new Mock<ILearningComponentAuditRepository>();
        _serviceUnderTest = new LearningComponentAuditServices(_auditRepositoryMock.Object);

        _auditList = new List<LearningComponentAudit>
        {
            new()
            {
                LearningComponentAuditId = 1,
                ComponentId = 101,
                Width = 120,
                Height = 80,
                Depth = 10,
                X = 1,
                Y = 2,
                Z = 3,
                Orientation = "North",
                IsDeleted = false,
                ComponentType = "Whiteboard",
                MarkerColor = "Red",
                Action = "Created",
                ModifiedAt = DateTime.UtcNow
            }
        };

        _paginatedAuditList = new PaginatedList<LearningComponentAudit>(_auditList, _auditList.Count, _pageSize, _pageIndex);
    }

    /// <summary>
    /// Validates that the service returns all audit records from the repository.
    /// </summary>
    [Fact]
    public async Task ListLearningComponentAuditAsync_ShouldReturnAuditList_WhenRecordsExist()
    {
        // Arrange
        _auditRepositoryMock
            .Setup(repo => repo.ListLearningComponentAuditAsync())
            .ReturnsAsync(_auditList);

        // Act
        var result = await _serviceUnderTest.ListLearningComponentAuditAsync();

        // Assert
        result.Should().BeEquivalentTo(_auditList, "the service should return all audit records from the repository");
    }

    /// <summary>
    /// Validates that the service returns an empty list when the repository contains no audit records.
    /// </summary>
    [Fact]
    public async Task ListLearningComponentAuditAsync_ShouldReturnEmptyList_WhenNoRecordsExist()
    {
        // Arrange
        _auditRepositoryMock
            .Setup(repo => repo.ListLearningComponentAuditAsync())
            .ReturnsAsync(new List<LearningComponentAudit>());

        // Act
        var result = await _serviceUnderTest.ListLearningComponentAuditAsync();

        // Assert
        result.Should().BeEmpty("an empty repository should return an empty audit list");
    }

    /// <summary>
    /// Validates that the service correctly returns a paginated list of audit records.
    /// </summary>
    [Fact]
    public async Task GetPaginatedLearningComponentAuditAsync_ShouldReturnPaginatedAuditList()
    {
        // Arrange
        _auditRepositoryMock
            .Setup(repo => repo.GetPaginatedLearningComponentAuditAsync(_pageSize, _pageIndex))
            .ReturnsAsync(_paginatedAuditList);

        // Act
        var result = await _serviceUnderTest.GetPaginatedLearningComponentAuditAsync(_pageSize, _pageIndex);

        // Assert
        result.Should().BeEquivalentTo(_paginatedAuditList, "the service should return paginated audit records as provided by the repository");
    }

    /// <summary>
    /// Validates that the service returns an empty paginated list when no audit records exist.
    /// </summary>
    [Fact]
    public async Task GetPaginatedLearningComponentAuditAsync_ShouldReturnEmptyPaginatedList_WhenNoRecordsExist()
    {
        // Arrange
        var emptyPaginated = PaginatedList<LearningComponentAudit>.Empty(_pageSize, _pageIndex);
        _auditRepositoryMock
            .Setup(repo => repo.GetPaginatedLearningComponentAuditAsync(_pageSize, _pageIndex))
            .ReturnsAsync(emptyPaginated);

        // Act
        var result = await _serviceUnderTest.GetPaginatedLearningComponentAuditAsync(_pageSize, _pageIndex);

        // Assert
        result.Should().BeEquivalentTo(emptyPaginated, "an empty repository should return an empty paginated list");
    }
}
