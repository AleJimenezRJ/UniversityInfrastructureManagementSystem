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
/// Unit tests for the <see cref="PutModifyUserHandler"/> class.
/// </summary>
public class PutModifyUserHandlerTests
{
    private readonly Mock<IUserService> _userServiceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="PutModifyUserHandlerTests"/> class.
    /// </summary>
    public PutModifyUserHandlerTests()
    {
        _userServiceMock = new Mock<IUserService>();
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when the user ID is invalid (0).
    /// </summary>
    /// <returns>
    /// Returns a BadRequest with a message if an invalid user ID is provided.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var request = new PutModifyUserRequest(new ModifyUserDto("validuser"));

        // Act
        // We are passing an invalid user ID (0) to the handler.
        var result = await PutModifyUserHandler.HandleAsync(0, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        var badRequest = result.Result as BadRequest<List<ValidationError>>;

        // This gives us the list of validation errors because the user ID is invalid (0).
        badRequest!.Value.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ValidationError("UserId", "Invalid user ID."));
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when the user name is invalid (empty or whitespace).
    /// </summary>
    /// <returns>
    /// Returns a BadRequest with a list of validation errors if the user name is invalid.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUserNameIsInvalid()
    {
        // Arrange
        var request = new PutModifyUserRequest(new ModifyUserDto(" "));

        // Act
        // We are passing a valid user ID (1) but an invalid username (empty) to the handler.
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        // This gives us the list of validation errors because the username is invalid (empty).
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ValidationError("UserName", "Username is required."));
    }


    /// <summary>
    /// Tests that the handler returns a BadRequest when the request user is null.
    /// </summary>
    /// <returns>
    /// Returns a BadRequest with a list of validation errors if the request user is null.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenRequestUserIsNull()
    {
        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, new PutModifyUserRequest(null!));

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        // This gives us the list of validation errors because the request user is null.
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ValidationError("UserName", "Username is required."));
    }

    /// <summary>
    /// Tests that the handler returns a NotFound when the user does not exist in the system.
    /// </summary>
    /// <returns>
    /// Returns a NotFound result with an error message if the user does not exist.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync([]);

        // Create a request with a valid user name but the user does not exist.
        var request = new PutModifyUserRequest(new ModifyUserDto("tester"));

        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
    }

    /// <summary>
    /// Tests that the handler returns an Ok response when the user is successfully modified.
    /// </summary>
    /// <returns>
    /// Returns an Ok response with the modified user information if the modification is successful.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUserIsSuccessfullyModified()
    {
        // Arrange
        // Mock a user with an old name and set up the service to return this user.
        var user = new User(UserName.Create("oldname"), 1) { Id = 1 };
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync([user]);
        _userServiceMock.Setup(s => s.ModifyUserAsync(1, It.IsAny<User>())).ReturnsAsync(true);

        // Create a request with a new user name.
        var request = new PutModifyUserRequest(new ModifyUserDto("newname"));

        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<Ok<PutModifyUserResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns a Conflict when a DuplicatedEntityException is thrown during user modification.
    /// </summary>
    /// <returns>
    /// Returns a Conflict result with an error message if a DuplicatedEntityException is thrown.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDuplicatedEntityExceptionThrown()
    {
        // Arrange
        // Mock a user with a specific name and set up the service to return this user.
        var user = new User(UserName.Create("tester"), 1) { Id = 1 };
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync([user]);
        _userServiceMock.Setup(s => s.ModifyUserAsync(1, It.IsAny<User>())).Throws(new DuplicatedEntityException("Duplicate"));

        // Create a request with a user name that would cause a conflict.
        var request = new PutModifyUserRequest(new ModifyUserDto("tester"));

        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
    }

    /// <summary>
    /// Tests that the handler returns a NotFound when a NotFoundException is thrown during user modification.
    /// </summary>
    /// <returns>
    /// Returns a NotFound result with an error message if a NotFoundException is thrown.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNotFoundExceptionThrown()
    {
        // Arrange
        // Mock a user with a specific name and set up the service to return this user.
        var user = new User(UserName.Create("tester"), 1) { Id = 1 };
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync([user]);
        _userServiceMock.Setup(s => s.ModifyUserAsync(1, It.IsAny<User>())).Throws(new NotFoundException("Not found"));

        // Create a request with a user name that would cause a not found exception.
        var request = new PutModifyUserRequest(new ModifyUserDto("tester"));

        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
    }

    /// <summary>
    /// Tests that the handler returns a Conflict when a DomainException is thrown during user modification.
    /// </summary>
    /// <returns>
    /// Returns a Conflict result with an error message if a DomainException is thrown.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionThrown()
    {
        // Arrange
        // Mock a user with a specific name and set up the service to return this user.
        var user = new User(UserName.Create("tester"), 1) { Id = 1 };
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync([user]);
        _userServiceMock.Setup(s => s.ModifyUserAsync(1, It.IsAny<User>())).Throws(new DomainException("Invalid domain state"));

        // Create a request with a user name that would cause a domain exception.
        var request = new PutModifyUserRequest(new ModifyUserDto("tester"));

        // Act
        var result = await PutModifyUserHandler.HandleAsync(1, _userServiceMock.Object, request);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
    }
}