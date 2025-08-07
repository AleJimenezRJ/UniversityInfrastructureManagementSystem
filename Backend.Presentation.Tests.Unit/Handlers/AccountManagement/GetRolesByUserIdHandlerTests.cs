using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetRolesByUserIdHandler"/> class,
/// which is responsible for retrieving roles assigned to a user by ID.
/// </summary>
public class GetRolesByUserIdHandlerTests
{
    private readonly Mock<IUserRoleService> _serviceMock;

    public GetRolesByUserIdHandlerTests()
    {
        _serviceMock = new Mock<IUserRoleService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that the handler returns a <see cref="BadRequest{T}"/> result
    /// when an invalid user ID (zero or negative) is provided.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task HandleAsync_InvalidUserId_ReturnsBadRequest(int invalidUserId)
    {
        // Arrange
        var request = new GetRolesByUserIdRequest { UserId = invalidUserId };

        // Act
        var result = await GetRolesByUserIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value!;
        errors.Should().ContainSingle();
        errors[0].Parameter.Should().Be("UserId");
    }

    /// <summary>
    /// Tests that the handler returns <see cref="NotFound{T}"/> when no roles are found for a valid user ID.
    /// </summary>
    [Fact]
    public async Task HandleAsync_NoRolesFound_ReturnsNotFound()
    {
        // Arrange
        var request = new GetRolesByUserIdRequest { UserId = 100 };
        _serviceMock.Setup(s => s.GetRolesByUserIdAsync(request.UserId))
                    .ReturnsAsync(new List<Role>());

        // Act
        var result = await GetRolesByUserIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var message = (result.Result as NotFound<string>)?.Value;
        message.Should().Contain("No roles were found");
    }

    /// <summary>
    /// Tests that the handler returns <see cref="Ok{T}"/> with the expected response when roles are found.
    /// </summary>
    [Fact]
    public async Task HandleAsync_RolesFound_ReturnsOkWithRoles()
    {
        // Arrange
        var request = new GetRolesByUserIdRequest { UserId = 200 };

        var roles = new List<Role>
        {
            new Role("Admin") { Id = 1 },
            new Role("Editor") { Id = 2 }
        };

        _serviceMock.Setup(s => s.GetRolesByUserIdAsync(request.UserId))
                    .ReturnsAsync(roles);

        // Act
        var result = await GetRolesByUserIdHandler.HandleAsync(request, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetRolesByUserIdResponse>>();
        var response = (result.Result as Ok<GetRolesByUserIdResponse>)?.Value!;
        response.Should().NotBeNull();
        response.Roles.Should().HaveCount(2);
        response.Roles.Select(r => r.Name).Should().Contain(new[] { "Admin", "Editor" });
    }

}
