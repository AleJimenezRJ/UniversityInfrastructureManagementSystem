using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

public class GetLearningSpaceResponseTests
{
    /// <summary>
    /// Represents a valid learning space DTO.
    /// </summary>
    private readonly LearningSpaceDto _learningSpaceDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningSpaceResponseTests"/> class.
    /// </summary>
    public GetLearningSpaceResponseTests()
    {
        _learningSpaceDto = new LearningSpaceDto(
            Name: "Physics Lab",
            Type: "Laboratory",
            MaxCapacity: 30,
            Height: 3.5,
            Width: 5.0,
            Length: 10.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "Red"
        );
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetLearningSpaceResponse"/> initializes the <see
    /// cref="GetLearningSpaceResponse.LearningSpace"/> property correctly when a valid DTO is provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidDtoProvided_ShouldInitializeLearningSpace()
    {
        // Act
        var response = new GetLearningSpaceResponse(_learningSpaceDto);

        // Assert
        response.LearningSpace.Should().BeEquivalentTo(_learningSpaceDto, because: "the LearningSpace property should match the provided DTO");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="GetLearningSpaceResponse"/> created with the same DTO are considered equal.
    /// </summary>
    [Fact]
    public void Constructor_WhenTwoResponsesAreCreatedWithSameDto_ShouldBeEqual()
    {
        // Act
        var response1 = new GetLearningSpaceResponse(_learningSpaceDto);
        var response2 = new GetLearningSpaceResponse(_learningSpaceDto);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "two responses with the same DTO should be equal");
    }
}
