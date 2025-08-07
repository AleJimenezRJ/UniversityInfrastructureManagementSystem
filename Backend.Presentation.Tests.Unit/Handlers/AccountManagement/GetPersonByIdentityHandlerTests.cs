using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetPersonByIdentityHandler"/> class.
/// Verifies the behavior of the handler when retrieving a person by identity number,
/// including validation, not found, domain exception, and success scenarios.
/// </summary>
public class GetPersonByIdentityHandlerTests
{
    private readonly Mock<IPersonService> _serviceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdentityHandlerTests"/> class.
    /// Sets up a strict mock for <see cref="IPersonService"/>.
    /// </summary>
    public GetPersonByIdentityHandlerTests()
    {
        _serviceMock = new Mock<IPersonService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that an invalid identity number format returns a BadRequest with validation errors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_InvalidIdentityNumberFormat_ReturnsBadRequest()
    {
        // Arrange
        var invalidIdentity = "123";

        // Act
        var result = await GetPersonByIdentityHandler.HandleAsync(invalidIdentity, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value;
        errors.Should().ContainSingle(e => e.Parameter == "IdentityNumber");
    }

    /// <summary>
    /// Tests that when the person is not found, a NotFound result is returned.
    /// </summary>
    [Fact]
    public async Task HandleAsync_IdentityNotFound_ReturnsNotFound()
    {
        // Arrange
        var identity = "1-1111-1111";
        var identityNumber = IdentityNumber.Create(identity);

        _serviceMock.Setup(s => s.GetPersonByIdAsync(identityNumber.Value))
            .ThrowsAsync(new NotFoundException("Person not found"));

        // Act
        var result = await GetPersonByIdentityHandler.HandleAsync(identity, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var value = (result.Result as NotFound<string>)?.Value;
        value.Should().Contain("was not found");
    }

    /// <summary>
    /// Tests that a domain exception returns a BadRequest with a domain error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_DomainException_ReturnsBadRequestWithDomainError()
    {
        // Arrange
        var identity = "1-1111-1111";
        var identityNumber = IdentityNumber.Create(identity);

        _serviceMock.Setup(s => s.GetPersonByIdAsync(identityNumber.Value))
            .ThrowsAsync(new DomainException("Domain error"));

        // Act
        var result = await GetPersonByIdentityHandler.HandleAsync(identity, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value;
        errors.Should().ContainSingle(e => e.Parameter == "Domain" && e.Message == "Domain error");
    }

    /// <summary>
    /// Tests that an unexpected exception returns a BadRequest with exception details.
    /// </summary>
    [Fact]
    public async Task HandleAsync_UnexpectedException_ReturnsBadRequestWithExceptionDetails()
    {
        // Arrange
        var identity = "1-1111-1111";
        var identityNumber = IdentityNumber.Create(identity);

        _serviceMock.Setup(s => s.GetPersonByIdAsync(identityNumber.Value))
            .ThrowsAsync(new Exception("Something broke"));

        // Act
        var result = await GetPersonByIdentityHandler.HandleAsync(identity, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value;
        errors.Should().Contain(e => e.Parameter == "ExceptionDetails" && e.Message == "Something broke");
    }

    /// <summary>
    /// Tests that a valid identity returns an Ok result with the person data.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ValidIdentity_ReturnsOk()
    {
        // Arrange
        var identity = "1-1111-1111";
        var identityNumber = IdentityNumber.Create(identity);
        var person = new Person(
            Email.Create("test@ucr.ac.cr"),
            "Gael",
            "Alpizar",
            Phone.Create("8888-8888"),
            BirthDate.Create(new DateOnly(2000, 1, 1)),
            IdentityNumber.Create(identity),
            1);

        _serviceMock.Setup(s => s.GetPersonByIdAsync(identityNumber.Value))
            .ReturnsAsync(person);

        // Act
        var result = await GetPersonByIdentityHandler.HandleAsync(identity, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetPersonByIdentityResponse>>();
        var response = (result.Result as Ok<GetPersonByIdentityResponse>)?.Value;
        response.Should().NotBeNull();
        response!.Person.IdentityNumber.Should().Be(identity);
    }
}
