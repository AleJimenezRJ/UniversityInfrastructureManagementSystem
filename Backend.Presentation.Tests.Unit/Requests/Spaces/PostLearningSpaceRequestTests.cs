using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.Spaces;

/// <summary>
/// Contains unit tests for the <see cref="PostLearningSpaceRequest"/> class,  verifying its behavior when initialized
/// with a valid <see cref="LearningSpaceDto"/>.
/// </summary>
public class PostLearningSpaceRequestTests
{
    /// <summary>
    /// Represents a valid learning space data transfer object.
    /// </summary>
    public LearningSpaceDto _validLearningSpaceDto;

    /// <summary>
    /// Constructor to initialize a valid <see cref="LearningSpaceDto"/> instance
    /// </summary>
    public PostLearningSpaceRequestTests()
    {
        _validLearningSpaceDto = new LearningSpaceDto
        (
            "Physics Lab",
            "Laboratory",
            30,
            10.0,
            4.0,
            12.0,
            "Blue",
            "Gray",
            "White"
        );
    }

    /// <summary>
    /// Tests that the constructor of <see cref="PostLearningSpaceRequest"/> correctly sets the <c>LearningSpace</c>
    /// property when a valid DTO is provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidDtoProvided_ShouldSetLearningSpaceProperty()
    {
        // Act
        var request = new PostLearningSpaceRequest(_validLearningSpaceDto);

        // Assert
        request.LearningSpace.Should().Be(_validLearningSpaceDto, because: "the LearningSpace property should be set to the provided DTO");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="PostLearningSpaceRequest"/> created with the same DTO have identical
    /// learning spaces. 
    /// </summary>
    [Fact]
    public void Constructor_WhenTwoRequestsCreated_ShouldHaveSameLearningSpace()
    {
        // Arrange
        var request1 = new PostLearningSpaceRequest(_validLearningSpaceDto);
        var request2 = new PostLearningSpaceRequest(_validLearningSpaceDto);

        // Act & Assert
        request1.LearningSpace.Should().Be(request2.LearningSpace, because: "both requests should have the same LearningSpace DTO");
    }
}
