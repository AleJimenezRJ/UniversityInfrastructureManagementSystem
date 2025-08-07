using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetUserIdByEmailHandler"/> class.
/// Verifies the behavior of the handler when retrieving a user ID by email address.
/// </summary>
public class GetUserIdByEmailHandlerTests
{
    /// <summary>
    /// Mock instance of <see cref="IUserWithPersonService"/> used for testing.
    /// </summary>
    private readonly Mock<IUserWithPersonService> _serviceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserIdByEmailHandlerTests"/> class.
    /// Sets up the mock service with strict behavior.
    /// </summary>
    public GetUserIdByEmailHandlerTests()
    {
        _serviceMock = new Mock<IUserWithPersonService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetUserIdByEmailHandler.HandleAsync"/> returns an <see cref="Ok{T}"/> result
    /// with the user ID when the user exists for the given email.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUserExists_ReturnsOkWithUserId()
    {
        // Arrange
        var email = "gael@ucr.ac.cr";
        var expectedUserId = 42;

        _serviceMock.Setup(s => s.GetUserIdByEmailAsync(email))
                    .ReturnsAsync(expectedUserId);

        // Act
        var result = await GetUserIdByEmailHandler.HandleAsync(email, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<int>>();
        (result.Result as Ok<int>)!.Value.Should().Be(expectedUserId);
    }

    /// <summary>
    /// Tests that <see cref="GetUserIdByEmailHandler.HandleAsync"/> returns a <see cref="NotFound{T}"/> result
    /// with an error response when the user does not exist for the given email.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUserDoesNotExist_ReturnsNotFoundWithError()
    {
        // Arrange
        var email = "notfound@ucr.ac.cr";

        _serviceMock.Setup(s => s.GetUserIdByEmailAsync(email))
                    .ReturnsAsync(0);

        // Act
        var result = await GetUserIdByEmailHandler.HandleAsync(email, _serviceMock.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<ErrorResponse>>();
        var notFound = result.Result as NotFound<ErrorResponse>;

        notFound!.Value.Should().NotBeNull();
        notFound.Value.ErrorMessages.Should().ContainSingle()
            .Which.Should().Be($"No user found with email '{email}'.");
    }
}
