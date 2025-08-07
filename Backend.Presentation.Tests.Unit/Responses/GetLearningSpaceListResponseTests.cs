using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Provides unit tests for the <see cref="GetLearningSpaceListResponse"/> class,  ensuring that the constructor
/// correctly assigns the list of learning spaces  and that instances with equivalent lists are considered equivalent.
/// </summary>
public class GetLearningSpaceListResponseTests
{
    /// <summary>
    /// Represents a collection of learning spaces.
    /// </summary>
    private readonly List<LearningSpaceListDto> _learningSpaces;

    /// <summary>
    /// Represents a collection of additional learning spaces.
    /// </summary>
    private readonly List<LearningSpaceListDto> _otherLearningSpaces;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningSpaceListResponseTests"/> class.
    /// </summary>
    public GetLearningSpaceListResponseTests()
    {
        _learningSpaces = new List<LearningSpaceListDto>
        {
            new LearningSpaceListDto(
                LearningSpaceId: 1,
                Name: "Lab 101",
                Type: "Laboratory"
            ),
            new LearningSpaceListDto(
                LearningSpaceId: 2,
                Name: "Classroom 1",
                Type: "Classroom"
            )
        };

        _otherLearningSpaces = new List<LearningSpaceListDto>
        {
            new LearningSpaceListDto(
                LearningSpaceId: 3,
                Name: "Auditorium",
                Type: "Auditorium"
            )
        };
    }

    /// <summary>
    /// Tests that the <see cref="GetLearningSpaceListResponse"/> constructor correctly assigns the provided list of
    /// learning spaces to the <see cref="GetLearningSpaceListResponse.LearningSpaces"/> property.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidList_AssignsLearningSpacesCorrectly()
    {
        // Act
        var response = new GetLearningSpaceListResponse(_learningSpaces);

        // Assert
        response.LearningSpaces.Should().BeEquivalentTo(_learningSpaces, because: "the LearningSpaces property should match the provided list of learning spaces");
    }

    /// <summary>
    /// Tests that the <see cref="GetLearningSpaceListResponse"/> constructor correctly initializes the <see
    /// cref="GetLearningSpaceListResponse.LearningSpaces"/> property to an empty list when provided with an empty list
    /// of <see cref="LearningSpaceListDto"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenEmptyList_AssignsLearningSpacesCorrectly()
    {
        // Act
        var response = new GetLearningSpaceListResponse(new List<LearningSpaceListDto>());

        // Assert
        response.LearningSpaces.Should().BeEmpty(because: "the LearningSpaces property should be initialized to an empty list when no learning spaces are provided");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="GetLearningSpaceListResponse"/> initialized with the same list of
    /// learning spaces are equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithSameLearningSpaces_ShouldBeEquivalent()
    {
        // Act
        var response1 = new GetLearningSpaceListResponse(_learningSpaces);
        var response2 = new GetLearningSpaceListResponse(_learningSpaces);

        // Assert
        response1.Should().BeEquivalentTo(response2, "responses with the same list of learning spaces should be equivalent");
    }

    /// <summary>
    /// Verifies that two <see cref="GetLearningSpaceListResponse"/> instances with different learning spaces are not
    /// equivalent.
    /// </summary>
    [Fact]
    public void Constrructor_TwoResponsesWithDifferentLearningSpaces_ShouldNotBeEquivalent()
    {
        // Act
        var response1 = new GetLearningSpaceListResponse(_learningSpaces);
        var response2 = new GetLearningSpaceListResponse(_otherLearningSpaces);

        // Assert
        response1.Should().NotBeEquivalentTo(response2, "responses with different lists of learning spaces should not be equivalent");
    }
}
