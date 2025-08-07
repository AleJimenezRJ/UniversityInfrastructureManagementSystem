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
/// A test class for the GetAllStaffHandler.
/// </summary>
public class GetAllStaffHandlerTests
{
    /// <summary>
    /// Mock service for testing the GetAllStaffHandler.
    /// </summary>
    private readonly Mock<IStaffService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStaffHandler to ensure it returns Ok when staff exists.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenStaffExists()
    {
        // Arrange
        var staffList = new List<Staff>
        {
            new Staff(Email.Create("mario@ucr.ac.cr"), 1, "Administrative")
            {
                Person = new Person(
                    Email.Create("mario@ucr.ac.cr"),
                    "Mario",
                    "Córdoba",
                    Phone.Create("2222-3333"),
                    BirthDate.Create(new DateOnly(1980, 4, 12)),
                    IdentityNumber.Create("1-2345-6789"),
                    1
                )
            },
            new Staff(Email.Create("juan@ucr.ac.cr"), 2, "Administrative")
            {
                Person = new Person(
                    Email.Create("juan@ucr.ac.cr"),
                    "Juan",
                    "Pérez",
                    Phone.Create("8888-9999"),
                    BirthDate.Create(new DateOnly(1990, 6, 30)),
                    IdentityNumber.Create("9-8765-4321"),
                    2
                )
            }
        };

        _mockService.Setup(s => s.ListStaffAsync()).ReturnsAsync(staffList);

        // Act
        var result = await GetAllStaffHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetAllStaffResponse>>();
        var value = ((Ok<GetAllStaffResponse>)result.Result!).Value;
        value.Staff.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStaffHandler to ensure it returns NotFound when no staff exists.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNoStaffExists()
    {
        // Arrange
        _mockService.Setup(s => s.ListStaffAsync()).ReturnsAsync(new List<Staff>());

        // Act
        var result = await GetAllStaffHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.ToLower().Should().Contain("no registered staff");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStaffHandler to ensure it returns Conflict when a DomainException occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        _mockService
            .Setup(s => s.ListStaffAsync())
            .ThrowsAsync(new DomainException("Database error"));

        // Act
        var result = await GetAllStaffHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("Database error");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStaffHandler to ensure it maps staff email correctly to DTO.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldMapStaffEmailCorrectly_ToDto()
    {
        // Arrange
        var staffList = new List<Staff>
        {
            new Staff(Email.Create("gabriela@ucr.ac.cr"), 1, "Administrative")
            {
                Person = new Person(
                    Email.Create("gabriela@ucr.ac.cr"),
                    "Gabriela",
                    "Vargas",
                    Phone.Create("7000-1111"),
                    BirthDate.Create(new DateOnly(1992, 5, 10)),
                    IdentityNumber.Create("1-1122-2333"),
                    1
                )
            }
        };

        _mockService.Setup(s => s.ListStaffAsync()).ReturnsAsync(staffList);

        // Act
        var result = await GetAllStaffHandler.HandleAsync(_mockService.Object);

        // Assert
        var value = ((Ok<GetAllStaffResponse>)result.Result!).Value;
        value.Staff.Should().ContainSingle();

        var dto = value.Staff.First();
        dto.Email.Should().Be("gabriela@ucr.ac.cr");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStaffHandler to ensure it returns NotFound when the staff list is null.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenListIsNull()
    {
        _mockService.Setup(s => s.ListStaffAsync()).ReturnsAsync((List<Staff>?)null);

        var result = await GetAllStaffHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.ToLower().Should().Contain("no registered staff");
    }

}
