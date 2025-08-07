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
/// Unit tests for the <see cref="GetUserWithPersonPaginatedListHandler"/> class.
/// These tests verify the handler's behavior for paginated user retrieval, including
/// successful responses and validation error scenarios.
/// </summary>
public class GetUserWithPersonPaginatedListHandlerTests
{
    private readonly Mock<IUserWithPersonService> _serviceMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserWithPersonPaginatedListHandlerTests"/> class.
    /// Sets up a strict mock for <see cref="IUserWithPersonService"/>.
    /// </summary>
    public GetUserWithPersonPaginatedListHandlerTests()
    {
        _serviceMock = new Mock<IUserWithPersonService>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that a valid request returns an Ok result with the expected paginated response.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsOkWithResponse()
    {
        // Arrange
        int pageSize = 10;
        int pageNumber = 0;

        var paginatedResult = new PaginatedList<UserWithPerson>(
            new List<UserWithPerson>
            {
                new UserWithPerson(
                    userName: UserName.Create("jdoe"),
                    firstName: "John",
                    lastName: "Doe",
                    email: Email.Create("john.doe@example.com"),
                    phone: Phone.Create("8888-8888"),
                    identityNumber: IdentityNumber.Create("1-2345-6789"),
                    birthDate: BirthDate.Create(new DateOnly(1990, 5, 15)),
                    roles: new List<string> { "Admin", "Manager" },
                    userId: 1,
                    personId: 2
                ),
                new UserWithPerson(
                    userName: UserName.Create("asmith"),
                    firstName: "Alice",
                    lastName: "Smith",
                    email: Email.Create("alice.smith@example.com"),
                    phone: Phone.Create("8777-7777"),
                    identityNumber: IdentityNumber.Create("2-3456-7890"),
                    birthDate: BirthDate.Create(new DateOnly(1985, 11, 3)),
                    roles: new List<string> { "Editor" },
                    userId: 3,
                    personId: 4
                )
            },
            totalCount: 2,
            pageSize: 10,
            pageIndex: 0
        );

        string searchText = "";
        _serviceMock.Setup(s => s.GetPaginatedUsersAsync(pageSize, pageNumber, searchText))
                    .ReturnsAsync(paginatedResult);

        // Act
        var result = await GetUserWithPersonPaginatedListHandler.HandleAsync(_serviceMock.Object, pageSize, pageNumber, searchText);

        // Assert
        result.Result.Should().BeOfType<Ok<GetUserWithPersonPaginatedListResponse>>();
        var ok = result.Result as Ok<GetUserWithPersonPaginatedListResponse>;
        ok!.Value!.Users.Should().HaveCount(2);
        ok.Value.TotalCount.Should().Be(2);
        ok.Value.PageSize.Should().Be(10);
    }

    /// <summary>
    /// Tests that an invalid page size returns a BadRequest with a validation error for PageSize.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithInvalidPageSize_ReturnsBadRequest()
    {
        // Arrange
        int invalidPageSize = 0;
        int pageNumber = 0;

        // Act
        var result = await GetUserWithPersonPaginatedListHandler.HandleAsync(_serviceMock.Object, invalidPageSize, pageNumber);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().ContainSingle()
            .Which.Parameter.Should().Be("PageSize");
    }

    /// <summary>
    /// Tests that an invalid page index returns a BadRequest with a validation error for PageIndex.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithInvalidPageIndex_ReturnsBadRequest()
    {
        // Arrange
        int pageSize = 10;
        int invalidPageNumber = -1;

        // Act
        var result = await GetUserWithPersonPaginatedListHandler.HandleAsync(_serviceMock.Object, pageSize, invalidPageNumber);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().ContainSingle()
            .Which.Parameter.Should().Be("PageIndex");
    }

    /// <summary>
    /// Tests that multiple validation errors (invalid page size and index) return a BadRequest with both errors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithMultipleValidationErrors_ReturnsBadRequest()
    {
        // Arrange
        int invalidPageSize = 200;
        int invalidPageNumber = -5;

        // Act
        var result = await GetUserWithPersonPaginatedListHandler.HandleAsync(_serviceMock.Object, invalidPageSize, invalidPageNumber);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var errors = (result.Result as BadRequest<List<ValidationError>>)?.Value;
        errors.Should().HaveCount(2);
        errors!.Select(e => e.Parameter).Should().Contain(new[] { "PageSize", "PageIndex" });
    }
}
