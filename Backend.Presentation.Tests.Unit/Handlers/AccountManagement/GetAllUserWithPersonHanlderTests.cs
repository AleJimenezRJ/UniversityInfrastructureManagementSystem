using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the GetAllUsersWithPersonHandler.
/// </summary>
public class GetAllUserWithPersonHanlderTests
{
    /// <summary>
    /// Mocks for the services used in the handler.
    /// </summary>
    private readonly Mock<IUserService> _mockUserService = new();

    /// <summary>
    /// Mocks for the person service used in the handler.
    /// </summary>
    private readonly Mock<IPersonService> _mockPersonService = new();

    /// <summary>
    /// Mocks for the user role service used in the handler.
    /// </summary>
    private readonly Mock<IUserRoleService> _mockUserRoleService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersWithPersonHandler to ensure it returns Ok when users and people exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUsersAndPeopleExist()
    {
        var person = new Person(
            Email.Create("ana@ucr.ac.cr"),
            "Ana",
            "Ramírez",
            Phone.Create("8888-0000"),
            BirthDate.Create(new DateOnly(1995, 4, 23)),
            IdentityNumber.Create("1-2345-6789"),
            id: 1);

        var user = new User( UserName.Create("ana01"), person.Id, id: 2);

        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(new List<User> { user });
        _mockPersonService.Setup(s => s.GetAllPeopleAsync()).ReturnsAsync(new List<Person> { person });
        _mockUserRoleService.Setup(s => s.GetRolesByUserIdAsync(user.Id))
            .ReturnsAsync(new List<Role> { new Role ("Admin") });

        var result = await GetAllUsersWithPersonHandler.HandleAsync(
            _mockUserService.Object,
            _mockPersonService.Object,
            _mockUserRoleService.Object);

        result.Result.Should().BeOfType<Ok<List<UserWithPersonDto>>>();
        var dtoList = ((Ok<List<UserWithPersonDto>>)result.Result!).Value;
        dtoList.Should().ContainSingle();
        dtoList[0].UserName.Should().Be("ana01");
        dtoList[0].Roles.Should().Contain("Admin");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersWithPersonHandler to ensure it returns NotFound when no users exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserListIsEmpty()
    {
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(new List<User>());
        _mockPersonService.Setup(s => s.GetAllPeopleAsync()).ReturnsAsync(new List<Person>());

        var result = await GetAllUsersWithPersonHandler.HandleAsync(
            _mockUserService.Object,
            _mockPersonService.Object,
            _mockUserRoleService.Object);

        ((NotFound<string>)result.Result!).Value
            .ToLower().Should().Contain("no registered users");

    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUsersWithPersonHandler to ensure it returns BadRequest when the user list is null.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ThrowsAsync(new DomainException("Service failure"));

        var result = await GetAllUsersWithPersonHandler.HandleAsync(
            _mockUserService.Object,
            _mockPersonService.Object,
            _mockUserRoleService.Object);

        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("Service failure");
    }
}
