using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.ComponentManagement;
/// <summary>
/// Unit tests for the Whiteboard repository.
/// Theres not an specific PBI for listing whiteboards, but it is a common functionality in the system but it is for adding and updating whiteboards. #9, #27, #21
/// Tasks: Add unit tests for the WhiteboardServices methods GetWhiteboardAsync, AddWhiteboardAsync and UpdateWhiteboardAsync.
/// Participants: Sebastian Arce, Marcelo Picado
/// </summary>
public class WhiteBoardServicesTests
{

    private readonly Mock<IWhiteboardRepository> _whiteboardRepositoryMock;
    private readonly WhiteboardServices _whiteboardServiceUnderTest;
    private readonly Whiteboard _whiteboard;
    private readonly Whiteboard _whiteboard2;
    private readonly List<Whiteboard> nonEmptyList;
    private readonly List<Whiteboard> emptyList;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhiteBoardServicesTests"/> class.
    /// </summary>
    public WhiteBoardServicesTests()
    {
        _whiteboardRepositoryMock = new Mock<IWhiteboardRepository>(
            behavior: MockBehavior.Strict);
        
        _whiteboard = new Whiteboard
        {
            MarkerColor = Colors.Create("Red"),
            ComponentId = 1,
            Orientation = Orientation.Create("South"),
            Position = Coordinates.Create(12, 20, 0),
            Dimensions = Dimension.Create(10, 18, 2)

        };
        _whiteboard2 = new Whiteboard
        {
            MarkerColor = Colors.Create("Blue"),
            ComponentId = 1,
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(15, 12, 0),
            Dimensions = Dimension.Create(32, 34, 1)

        };
        _whiteboardServiceUnderTest = new WhiteboardServices(_whiteboardRepositoryMock.Object);
        nonEmptyList = new List<Whiteboard> { _whiteboard, _whiteboard2 };
        emptyList = new List<Whiteboard>();

    }
    /// <summary>
    /// Tests the GetWhiteboardAsync method of the WhiteboardServices class when whiteboards exist.
    /// </summary>
    /// <returns> should return a non-empty list of whiteboards when they exist.
    /// </returns>
    [Fact]
    public async Task GetWhiteboardAsync_ShouldReturnNonEmptyList_WhenWhiteboardsExist()
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(WhiteBoardServices => WhiteBoardServices.GetAllAsync())
            .ReturnsAsync(nonEmptyList);
        
        // Act
        var result = await _whiteboardServiceUnderTest.GetWhiteboardAsync();

        // Assert
        result.Should().BeSameAs(nonEmptyList, because: "The repository returns a non-empty list when whiteboards exist");
    }

    /// <summary>
    /// Tests the GetWhiteboardAsync method of the WhiteboardServices class when no whiteboards exist.
    /// </summary>
    /// <returns> should return an empty list when no whiteboards exist.
    /// </returns>
    [Fact]
    public async Task GetWhiteboardAsync_ShouldReturnEmptyList_WhenNoWhiteboardsExist()
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(WhiteBoardServices => WhiteBoardServices.GetAllAsync())
            .ReturnsAsync(emptyList);

        // Act
        var result = await _whiteboardServiceUnderTest.GetWhiteboardAsync();

        // Assert
        result.Should().BeSameAs(emptyList,because: "The repository returns an empty list when no whiteboards exist");
    }

    /// <summary>
    /// Tests the AddWhiteboardAsync method of the WhiteboardServices class.
    /// </summary>
    /// <param name="expectedResult"></param>
    /// <returns></returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddWhiteboardAsync_ShouldReturnTrue_WhenWhiteboardIsAddedSuccessfully(bool expectedResult)
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(repo => repo.AddComponentAsync(It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(expectedResult);
        // Act
        var result = await _whiteboardServiceUnderTest.AddWhiteboardAsync(1, _whiteboard);
        // Assert
        result.Should().Be(expectedResult, because: "The repository returns true when the whiteboard is added successfully");
    }

    /// <summary>
    /// Tests the AddWhiteboardAsync method of the WhiteboardServices class when given valid parameters.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddWhiteboardAsync_WhenGivenValidParameters_CallsCreateOnRepository()
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(repo => repo.AddComponentAsync(It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(false);
        // Act
        var result = await _whiteboardServiceUnderTest.AddWhiteboardAsync(1, _whiteboard);

        // Assert
        _whiteboardRepositoryMock.Verify(
            whiteboardRepository => whiteboardRepository.AddComponentAsync(1, _whiteboard),
            Times.Once,
            failMessage: "Service should always call CreateAsync on repository when adding a new Whiteboard");
    }

    /// <summary>
    /// Tests the UpdateWhiteboardAsync method of the WhiteboardServices class.
    /// </summary>
    /// <param name="expectedResult"></param>
    /// <returns></returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateWhiteboardAsync_ShouldReturnTrue_WhenWhiteboardIsUpdatedSuccessfully(bool expectedResult)
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _whiteboardServiceUnderTest.UpdateWhiteboardAsync(1, 1, _whiteboard);

        // Assert
        result.Should().Be(expectedResult, because: "The repository returns true when the whiteboard is updated successfully");
    }

    /// <summary>
    /// Tests the UpdateWhiteboardAsync method of the WhiteboardServices class when given valid parameters.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateWhiteboardAsync_WhenGivenValidParameters_CallsCreateOnRepository()
    {
        // Arrange
        _whiteboardRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(false);
        // Act
        var result = await _whiteboardServiceUnderTest.UpdateWhiteboardAsync(1, 1, _whiteboard);

        // Assert
        _whiteboardRepositoryMock.Verify(
            whiteboardRepository => whiteboardRepository.UpdateAsync(1, 1, _whiteboard),
            Times.Once,
            failMessage: "Service should always call CreateAsync on repository when adding a new Whiteboard");
    }

}
