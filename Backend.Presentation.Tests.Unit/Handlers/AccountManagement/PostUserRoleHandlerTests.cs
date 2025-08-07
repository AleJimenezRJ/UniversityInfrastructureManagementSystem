using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Handlers;

/// <summary>
/// Unit tests for the <see cref="PostUserRoleHandler"/> class.
/// </summary>
public class PostUserRoleHandlerTests
{
    /// <summary>
    /// Creates a valid <see cref="UserRoleDto"/> instance for testing purposes.
    /// </summary>
    /// <returns>A valid <see cref="UserRoleDto"/> instance.</returns>
    private static UserRoleDto CreateValidDto() => new(
        UserId: 1,
        RoleId: 2
    );

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> returns an <see cref="Ok{T}"/> result
    /// when provided with a valid request.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();
        serviceMock.Setup(s => s.AssignRoleAsync(dto.UserId, dto.RoleId)).ReturnsAsync(true);

        var result = await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);

        result.Result.Should().BeOfType<Ok<PostUserRoleResponse>>();
    }

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> returns a <see cref="BadRequest{T}"/> result
    /// when provided with invalid user or role IDs.
    /// </summary>
    /// <param name="userId">The user ID to test.</param>
    /// <param name="roleId">The role ID to test.</param>
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    [InlineData(0, 0)]
    public async Task GivenInvalidUserIdOrRoleId_WhenHandleAsync_ReturnsBadRequest(int userId, int roleId)
    {
        var dto = new UserRoleDto(userId, roleId);
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();
        var result = await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
    /// when attempting to assign a duplicate role to a user.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateRoleAssignment_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();
        serviceMock.Setup(s => s.AssignRoleAsync(dto.UserId, dto.RoleId)).ReturnsAsync(false);

        var result = await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);

        result.Result.Should().BeOfType<Conflict<string>>()
              .Which.Value.Should().Be("User already has this role assigned.");
    }

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
    /// when a <see cref="DomainException"/> is thrown by the service.
    /// </summary>
    [Fact]
    public async Task GivenDomainException_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();
        serviceMock.Setup(s => s.AssignRoleAsync(dto.UserId, dto.RoleId))
                   .ThrowsAsync(new DomainException("Domain error"));

        var result = await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);

        result.Result.Should().BeOfType<Conflict<string>>()
              .Which.Value.Should().Be("Domain error");
    }

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> calls the service method exactly once
    /// when provided with a valid request.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_CallsServiceOnce()
    {
        var dto = CreateValidDto();
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();
        serviceMock.Setup(s => s.AssignRoleAsync(dto.UserId, dto.RoleId)).ReturnsAsync(true);

        await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);

        serviceMock.Verify(s => s.AssignRoleAsync(dto.UserId, dto.RoleId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="PostUserRoleHandler.HandleAsync"/> does not call the service method
    /// when provided with an invalid request.
    /// </summary>
    [Fact]
    public async Task GivenInvalidRequest_WhenHandleAsync_DoesNotCallService()
    {
        var dto = new UserRoleDto(0, 0);
        var request = new PostUserRoleRequest(dto);
        var serviceMock = new Mock<IUserRoleService>();

        await PostUserRoleHandler.HandleAsync(request, serviceMock.Object);

        serviceMock.Verify(s => s.AssignRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}
