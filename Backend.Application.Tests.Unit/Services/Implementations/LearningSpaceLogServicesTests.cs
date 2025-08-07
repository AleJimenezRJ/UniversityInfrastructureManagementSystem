using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/// <summary>
/// Unit tests for the LearningSpaceLogServices class.
/// </summary>
public class LearningSpaceLogServicesTests
{
    // Constants for test data
    private const int LogId1 = 1;
    private const int LogId2 = 2;
    private const string Name1 = "Aula 1";
    private const string Name2 = "Lab 2";
    private const string TypeClassroom = "Classroom";
    private const string TypeLab = "Laboratory";
    private const int Capacity1 = 30;
    private const int Capacity2 = 20;
    private const decimal Width1 = 5;
    private const decimal Width2 = 6;
    private const decimal Height1 = 3;
    private const decimal Height2 = 3;
    private const decimal Length1 = 7;
    private const decimal Length2 = 8;
    private const string ColorFloor1 = "Blue";
    private const string ColorFloor2 = "Gray";
    private const string ColorWalls = "White";
    private const string ColorCeiling = "White";
    private const string ActionCreated = "Created";
    private const string ActionUpdated = "Updated";

    private readonly Mock<ILearningSpaceLogRepository> _learningSpaceLogRepositoryMock;
    private readonly LearningSpaceLogServices _serviceUnderTest;
    private readonly List<LearningSpaceLog> _logsList;
    private readonly List<LearningSpaceLog> _emptyLogsList;
    private readonly DateTime _now;

    public LearningSpaceLogServicesTests()
    {
        _learningSpaceLogRepositoryMock = new Mock<ILearningSpaceLogRepository>(MockBehavior.Strict);
        _serviceUnderTest = new LearningSpaceLogServices(_learningSpaceLogRepositoryMock.Object);
        _now = DateTime.UtcNow;
        _logsList = new List<LearningSpaceLog>
        {
            new LearningSpaceLog
            {
                LearningSpaceLogInternalId = LogId1,
                Name = Name1,
                Type = TypeClassroom,
                MaxCapacity = Capacity1,
                Width = Width1,
                Height = Height1,
                Length = Length1,
                ColorFloor = ColorFloor1,
                ColorWalls = ColorWalls,
                ColorCeiling = ColorCeiling,
                ModifiedAt = _now,
                Action = ActionCreated
            },
            new LearningSpaceLog
            {
                LearningSpaceLogInternalId = LogId2,
                Name = Name2,
                Type = TypeLab,
                MaxCapacity = Capacity2,
                Width = Width2,
                Height = Height2,
                Length = Length2,
                ColorFloor = ColorFloor2,
                ColorWalls = ColorWalls,
                ColorCeiling = ColorCeiling,
                ModifiedAt = _now.AddMinutes(-10),
                Action = ActionUpdated
            }
        };
        _emptyLogsList = new List<LearningSpaceLog>();
    }

    /// <summary>
    /// Tests that the service returns a list of logs when the repository returns logs.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenRepositoryReturnsLogs_ReturnsLogsList()
    {
        // Arrange
        _learningSpaceLogRepositoryMock
            .Setup(repo => repo.ListLearningSpaceLogsAsync())
            .ReturnsAsync(_logsList);

        // Act
        var result = await _serviceUnderTest.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().BeEquivalentTo(_logsList);
    }

    /// <summary>
    /// Tests that the service returns an empty list when the repository returns an empty list.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _learningSpaceLogRepositoryMock
            .Setup(repo => repo.ListLearningSpaceLogsAsync())
            .ReturnsAsync(_emptyLogsList);

        // Act
        var result = await _serviceUnderTest.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that the service propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        _learningSpaceLogRepositoryMock
            .Setup(repo => repo.ListLearningSpaceLogsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _serviceUnderTest.ListLearningSpaceLogsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
