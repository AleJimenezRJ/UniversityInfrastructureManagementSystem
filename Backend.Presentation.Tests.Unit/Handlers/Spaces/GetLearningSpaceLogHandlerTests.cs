using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

/// <summary>
/// Unit tests for the <see cref="GetLearningSpaceLogHandler"/> class.
/// These tests verify the different outcomes of retrieving learning space logs,
/// including successful retrieval, empty result, and null result scenarios.
/// </summary>
public class GetLearningSpaceLogHandlerTests
{
    private readonly Mock<ILearningSpaceLogServices> _serviceMock;
    private readonly List<LearningSpaceLog> _exampleLogs;

    public GetLearningSpaceLogHandlerTests()
    {
        _serviceMock = new Mock<ILearningSpaceLogServices>(MockBehavior.Strict);
        _exampleLogs = new List<LearningSpaceLog>
        {
            new LearningSpaceLog
            {
                LearningSpaceLogInternalId = 1,
                Name = "Aula Magna",
                Type = "Auditorium",
                MaxCapacity = 100,
                Width = 10.5m,
                Height = 4.2m,
                Length = 15.0m,
                ColorFloor = "Red",
                ColorWalls = "White",
                ColorCeiling = "Gray",
                ModifiedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                Action = "Created"
            },
            new LearningSpaceLog
            {
                LearningSpaceLogInternalId = 2,
                Name = "Lab 202",
                Type = "Laboratory",
                MaxCapacity = 25,
                Width = 7.5m,
                Height = 3.5m,
                Length = 9.0m,
                ColorFloor = "Blue",
                ColorWalls = "Gray",
                ColorCeiling = "White",
                ModifiedAt = new DateTime(2024, 1, 2, 14, 0, 0, DateTimeKind.Utc),
                Action = "Updated"
            }
        };
    }

    /// <summary>
    /// Verifies that when logs exist, the handler returns a 200 OK response with a response object.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenLogsExist_ReturnsOkWithResponse()
    {
        // Arrange
        _serviceMock.Setup(s => s.ListLearningSpaceLogsAsync()).ReturnsAsync(_exampleLogs);

        // Act
        var result = await GetLearningSpaceLogHandler.HandleAsync(_serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetLearningSpaceLogResponse>>();
        var okResult = result.Result as Ok<GetLearningSpaceLogResponse>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        okResult.Value.Logs.Should().HaveCount(2);
        okResult.Value.Logs[0].Name.Should().Be("Aula Magna");
        okResult.Value.Logs[1].Name.Should().Be("Lab 202");
    }

    /// <summary>
    /// Verifies that when no logs exist, the handler returns a 404 NotFound response with a message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoLogsExist_ReturnsNotFoundWithMessage()
    {
        // Arrange
        _serviceMock.Setup(s => s.ListLearningSpaceLogsAsync()).ReturnsAsync(new List<LearningSpaceLog>());

        // Act
        var result = await GetLearningSpaceLogHandler.HandleAsync(_serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var notFoundResult = result.Result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("No se encontraron registros de cambios en los espacios de aprendizaje.");
    }

    /// <summary>
    /// Verifies that when the service returns null, the handler returns a 404 NotFound response with a message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenServiceReturnsNull_ReturnsNotFoundWithMessage()
    {
        // Arrange
        _serviceMock.Setup(s => s.ListLearningSpaceLogsAsync()).ReturnsAsync((List<LearningSpaceLog>?)null);

        // Act
        var result = await GetLearningSpaceLogHandler.HandleAsync(_serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var notFoundResult = result.Result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("No se encontraron registros de cambios en los espacios de aprendizaje.");
    }
}
