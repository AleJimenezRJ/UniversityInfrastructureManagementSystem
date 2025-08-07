using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Unit tests for <see cref="GetByUserAndRoleHandler"/>.
/// </summary>
public class GetByUserAndRoleHandlerTests
{
    private readonly Mock<IUserRoleService> _userRoleServiceMock;

    public GetByUserAndRoleHandlerTests()
    {
        _userRoleServiceMock = new Mock<IUserRoleService>();
    }

    /// <summary>
    /// Tests that the handler returns Ok with a message when the user-role association exists.
    /// </summary>
    /// <returns>
    /// Returns an <see cref="Ok{T}"/> result with a success message if the user-role association is found,
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUserRoleExists()
    {
        // Arrange
        var userRole = new UserRole(1, 2);
        _userRoleServiceMock.Setup(s => s.GetByUserAndRoleAsync(1, 2))
                   .ReturnsAsync(userRole);

        // Act
        var result = await GetByUserAndRoleHandler.HandleAsync(1, 2, _userRoleServiceMock.Object);

        // Assert
        var okResult = result.Result as Ok<GetByUserAndRoleResponse>;
        okResult.Should().NotBeNull();
        okResult!.Value?.Message.Should().Contain("User-role association for User ID 1 and Role ID 2 found.");
    }

    /// <summary>
    /// Tests that the handler returns NotFound when the user-role association does not exist.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="NotFound{T}"/> result with an error message if the user-role association is not found.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserRoleIsNull()
    {
        // Arrange
        _userRoleServiceMock
            .Setup(s => s.GetByUserAndRoleAsync(1, 2))
            .ReturnsAsync((UserRole?)null);

        // Act
        var result = await GetByUserAndRoleHandler.HandleAsync(1, 2, _userRoleServiceMock.Object);

        // Assert
        var notFoundResult = result.Result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("User-role association not found.");
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when userId is invalid (less than or equal to zero).
    /// </summary>
    /// <returns>
    /// Returns a <see cref="BadRequest{T}"/> with a list of validation errors if the userId is invalid.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var errorMessages = new List<ValidationError>
        {
            new ValidationError("UserId", "UserId must be a positive integer")
        };
        // Act
        var result = await GetByUserAndRoleHandler.HandleAsync(-1, 2, _userRoleServiceMock.Object);
        
        // Assert
        var badRequestResult = result.Result as BadRequest<List<ValidationError>>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().BeEquivalentTo(errorMessages);
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when roleId is invalid (less than or equal to zero).
    /// </summary>
    /// <returns>
    /// Returns a <see cref="BadRequest{T}"/> with a list of validation errors if the roleId is invalid.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenRoleIdIsInvalid()
    {
        // Arrange
        var errorMessages = new List<ValidationError>
        {
            new ValidationError("RoleId", "RoleId must be a positive integer")
        };
        // Act
        var result = await GetByUserAndRoleHandler.HandleAsync(1, -2, _userRoleServiceMock.Object);
        
        // Assert
        var badRequestResult = result.Result as BadRequest<List<ValidationError>>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().BeEquivalentTo(errorMessages);
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when both userId and roleId are invalid (less than or equal to zero).
    /// </summary>
    /// <returns>
    /// Returns a <see cref="BadRequest{T}"/> with a list of validation errors if both userId and roleId are invalid.
    /// </returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenRoleIdAndUserIdIsInvalid()
    {
        // Arrange
        var errorMessages = new List<ValidationError>
        {
            new ValidationError("UserId", "UserId must be a positive integer"),
            new ValidationError("RoleId", "RoleId must be a positive integer")
        };
        // Act
        var result = await GetByUserAndRoleHandler.HandleAsync(0, 0, _userRoleServiceMock.Object);

        // Assert
        var badRequestResult = result.Result as BadRequest<List<ValidationError>>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().BeEquivalentTo(errorMessages);
    }
}