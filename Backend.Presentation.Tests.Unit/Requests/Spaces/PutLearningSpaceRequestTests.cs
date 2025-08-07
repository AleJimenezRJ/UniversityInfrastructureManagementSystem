using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.Spaces;

/// <summary>
/// Contains unit tests for the <see cref="PutLearningSpaceRequest"/> class, ensuring that its constructor correctly
/// initializes the <c>LearningSpace</c> property with a valid <see cref="LearningSpaceDto"/>.
/// </summary>
public class PutLearningSpaceRequestTests
{
    /// <summary>
    /// Represents a valid learning space data transfer object.
    /// </summary>
    public LearningSpaceDto _validLearningSpaceDto;

    /// <summary>
    /// Constructor to initialize a valid <see cref="LearningSpaceDto"/> instance
    /// </summary>
    public PutLearningSpaceRequestTests()
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
    /// Tests that the constructor of <see cref="PutLearningSpaceRequest"/> correctly sets the <c>LearningSpace</c>
    /// property when a valid DTO is provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidDtoProvided_ShouldSetLearningSpaceProperty()
    {
        // Act
        var request = new PutLearningSpaceRequest(_validLearningSpaceDto);

        // Assert
        request.LearningSpace.Should().Be(_validLearningSpaceDto, because: "the LearningSpace property should be set to the provided DTO");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="PutLearningSpaceRequest"/> created with the same DTO have identical
    /// learning spaces. 
    /// </summary>
    [Fact]
    public void Constructor_WhenTwoRequestsCreated_ShouldHaveSameLearningSpace()
    {
        // Arrange
        var request1 = new PutLearningSpaceRequest(_validLearningSpaceDto);
        var request2 = new PostLearningSpaceRequest(_validLearningSpaceDto);

        // Act & Assert
        request1.LearningSpace.Should().Be(request2.LearningSpace, because: "both requests should have the same LearningSpace DTO");
    }
}
