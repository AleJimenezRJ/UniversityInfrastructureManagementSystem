using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// A test class for the GetAllPeopleHandler.
/// </summary>
public class GetAllPeopleHandlerTests
{
    /// <summary>
    /// Mock service for IPersonService to simulate the behavior of the person service in tests.
    /// </summary>
    private readonly Mock<IPersonService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPeopleHandler to ensure it returns Ok when people exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenPeopleExist()
    {
        // Arrange
        var people = new List<Person>
        {
            new Person(
                 Email.Create("andres@email.com"),
                "Andres",
                "Gonzales",
                Phone.Create("8888-8888"),
                BirthDate.Create(new DateOnly(2000, 1, 1)),
                IdentityNumber.Create("2-3636-6958"),
                1
            ),
            new Person(
                Email.Create("luis@email.com"),
                "Luis",
                "Chang",
                Phone.Create("8999-9999"),
                BirthDate.Create(new DateOnly(1975, 5, 20)),
                IdentityNumber.Create("9-9999-8555"),
                2
            )
        };

        _mockService
            .Setup(s => s.GetAllPeopleAsync())
            .ReturnsAsync(people);

        // Act
        var result = await GetAllPeopleHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetAllPeopleResponse>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPeopleHandler to ensure it returns NotFound when no people exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNoPeopleExist()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllPeopleAsync())
            .ReturnsAsync(new List<Person>());

        // Act
        var result = await GetAllPeopleHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().Contain("no registered people");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPeopleHandler to ensure it returns BadRequest when a DomainException occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenDomainExceptionOccurs()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllPeopleAsync())
            .ThrowsAsync(new DomainException("Domain failure"));

        // Act
        var result = await GetAllPeopleHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<string>>();
        ((BadRequest<string>)result.Result!).Value.Should().Contain("Domain failure");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPeopleHandler to ensure it returns BadRequest when an unexpected exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllPeopleAsync())
            .ThrowsAsync(new System.Exception("Something broke"));

        // Act
        var result = await GetAllPeopleHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<string>>();
        ((BadRequest<string>)result.Result!).Value.Should().Contain("unexpected error");
    }
}
