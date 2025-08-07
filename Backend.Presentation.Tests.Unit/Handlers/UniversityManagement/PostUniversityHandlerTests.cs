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

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

public class PostUniversityHandlerTests
{
    private readonly Mock<IUniversityServices> _universityServiceMock;

    private readonly UniversityDto _validDto;
    private readonly UniversityDto _invalidDto;

    public PostUniversityHandlerTests()
    {
        _universityServiceMock = new Mock<IUniversityServices>(MockBehavior.Strict);

        _validDto = new UniversityDto(
            Name: "UCR",
            Country: "Costa Rica"
        );

        _invalidDto = new UniversityDto(
            Name: "",  // Invalid name (will trigger ValidationException)
            Country: "Costa Rica"
        );
    }

    /// <summary>
    /// Ensures that the handler returns BadRequest when the input DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenSuccessful_ShouldReturnOk()
    {
        // Arrange
        // Simulate successful university creation (returns true).
        _universityServiceMock.Setup(x => x.AddUniversityAsync(It.IsAny<University>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostUniversityHandler.HandleAsync(
            _universityServiceMock.Object,
            _validDto);

        // Assert
        var ok = Assert.IsType<Ok<PostUniversityResponse>>(result.Result);
        ok.Value!.University.Name.Should().Be("UCR");
        ok.Value.University.Country.Should().Be("Costa Rica");
    }


    /// <summary>
    /// Ensures that the handler returns Conflict when the service fails to persist the university.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenAdditionFails_ShouldReturnConflict()
    {
        _universityServiceMock.Setup(x => x.AddUniversityAsync(It.IsAny<University>()))
            .ReturnsAsync(false);

        var result = await PostUniversityHandler.HandleAsync(
            _universityServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("University could not be added.");
    }

    /// <summary>
    /// Ensures that the handler returns Conflict when the university already exists.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUniversityAlreadyExists_ShouldReturnConflict()
    {
        _universityServiceMock.Setup(x => x.AddUniversityAsync(It.IsAny<University>()))
            .ThrowsAsync(new DuplicatedEntityException("University already exists."));

        var result = await PostUniversityHandler.HandleAsync(
            _universityServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("University already exists.");
    }

    /// <summary>
    /// Ensures that the handler returns BadRequest when the input DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnValidationError()
    {
        var result = await PostUniversityHandler.HandleAsync(
            _universityServiceMock.Object,
            _invalidDto);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="PostUniversityHandler.HandleAsync(IUniversityServices, PostUniversityDto)"/>
    /// does not call the service and returns <see cref="BadRequest{T}"/> when the input DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldNotCallService_WhenDtoIsInvalid()
    {
        var result = await PostUniversityHandler.HandleAsync(_universityServiceMock.Object, _invalidDto);

        _universityServiceMock.Verify(x => x.AddUniversityAsync(It.IsAny<University>()), Times.Never);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().NotBeEmpty();
    }

}