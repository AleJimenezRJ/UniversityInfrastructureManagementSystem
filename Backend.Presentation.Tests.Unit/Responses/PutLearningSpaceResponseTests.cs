using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Contains unit tests for the <see cref="PutLearningSpaceResponse"/> class, verifying its behavior when initialized
/// with various parameters.
/// </summary>
public class PutLearningSpaceResponseTests
{
    /// <summary>
    /// Represents a data transfer object for a learning space.
    /// </summary>
    private readonly LearningSpaceDto _learningSpaceDto;

    /// <summary>
    /// Represents another learning space associated with the current context.
    /// </summary>
    private readonly LearningSpaceDto _otherLearningSpace;

    /// <summary>
    /// Initializes a new instance of the <see cref="PutLearningSpaceResponseTests"/> class.
    /// </summary>
    public PutLearningSpaceResponseTests()
    {
        _learningSpaceDto = new LearningSpaceDto(
            Name: "Chemistry Lab",
            Type: "Laboratory",
            MaxCapacity: 40,
            Height: 3.0,
            Width: 8.0,
            Length: 12.0,
            ColorFloor: "White",
            ColorWalls: "Light Blue",
            ColorCeiling: "White"
        );

        _otherLearningSpace = new LearningSpaceDto(
            Name: "Physics Lab",
            Type: "Laboratory",
            MaxCapacity: 30,
            Height: 3.5,
            Width: 6.0,
            Length: 10.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "Red"
        );
    }

    /// <summary>
    /// Tests that the <see cref="PutLearningSpaceResponse"/> constructor correctly sets the <see
    /// cref="PutLearningSpaceResponse.LearningSpaceDto"/> and <see cref="PutLearningSpaceResponse.Message"/> properties
    /// when provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetLearningSpaceDto()
    {
        // Arrange
        var message = "Learning space updated successfully.";

        // Act
        var response = new PutLearningSpaceResponse(_learningSpaceDto, message);

        // Assert
        response.LearningSpaceDto.Should().BeEquivalentTo(_learningSpaceDto, because: "LearningSpaceDto should match the provided DTO");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="PutLearningSpaceResponse"/> sets the <see
    /// cref="PutLearningSpaceResponse.Message"/> property correctly when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetMessage()
    {
        // Arrange
        var message = "Learning space updated successfully.";

        // Act
        var response = new PutLearningSpaceResponse(_learningSpaceDto, message);

        // Assert
        response.Message.Should().Be(message, because: "Message should match the provided value");
    }

    /// <summary>
    /// Tests that the <see cref="PutLearningSpaceResponse"/> constructor sets the <c>Message</c> property to an empty string
    /// when initialized with an empty message.
    /// </summary>
    [Fact]
    public void Constructor_WhenMessageIsEmpty_ShouldSetMessagePropertyToEmptyString()
    {
        // Arrange
        var message = string.Empty;

        // Act
        var response = new PutLearningSpaceResponse(_learningSpaceDto, message);

        // Assert
        response.Message.Should().BeEmpty(because: "Message should be set to an empty string when provided as such");
    }

    /// <summary>
    /// Verifies that two <see cref="PutLearningSpaceResponse"/> instances with the same learning space and message are equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithSameParameters_ShouldBeEquivalent()
    {
        // Arrange
        var message = "Learning space already exists.";

        // Act
        var response1 = new PutLearningSpaceResponse(_learningSpaceDto, message);
        var response2 = new PutLearningSpaceResponse(_learningSpaceDto, message);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "responses with the same learning space and message should be equivalent");
    }

    /// <summary>
    /// Verifies that two <see cref="PutLearningSpaceResponse"/> instances with different messages are not considered equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithDifferentMessages_ShouldNotBeEquivalent()
    {
        // Arrange
        var response1 = new PutLearningSpaceResponse(_learningSpaceDto, "Updated.");
        var response2 = new PutLearningSpaceResponse(_learningSpaceDto, "Error: already exists.");

        // Assert
        response1.Should().NotBeEquivalentTo(response2, "responses with different messages should not be equivalent");
    }

    /// <summary>
    /// Verifies that two <see cref="PutLearningSpaceResponse"/> instances with different learning spaces are not considered equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithDifferentLearningSpaces_ShouldNotBeEquivalent()
    {
        // Arrange
        var message = "Learning space updated successfully.";

        // Act
        var response1 = new PutLearningSpaceResponse(_learningSpaceDto, message);
        var response2 = new PutLearningSpaceResponse(_otherLearningSpace, message);

        // Assert
        response1.Should().NotBeEquivalentTo(response2, "responses with different learning spaces should not be equivalent");
    }
}
