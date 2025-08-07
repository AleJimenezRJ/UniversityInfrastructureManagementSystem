using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;
/// <summary>
/// Unit tests for <see cref="PostCampusHandler"/>, which handles HTTP POST requests
/// </summary>
public class PostCampusHandlerTests
{
                
    private readonly Mock<IUniversityServices> _universityServiceMock;
    private readonly Mock<ICampusServices> _campusServiceMock;

    private readonly AddCampusDto _validDto;
    private readonly AddCampusDto _invalidDto;
    /// <summary>
    /// Initializes the mock services and DTOs used for testing <see cref="PostCampusHandler"/>.
    /// </summary>
    public PostCampusHandlerTests()
    {
        _universityServiceMock = new Mock<IUniversityServices>(MockBehavior.Strict);
        _campusServiceMock = new Mock<ICampusServices>(MockBehavior.Strict);

        _validDto = new AddCampusDto(
            Name: "Pacifico",
            Location: "Puntarenas",
            University: new AddCampusUniversityDto("UCR")
        );

        _invalidDto = new AddCampusDto(
            Name: "", // Invalid
            Location: "Puntarenas",
            University: new AddCampusUniversityDto("UCR")
        );
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenSuccessful_ShouldReturnOk()
    {
        // Arrange
        var university = new University(new EntityName("UCR"), new EntityLocation("San José"));

        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        _campusServiceMock.Setup(x => x.AddCampusAsync(It.IsAny<Campus>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _validDto);

        // Assert
        var ok = Assert.IsType<Ok<PostCampusResponse>>(result.Result);
        ok.Value.Campus.Name.Should().Be("Pacifico");
        ok.Value.Campus.Location.Should().Be("Puntarenas");
        ok.Value.Campus.University.Name.Should().Be("UCR");
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> returns a BadRequest
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenUniversityDoesNotExist_ShouldReturnBadRequest()
    {
        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync((University?)null);

        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _validDto);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value.ErrorMessages.Should().Contain("The specified university does not exist.");
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> returns a BadRequest
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusIsInvalid_ShouldReturnBadRequest()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("SJ"));

        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _invalidDto);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value.ErrorMessages.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> returns a Conflict
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusAlreadyExists_ShouldReturnConflict()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("SJ"));

        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        _campusServiceMock.Setup(x => x.AddCampusAsync(It.IsAny<Campus>()))
            .ThrowsAsync(new DuplicatedEntityException("Campus already exists."));

        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value.ErrorMessages.Should().Contain("Campus already exists.");
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> returns a Conflict
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenAddCampusFails_ShouldReturnConflict()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("SJ"));

        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        _campusServiceMock.Setup(x => x.AddCampusAsync(It.IsAny<Campus>()))
            .ReturnsAsync(false);

        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value.ErrorMessages.Should().Contain("Campus could not be added.");
    }

    /// <summary>
    /// Tests that <see cref="PostCampusHandler.HandleAsync"/> does not call AddCampus
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldNotCallAddCampus_WhenDtoIsInvalid()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("SJ"));

        _universityServiceMock.Setup(x => x.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        var result = await PostCampusHandler.HandleAsync(
            _campusServiceMock.Object,
            _universityServiceMock.Object,
            _invalidDto);

        _campusServiceMock.Verify(x => x.AddCampusAsync(It.IsAny<Campus>()), Times.Never);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value.ErrorMessages.Should().NotBeEmpty();
    }
}
