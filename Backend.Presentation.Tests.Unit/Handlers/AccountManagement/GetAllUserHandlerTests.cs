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
/// Tests for the GetAllUserHandler.
/// </summary>
public class GetAllUserHandlerTests
{
    /// <summary>
    /// Mock service for user operations.
    /// </summary>
    private readonly Mock<IUserService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersHandler to ensure it returns Ok when users exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUsersExist()
    {
        var users = new List<User>
        {
            new(UserName.Create("admin01"), 1)
            {
                Person = new Person(
                    Email.Create("admin@ucr.ac.cr"),
                    "Ana",
                    "Ramírez",
                    Phone.Create("8888-0000"),
                    BirthDate.Create(new DateOnly(1995, 7, 6)),
                    IdentityNumber.Create("1-2345-6789")),
                UserRoles = new List<UserRole>
                {
                     new(2, 2) { Role = new Role ("Admin")}
                }
            },
            new(UserName.Create("staff01"), 2)
            {
                Person = new Person(
                    Email.Create("staff@ucr.ac.cr"),
                    "Pedro",
                    "López",
                    Phone.Create("8999-1111"),
                    BirthDate.Create(new DateOnly(1990, 1, 1)),
                    IdentityNumber.Create("9-8765-4321")),
                UserRoles = new List<UserRole>
                {
                    new(2, 2) { Role = new Role ("Staff") }
                }
            }
        };

        _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        var result = await GetAllUsersHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Ok<GetAllUsersResponse>>();
        var value = ((Ok<GetAllUsersResponse>)result.Result!).Value;
        value.Users.Should().HaveCount(2);
        value.Users[0].UserName.Should().Be("admin01");
        value.Users[1].UserName.Should().Be("staff01");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersHandler to ensure it returns NotFound when no users exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserListIsEmpty()
    {
        _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(new List<User>());

        var result = await GetAllUsersHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<NotFound<string>>();
        var value = ((NotFound<string>)result.Result!).Value!;
        value.ToLower().Should().Contain("no registered users");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersHandler to ensure it returns BadRequest when the user list is null.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        _mockService.Setup(s => s.GetAllUsersAsync()).ThrowsAsync(new DomainException("Error getting users"));

        var result = await GetAllUsersHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Conflict<string>>();
        var value = ((Conflict<string>)result.Result!).Value!;
        value.Should().Contain("Error getting users");
    }
}
