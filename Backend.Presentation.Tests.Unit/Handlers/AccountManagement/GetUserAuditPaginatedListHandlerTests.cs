using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Unit tests for <see cref="GetUserAuditPaginatedListHandler"/>.
/// Verifies the behavior of user audit record pagination.
/// </summary>
public class GetUserAuditPaginatedListHandlerTests
{
    /// <summary>
    /// Mock for <see cref="IUserAuditService"/> used in tests.
    /// </summary>
    private readonly Mock<IUserAuditService> _auditServiceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserAuditPaginatedListHandlerTests"/> class.
    /// </summary>
    public GetUserAuditPaginatedListHandlerTests()
    {
        _auditServiceMock = new Mock<IUserAuditService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetUserAuditPaginatedListHandler.HandleAsync"/> returns a BadRequest
    /// when provided with invalid page size and page number.
    /// </summary>
    [Fact]
    public async Task HandleAsync_InvalidPageSizeAndNumber_ReturnsBadRequest()
    {
        // Arrange
        int pageSize = 0;
        int pageNumber = -1;

        // Act
        var result = await GetUserAuditPaginatedListHandler.HandleAsync(_auditServiceMock.Object, pageSize, pageNumber);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value!;
        errors.Should().HaveCount(2);
        errors.Should().Contain(e => e.Parameter == "PageSize");
        errors.Should().Contain(e => e.Parameter == "PageIndex");
    }

    /// <summary>
    /// Tests that <see cref="GetUserAuditPaginatedListHandler.HandleAsync"/> returns a BadRequest
    /// when the page size exceeds the allowed maximum.
    /// </summary>
    [Fact]
    public async Task HandleAsync_PageSizeTooLarge_ReturnsBadRequest()
    {
        // Arrange
        int pageSize = 200;
        int pageNumber = 1;

        // Act
        var result = await GetUserAuditPaginatedListHandler.HandleAsync(_auditServiceMock.Object, pageSize, pageNumber);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value!;
        errors.Should().ContainSingle(e => e.Parameter == "PageSize");
    }

    /// <summary>
    /// Tests that <see cref="GetUserAuditPaginatedListHandler.HandleAsync"/> returns an Ok result
    /// with a valid <see cref="GetUserAuditPaginatedListResponse"/> when provided with valid page size and page number.
    /// Verifies that the response contains the correct audit records and pagination metadata.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ValidInputs_ReturnsOkWithResponse()
    {
        // Arrange
        int pageSize = 2;
        int pageNumber = 1;

        var auditList = new PaginatedList<UserAudit>(
            new List<UserAudit>
            {

                new UserAudit
                {
                    UserName = "admin",
                    Action = "Logged in",
                    ModifiedAt = DateTime.UtcNow
                },

                new UserAudit
                {
                    UserName = "admin",
                    Action = "Modified user",
                    ModifiedAt = DateTime.UtcNow
                }

            },
            totalCount: 10,
            pageIndex: pageNumber,
            pageSize: pageSize
        );

        _auditServiceMock
            .Setup(s => s.GetPaginatedUserAuditAsync(pageSize, pageNumber))
            .ReturnsAsync(auditList);

        // Act
        var result = await GetUserAuditPaginatedListHandler.HandleAsync(_auditServiceMock.Object, pageSize, pageNumber);

        // Assert
        result.Result.Should().BeOfType<Ok<GetUserAuditPaginatedListResponse>>();
        var response = (result.Result as Ok<GetUserAuditPaginatedListResponse>)?.Value!;
        response.Should().NotBeNull();
        response.Audits.Should().HaveCount(2);
        response.PageNumber.Should().Be(pageNumber);
        response.PageSize.Should().Be(pageSize);
        response.TotalCount.Should().Be(10);
        response.TotalPages.Should().Be((int)Math.Ceiling(10.0 / pageSize));

    }
}
