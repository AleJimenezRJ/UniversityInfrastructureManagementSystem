using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

public class GetLearningSpaceLogResponseTests
{
    private readonly List<LearningSpaceLogDto> _logs;

    private readonly List<LearningSpaceLogDto> _otherLogs;
    public GetLearningSpaceLogResponseTests()
    {
        _logs = new List<LearningSpaceLogDto>
        {
            new LearningSpaceLogDto(
                LearningSpaceLogInternalId: 1,
                Name: "Lab 101",
                MaxCapacity: 30,
                Type: "Laboratory",
                Width: 5.0m,
                Height: 3.0m,
                Length: 6.0m,
                ColorFloor: "Gray",
                ColorWalls: "White",
                ColorCeiling: "Red",
                ModifiedAt: new DateTime(2025, 1, 1, 10, 0, 0),
                Action: "CREATED"
            ),
            new LearningSpaceLogDto(
                LearningSpaceLogInternalId: 2,
                Name: "Lab 101",
                MaxCapacity: 32,
                Type: "Laboratory",
                Width: 5.5m,
                Height: 3.0m,
                Length: 6.0m,
                ColorFloor: "Gray",
                ColorWalls: "White",
                ColorCeiling: "Red",
                ModifiedAt: new DateTime(2025, 6, 1, 10, 0, 0),
                Action: "UPDATED"
            )
        };

        _otherLogs = new List<LearningSpaceLogDto>
        {
            new LearningSpaceLogDto(
                LearningSpaceLogInternalId: 3,
                Name: "Classroom A",
                MaxCapacity: 25,
                Type: "Classroom",
                Width: 7.0m,
                Height: 3.2m,
                Length: 8.0m,
                ColorFloor: "Blue",
                ColorWalls: "Green",
                ColorCeiling: "White",
                ModifiedAt: new DateTime(2024, 1, 10, 8, 0, 0),
                Action: "CREATED"
            )
        };
    }

    [Fact]
    public void Constructor_WhenCorrectLogsAreProvided_AssignsLogsCorrectly()
    {
        // Act
        var response = new GetLearningSpaceLogResponse(_logs);

        // Assert
        response.Logs.Should().BeEquivalentTo(_logs, because: "the Logs property should match the provided list of logs");
    }

    [Fact]
    public void Constructor_WhenEmptyListIsProvided_AssignsEmptyLogs()
    {
        // Act
        var response = new GetLearningSpaceLogResponse(new List<LearningSpaceLogDto>());

        // Assert
        response.Logs.Should().BeEmpty(because: "the Logs property should be initialized to an empty list when no logs are provided");
    }

    [Fact]
    public void Constructor_TwoResponsesWithSameLogs_ShouldBeEquivalent()
    {
        // Act
        var response1 = new GetLearningSpaceLogResponse(_logs);
        var response2 = new GetLearningSpaceLogResponse(_logs);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "two responses with the same list of logs should be equivalent"); 
    }

    [Fact]
    public void Constructor_TwoResponsesWithDifferentLogs_ShouldNotBeEquivalent()
    {
        // Act
        var response1 = new GetLearningSpaceLogResponse(_logs);
        var response2 = new GetLearningSpaceLogResponse(_otherLogs);

        // Assert
        response1.Should().NotBeEquivalentTo(response2, because: "responses with different lists of logs should not be equivalent");
    }
}
