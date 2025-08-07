using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Provides unit tests for the <see cref="DeleteFloorResponse"/> class, ensuring that its constructor and properties
/// behave as expected under various conditions.
/// </summary>
public class DeleteFloorResponseTests
{
    /// <summary>
    /// Tests that the <see cref="DeleteFloorResponse"/> constructor correctly sets the <see
    /// cref="DeleteFloorResponse.Message"/> property when a message is provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenMessageProvided_ShouldSetMessageProperty()
    {
        // Arrange
        var message = "Floor deleted successfully.";

        // Act
        var response = new DeleteFloorResponse(message);

        // Assert
        response.Message.Should().Be(message, because: "message should be set to the provided value");
    }

    /// <summary>
    /// Tests that the <see cref="DeleteFloorResponse"/> constructor sets the <c>Message</c> property to an empty string
    /// when initialized with an empty message.
    /// </summary>
    [Fact]
    public void Constructor_WhenMessageIsEmpty_ShouldSetMessagePropertyToEmptyString()
    {
        // Arrange
        var message = string.Empty;

        // Act
        var response = new DeleteFloorResponse(message);

        // Assert
        response.Message.Should().BeEmpty(because: "message should be set to an empty string when provided as such");
    }

    /// <summary>
    /// Verifies that two <see cref="DeleteFloorResponse"/> instances with the same message are equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithSameMessage_ShouldBeEquivalent()
    {
        // Arrange
        var message = "Floor not found.";

        // Act
        var response1 = new DeleteFloorResponse(message);
        var response2 = new DeleteFloorResponse(message);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "responses with the same message should be equivalent");
    }

    /// <summary>
    /// Verifies that two <see cref="DeleteFloorResponse"/> instances with different messages are not considered
    /// equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithDifferentMessages_ShouldNotBeEquivalent()
    {
        // Arrange
        var response1 = new DeleteFloorResponse("Deleted.");
        var response2 = new DeleteFloorResponse("Error: not found.");

        // Assert
        response1.Should().NotBeEquivalentTo(response2, "responses with different messages should not be equivalent");
    }
}
