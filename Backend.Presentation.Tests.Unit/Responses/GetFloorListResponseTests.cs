using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Contains unit tests for the <see cref="GetFloorListResponse"/> class, verifying its behavior when initialized with
/// different collections of floor data transfer objects.
/// </summary>
public class GetFloorListResponseTests
{
    /// <summary>
    /// Represents a collection of floor data transfer objects.
    /// </summary>
    private List<FloorDto> _floors;

    /// <summary>
    /// Represents a collection of additional floors associated with the current context.
    /// </summary>
    private List<FloorDto> _otherFloors;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFloorListResponseTests"/> class.
    /// </summary>
    public GetFloorListResponseTests()
    {
        _floors = new List<FloorDto>
        {
            new FloorDto(FloorId: 1, FloorNumber: 1),
            new FloorDto(FloorId: 2, FloorNumber: 2)
        };

        _otherFloors = new List<FloorDto>
        {
            new FloorDto(FloorId: 3, FloorNumber: 3),
            new FloorDto(FloorId: 4, FloorNumber: 4)
        };
    }

    /// <summary>
    /// Tests that the <see cref="GetFloorListResponseTests"/> constructor correctly assigns the provided list of floors to
    /// the <see cref="GetFloorListResponseTests.Floors"/> property.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidFloorsAreProvided_AssignsFloorsCorrectly()
    {
        // Act
        var response = new GetFloorListResponse(_floors);

        // Assert
        response.Floors.Should().BeEquivalentTo(_floors, because: "the Floors property should match the provided list of floors");
    }

    /// <summary>
    /// Tests that the <see cref="GetFloorListResponseTests"/> constructor initializes the <c>Floors</c> property to an empty
    /// list when provided with an empty collection.
    /// </summary>
    [Fact]
    public void Constructor_WhenEmptyFloorsAreProvided_ShouldInitializeEmptyList()
    {
        // Act
        var response = new GetFloorListResponse(new List<FloorDto>());

        // Assert
        response.Floors.Should().BeEmpty(because: "the Floors property should be initialized to an empty list when no floors are provided");
    }

    /// <summary>
    /// Verifies that two <see cref="GetFloorListResponseTests"/> instances constructed with the same list of floors are
    /// equivalent.
    /// </summary>
    [Fact]
    public void Constructor_TwoResponsesWithSameFloors_ShouldBeEquivalent()
    {
        // Act
        var response1 = new GetFloorListResponse(_floors);
        var response2 = new GetFloorListResponse(_floors);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "responses with the same list of floors should be equivalent");
    }

    /// <summary>
    /// Verifies that two <see cref="GetFloorListResponseTests"/> instances with different floor lists are not equivalent.
    /// </summary>
    [Fact]
    public void TwoResponses_WithDifferentFloors_ShouldNotBeEquivalent()
    {
        // Arrange
        var response1 = new GetFloorListResponse(_floors);
        var response2 = new GetFloorListResponse(_otherFloors);

        // Assert
        response1.Should().NotBeEquivalentTo(response2, because: "responses with different lists of floors should not be equivalent");
    }
}
