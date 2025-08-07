using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the PutModifyPersonHandler.
/// </summary>
public class PutModifyPersonHanlderTests
{
    /// <summary>
    /// Mock service for person operations.
    /// </summary>
    private readonly Mock<IPersonService> _mockService = new();

    /// <summary>
    /// Creates a valid request for modifying a person.
    /// </summary>
    /// <param name="identity"> The identity number of the person to be modified.</param>
    /// <returns> A <see cref="PutModifyPersonRequest"/> containing the details of the person to be modified.</returns>
    private static PutModifyPersonRequest CreateValidRequest(string identity)
    {
        return new PutModifyPersonRequest(
            new PersonDto(
                Id: 1,
                Email: "new@email.com",
                FirstName: "Updated",
                LastName: "Person",
                Phone: "8888-8888",
                BirthDate: new DateOnly(1990, 1, 1),
                IdentityNumber: identity
            )
        );
    }

    /// <summary>
    /// Tests the HandleAsync method of PutModifyPersonHandler to ensure it returns NotFound when the person does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var identity = "1-2345-6789";
        _mockService
        .Setup(s => s.GetPersonByIdAsync(identity))
        .ReturnsAsync((Person?)null);

        var request = CreateValidRequest(identity);

        // Act
        var result = await PutModifyPersonHandler.HandleAsync(
            _mockService.Object,
            identity,
            request);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value
        .ToLowerInvariant()
        .Should()
        .Contain("not found");
    }

    /// <summary>
    /// Tests the HandleAsync method of PutModifyPersonHandler to ensure it returns BadRequest when no fields are modified.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenNoFieldsAreModified()
    {
        var identity = "1-2345-6789";
        var idValue = IdentityNumber.Create(identity);

        var existingPerson = new Person(
            Email.Create("same@email.com"),
            "Same",
            "Person",
            Phone.Create("8888-8888"),
            BirthDate.Create(new DateOnly(1990, 1, 1)),
            idValue
        );

        _mockService.Setup(s => s.GetPersonByIdAsync(idValue.Value)).ReturnsAsync(existingPerson);

        var sameRequest = new PutModifyPersonRequest(
            new PersonDto(
                Id: 1,
                Email: "same@email.com",
                FirstName: "Same",
                LastName: "Person",
                Phone: "8888-8888",
                BirthDate: new DateOnly(1990, 1, 1),
                IdentityNumber: identity
            )
        );

        var result = await PutModifyPersonHandler.HandleAsync(
            _mockService.Object,
            identity,
            sameRequest);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = ((BadRequest<List<ValidationError>>)result.Result!).Value;
        errors.Should().Contain(e => e.Message.ToLowerInvariant().Contains("at least one field must be modified"));
    }

    /// <summary>
    /// Tests the HandleAsync method of PutModifyPersonHandler to ensure it returns Ok when a person is modified successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenPersonIsModified()
    {
        var identity = "1-2345-6789";
        var idValue = IdentityNumber.Create(identity);

        var existingPerson = new Person(
            Email.Create("old@email.com"),
            "Old",
            "Person",
            Phone.Create("1111-1111"),
            BirthDate.Create(new DateOnly(1980, 1, 1)),
            idValue
        );

        _mockService.Setup(s => s.GetPersonByIdAsync(idValue.Value)).ReturnsAsync(existingPerson);
        _mockService.Setup(s => s.ModifyPersonAsync(idValue.Value, It.IsAny<Person>()))
            .ReturnsAsync(true);

        var request = CreateValidRequest(identity);

        var result = await PutModifyPersonHandler.HandleAsync(
            _mockService.Object,
            identity,
            request);

        result.Result.Should().BeOfType<Ok<PutModifyPersonResponse>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of PutModifyPersonHandler to ensure it returns BadRequest when the identity number is invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenInvalidIdentityNumber()
    {
        var result = await PutModifyPersonHandler.HandleAsync(
            _mockService.Object,
            "invalid-id",
            CreateValidRequest("invalid-id"));

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenDuplicatedEntityExceptionThrown()
    {
        var identity = "1-2345-6789";
        var idValue = IdentityNumber.Create(identity);

        var existingPerson = new Person(
            Email.Create("old@email.com"),
            "Old",
            "Person",
            Phone.Create("1111-1111"),
            BirthDate.Create(new DateOnly(1980, 1, 1)),
            idValue
        );

        _mockService.Setup(s => s.GetPersonByIdAsync(idValue.Value)).ReturnsAsync(existingPerson);
        _mockService.Setup(s => s.ModifyPersonAsync(idValue.Value, It.IsAny<Person>()))
            .ThrowsAsync(new DuplicatedEntityException("Duplicate"));

        var result = await PutModifyPersonHandler.HandleAsync(
            _mockService.Object,
            identity,
            CreateValidRequest(identity));

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = ((BadRequest<List<ValidationError>>)result.Result!).Value;
        errors.Should().Contain(e => e.Message.ToLowerInvariant().Contains("duplicate"));
    }
}
