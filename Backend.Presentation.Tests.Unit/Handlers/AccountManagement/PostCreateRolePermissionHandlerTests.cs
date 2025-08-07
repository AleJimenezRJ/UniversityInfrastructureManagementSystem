using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="PostCreateRolePermissionHandler"/> class.
/// Verifies the behavior of assigning permissions to roles using the <see cref="IRolePermissionService"/>.
/// </summary>
public class PostCreateRolePermissionHandlerTests
{
    private readonly Mock<IRolePermissionService> _serviceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostCreateRolePermissionHandlerTests"/> class.
    /// Sets up a strict mock for <see cref="IRolePermissionService"/>.
    /// </summary>
    public PostCreateRolePermissionHandlerTests()
    {
        _serviceMock = new Mock<IRolePermissionService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns an <see cref="Ok{T}"/> result
    /// with the correct response when a valid request is provided and the service successfully assigns the permission.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ValidRequest_ReturnsOk()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: 1, PermId: 10);
        var request = new PostCreateRolePermissionRequest(dto);

        _serviceMock.Setup(s => s.AssignPermissionToRoleAsync(1, 10))
                    .ReturnsAsync(true);

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<PostCreateRolePermissionResponse>>();
        var ok = result.Result as Ok<PostCreateRolePermissionResponse>;
        ok!.Value!.RolePermission.RoleId.Should().Be(1);
        ok.Value.RolePermission.PermId.Should().Be(10);
        ok.Value.Message.Should().Be("Permission successfully assigned to role.");
    }

    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
    /// when the permission is already assigned to the role (i.e., the service returns false).
    /// </summary>
    [Fact]
    public async Task HandleAsync_PermissionAlreadyAssigned_ReturnsConflict()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: 1, PermId: 10);
        var request = new PostCreateRolePermissionRequest(dto);

        _serviceMock.Setup(s => s.AssignPermissionToRoleAsync(1, 10))
                    .ReturnsAsync(false);

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
    }

    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns a <see cref="BadRequest{T}"/> result
    /// containing a validation error for "RoleId" when a negative role ID is provided in the request.
    /// </summary>
    [Fact]
    public async Task HandleAsync_NegativeRoleId_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: -5, PermId: 10);
        var request = new PostCreateRolePermissionRequest(dto);

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>().Subject;
        badRequest.Value.Should().ContainSingle(v => v.Parameter == "RoleId");
    }


    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns a <see cref="BadRequest{T}"/> result
    /// containing a validation error for "PermId" when a negative permission ID is provided in the request.
    /// </summary>
    [Fact]
    public async Task HandleAsync_NegativePermId_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: 1, PermId: -10);
        var request = new PostCreateRolePermissionRequest(dto);

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>().Subject;
        badRequest.Value.Should().ContainSingle(v => v.Parameter == "PermId");
    }

    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns a <see cref="BadRequest{T}"/> result
    /// containing multiple validation errors when both RoleId and PermId are invalid (zero or negative).
    /// </summary>
    [Fact]
    public async Task HandleAsync_BothIdsInvalid_ReturnsMultipleValidationErrors()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: 0, PermId: -1);
        var request = new PostCreateRolePermissionRequest(dto);

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>().Subject;
        badRequest.Value.Should().HaveCount(2);
        badRequest.Value.Should().Contain(v => v.Parameter == "RoleId");
        badRequest.Value.Should().Contain(v => v.Parameter == "PermId");
    }

    /// <summary>
    /// Tests that <see cref="PostCreateRolePermissionHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
    /// containing the exception message when the <see cref="IRolePermissionService"/> throws a <see cref="DomainException"/>.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ThrowsDomainException_ReturnsConflictWithMessage()
    {
        // Arrange
        var dto = new RolePermissionDto(RoleId: 1, PermId: 10);
        var request = new PostCreateRolePermissionRequest(dto);

        _serviceMock.Setup(s => s.AssignPermissionToRoleAsync(1, 10))
                    .ThrowsAsync(new DomainException("Permission already assigned."));

        // Act
        var result = await PostCreateRolePermissionHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        var conflict = result.Result.Should().BeOfType<Conflict<string>>().Subject;
        conflict.Value.Should().Be("Permission already assigned.");
    }

}
