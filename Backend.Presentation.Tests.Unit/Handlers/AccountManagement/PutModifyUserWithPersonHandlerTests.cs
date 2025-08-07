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
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// A test class for the PutModifyUserWithPersonHandler.
/// </summary>
public class PutModifyUserWithPersonHandlerTests
{
    /// <summary>
    /// Mock service for IUserWithPersonService to simulate user-person operations in tests.
    /// </summary>
    private readonly Mock<IUserWithPersonService> _mockService = new();

    /// <summary>
    /// Creates a valid request for modifying a user with person details.
    /// </summary>
    /// <param name="identityNumber"> The identity number of the user to be modified.</param>
    /// <returns> A PutModifyUserWithPersonRequest object containing the identity number and user details.</returns>
    private static PutModifyUserWithPersonRequest CreateValidRequest(string identityNumber)
    {
        return new PutModifyUserWithPersonRequest
        {
            IdentityNumber = identityNumber,
            UserWithPerson = 
            new CreateUserWithPersonDto(
                UserName: "newuser",
                Email: "new@email.com",
                FirstName: "Test",
                LastName: "User",
                Phone: "8888-8888",
                BirthDate: new DateOnly(1995, 1, 1),
                IdentityNumber: identityNumber,
                Roles: new List<string> { "admin" }
             )
        };
    }


    /// <summary>
    /// Tests that the handler returns BadRequest when the identity number is invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdentityNumberIsInvalid()
    {
        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            new PutModifyUserWithPersonRequest
            {
                IdentityNumber = "invalid-id",
                UserWithPerson = CreateValidRequest("invalid-id").UserWithPerson
            });

        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when the UserWithPerson is null.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUserWithPersonIsNull()
    {
        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            new PutModifyUserWithPersonRequest
            {
                IdentityNumber = "1-1111-1111",
                UserWithPerson = null!
            });

        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns NotFound when the user with the specified identity number does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var identity = "1-1111-1111";
        _mockService.Setup(s => s.GetAllUserWithPersonAsync()).ReturnsAsync(new List<UserWithPerson>());

        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            CreateValidRequest(identity));

        result.Result.Should().BeOfType<NotFound<string>>();
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when the update operation fails.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUpdateFails()
    {
        var identity = "1-1111-1111";
        var existingUser = new UserWithPerson(
            userName: UserName.Create("olduser"),
            firstName: "Old",
            lastName: "User",
            email: Email.Create("old@email.com"),
            phone: Phone.Create("8888-8888"),
            identityNumber: IdentityNumber.Create(identity),
            birthDate: BirthDate.Create(new DateOnly(1995, 1, 1)),
            userId: 1,
            personId: 1
        );

        _mockService.Setup(s => s.GetAllUserWithPersonAsync()).ReturnsAsync([existingUser]);
        _mockService.Setup(s => s.UpdateUserWithPersonAsync(It.IsAny<UserWithPerson>())).ReturnsAsync(false);

        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            CreateValidRequest(identity));

        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns Ok when the user is successfully updated.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUpdateSucceeds()
    {
        var identity = "1-1111-1111";
        var existingUser = new UserWithPerson(
             userName: UserName.Create("olduser"),
             firstName: "Old",
             lastName: "User",
             email: Email.Create("old@email.com"),
             phone: Phone.Create("8888-8888"),
             identityNumber: IdentityNumber.Create(identity),
             birthDate: BirthDate.Create(new DateOnly(1995, 1, 1)),
             userId: 1,
             personId: 1
         );

        _mockService.Setup(s => s.GetAllUserWithPersonAsync()).ReturnsAsync([existingUser]);
        _mockService.Setup(s => s.UpdateUserWithPersonAsync(It.IsAny<UserWithPerson>())).ReturnsAsync(true);

        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            CreateValidRequest(identity));

        result.Result.Should().BeOfType<Ok<PutModifyUserWithPersonResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns BadRequest when a DuplicatedEntityException is thrown during the update operation.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenDuplicatedEntityExceptionThrown()
    {
        var identity = "1-1111-1111";
        var existingUser = new UserWithPerson(
            userName: UserName.Create("olduser"),
            firstName: "Old",
            lastName: "User",
            email: Email.Create("old@email.com"),
            phone: Phone.Create("8888-8888"),
            identityNumber: IdentityNumber.Create(identity),
            birthDate: BirthDate.Create(new DateOnly(1995, 1, 1)),
            userId: 1,
            personId: 1
        );

        _mockService.Setup(s => s.GetAllUserWithPersonAsync()).ReturnsAsync([existingUser]);
        _mockService.Setup(s => s.UpdateUserWithPersonAsync(It.IsAny<UserWithPerson>()))
            .ThrowsAsync(new DuplicatedEntityException("Duplicate"));

        var result = await PutModifyUserWithPersonHandler.HandleAsync(
            _mockService.Object,
            CreateValidRequest(identity));

        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }
}
