using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetPermissionsByRoleIdHandler"/> class.
/// Verifies the behavior of the handler when retrieving permissions by role ID,
/// including validation and service interaction scenarios.
/// </summary>
public class GetPermissionsByRoleIdHandlerTests
{
    private readonly Mock<IRolePermissionService> _serviceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPermissionsByRoleIdHandlerTests"/> class.
    /// Sets up the mock for <see cref="IRolePermissionService"/> with strict behavior.
    /// </summary>
    public GetPermissionsByRoleIdHandlerTests()
    {
        _serviceMock = new Mock<IRolePermissionService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that the handler returns a <see cref="BadRequest{T}"/> result
    /// containing a validation error when the provided role ID is invalid (zero).
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenRoleIdIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPermissionsByRoleIdRequest { RoleId = 0 };

        // Act
        var result = await GetPermissionsByRoleIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value!;
        errors.Should().ContainSingle();
        errors[0].Parameter.Should().Be("RoleId");
    }


    /// <summary>
    /// Tests that the handler returns a <see cref="NotFound{T}"/> result
    /// containing an appropriate message when no permissions are found for the given role ID.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenPermissionsAreEmpty_ReturnsNotFound()
    {
        // Arrange
        var request = new GetPermissionsByRoleIdRequest { RoleId = 1 };

        _serviceMock.Setup(s => s.ViewPermissionsByRoleIdAsync(request.RoleId))
                    .ReturnsAsync(new List<Permission>());

        // Act
        var result = await GetPermissionsByRoleIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var message = (result.Result as NotFound<string>)?.Value;
        message.Should().Contain("No permissions found");
    }

    /// <summary>
    /// Tests that the handler returns an <see cref="Ok{T}"/> result containing a <see cref="GetPermissionsByRoleIdResponse"/>
    /// when permissions exist for the specified role ID. Verifies that the response contains the correct number of permissions
    /// and that the permission descriptions match the expected values.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenPermissionsExist_ReturnsOkWithResponse()
    {
        // Arrange
        var request = new GetPermissionsByRoleIdRequest { RoleId = 2 };
        var permissions = new List<Permission>
      {
          new Permission { Id = 1, Description = "Read" },
          new Permission { Id = 2, Description = "Write" }
      };

        _serviceMock.Setup(s => s.ViewPermissionsByRoleIdAsync(request.RoleId))
                    .ReturnsAsync(permissions);

        // Act
        var result = await GetPermissionsByRoleIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetPermissionsByRoleIdResponse>>();
        var response = (result.Result as Ok<GetPermissionsByRoleIdResponse>)?.Value;
        response.Should().NotBeNull();
        response!.Permissions.Should().HaveCount(2);
        response.Permissions[0].Description.Should().Be("Read");
    }

    /// <summary>
    /// Tests that the handler returns a <see cref="NotFound{T}"/> result
    /// when the service returns an empty list of permissions for the given role ID.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ServiceReturnsEmptyList_ReturnsNotFound()
    {
        // Arrange
        var request = new GetPermissionsByRoleIdRequest { RoleId = 1 };

        _serviceMock.Setup(s => s.ViewPermissionsByRoleIdAsync(1))
                    .ReturnsAsync(new List<Permission>());

        // Act
        var result = await GetPermissionsByRoleIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
    }

    /// <summary>
    /// Tests that the handler returns an <see cref="Ok{T}"/> result containing a <see cref="GetPermissionsByRoleIdResponse"/>
    /// when a valid request is provided and permissions exist for the specified role ID.
    /// Verifies that the response contains the correct number of permissions.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ValidRequestWithPermissions_ReturnsOk()
    {
        // Arrange
        var request = new GetPermissionsByRoleIdRequest { RoleId = 1 };

        var permissions = new List<Permission>
        {
            new Permission { Id = 101, Description = "Read" },
            new Permission { Id = 102, Description = "Write" }
        };

        _serviceMock.Setup(s => s.ViewPermissionsByRoleIdAsync(1))
                    .ReturnsAsync(permissions);

        // Act
        var result = await GetPermissionsByRoleIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetPermissionsByRoleIdResponse>>();
        var ok = result.Result as Ok<GetPermissionsByRoleIdResponse>;
        ok!.Value!.Permissions.Should().HaveCount(2);
    }
}
