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
/// Tests for the GetAllStudentHandler.
/// </summary>
public class GetAllStudentHandlerTests
{
    /// <summary>
    /// Mock service for student operations.
    /// </summary>
    private readonly Mock<IStudentService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStudentHandler to ensure it returns Ok when students exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenStudentsExist()
    {
        var students = new List<Student>
        {
            new Student("B12345", Email.Create("ana@ucr.ac.cr"), 1)
            {
                Person = new Person(
                    Email.Create("ana@ucr.ac.cr"),
                    "Ana",
                    "Gómez",
                    Phone.Create("2222-3333"),
                    BirthDate.Create(new DateOnly(2000, 5, 20)),
                    IdentityNumber.Create("1-2345-6789")
                )
            },
            new Student("B67890", Email.Create("pedro@ucr.ac.cr"), 2)
            {
                Person = new Person(
                    Email.Create("pedro@ucr.ac.cr"),
                    "Pedro",
                    "Martínez",
                    Phone.Create("8888-9999"),
                    BirthDate.Create(new DateOnly(1999, 8, 15)),
                    IdentityNumber.Create("9-8765-4321")
                )
            }
        };

        _mockService.Setup(s => s.ListStudentsAsync()).ReturnsAsync(students);

        var result = await GetAllStudentHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Ok<GetAllStudentResponse>>();
        var value = ((Ok<GetAllStudentResponse>)result.Result!).Value;
        value.Students.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStudentHandler to ensure it returns NotFound when no students exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNoStudentsExist()
    {
        _mockService.Setup(s => s.ListStudentsAsync()).ReturnsAsync(new List<Student>());

        var result = await GetAllStudentHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("no registered students");
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenStudentListIsNull()
    {
        _mockService.Setup(s => s.ListStudentsAsync()).ReturnsAsync((List<Student>?)null);

        var result = await GetAllStudentHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("no registered students");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStudentHandler to ensure it returns Conflict when a domain exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionIsThrown()
    {
        _mockService.Setup(s => s.ListStudentsAsync()).ThrowsAsync(new DomainException("Error listing students"));

        var result = await GetAllStudentHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("Error listing students");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllStudentHandler to ensure it maps students correctly to DTOs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldMapStudentCorrectly_ToDto()
    {
        var students = new List<Student>
        {
            new Student("B54321", Email.Create("carlos@ucr.ac.cr"), 3)
            {
                Person = new Person(
                    Email.Create("carlos@ucr.ac.cr"),
                    "Carlos",
                    "Ramírez",
                    Phone.Create("2222-4444"),
                    BirthDate.Create(new DateOnly(1999, 10, 10)),
                    IdentityNumber.Create("1-2345-6789")
                )
            }
        };

        _mockService.Setup(s => s.ListStudentsAsync()).ReturnsAsync(students);

        var result = await GetAllStudentHandler.HandleAsync(_mockService.Object);

        var value = ((Ok<GetAllStudentResponse>)result.Result!).Value;
        value.Students.Should().ContainSingle();
    }
}
