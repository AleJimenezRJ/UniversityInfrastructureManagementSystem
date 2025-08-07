using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Handlers;

/// <summary>
/// Unit tests for the PostCreateUserWithPersonHandler.
/// </summary>
public class PostCreateUserWithPersonHandlerTests
{
    /// <summary>
    /// Creates a valid DTO for testing purposes.
    /// </summary>
    /// <returns>A valid <see cref="CreateUserWithPersonDto"/> instance.</returns>
    private static CreateUserWithPersonDto CreateValidDto() => new(
        UserName: "usuario.valido_123",
        Email: "usuario@example.com",
        FirstName: "Nombre",
        LastName: "Apellido",
        Phone: "8888-1234",
        BirthDate: new DateOnly(2000, 1, 1),
        IdentityNumber: "1-1234-5678",
        Roles: new List<string> { "Admin" }
    );

    /// <summary>
    /// Tests that a valid request returns an Ok result.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserWithPersonRequest(dto);
        var serviceMock = new Mock<IUserWithPersonService>();
        serviceMock.Setup(s => s.CreateUserWithPersonAsync(It.IsAny<UserWithPerson>()))
                   .ReturnsAsync(true);

        var result = await PostCreateUserWithPersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Ok<PostCreateUserWithPersonResponse>>();
    }

    /// <summary>
    /// Tests that invalid user data returns a BadRequest result.
    /// </summary>
    [Fact]
    public async Task GivenInvalidUserData_WhenHandleAsync_ReturnsBadRequest()
    {
        var invalidDto = new CreateUserWithPersonDto(
            UserName: "USUARIO CON ESPACIOS",
            Email: "correo_malformateado",
            FirstName: "",
            LastName: "",
            Phone: "12345678",
            BirthDate: new DateOnly(2035, 1, 1),
            IdentityNumber: "12345678",
            Roles: []
        );

        var request = new PostCreateUserWithPersonRequest(invalidDto);
        var serviceMock = new Mock<IUserWithPersonService>();

        var result = await PostCreateUserWithPersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that duplicate user or person data returns a Conflict result.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateUserOrPerson_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserWithPersonRequest(dto);
        var serviceMock = new Mock<IUserWithPersonService>();
        serviceMock.Setup(s => s.CreateUserWithPersonAsync(It.IsAny<UserWithPerson>()))
                   .ReturnsAsync(false);

        var result = await PostCreateUserWithPersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
              .Which.Value.Should().Be("The username or person identity already exists.");
    }

    /// <summary>
    /// Tests that a domain exception returns a Conflict result.
    /// </summary>
    [Fact]
    public async Task GivenDomainException_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserWithPersonRequest(dto);
        var serviceMock = new Mock<IUserWithPersonService>();
        serviceMock.Setup(s => s.CreateUserWithPersonAsync(It.IsAny<UserWithPerson>()))
                   .ThrowsAsync(new DomainException("Domain error"));

        var result = await PostCreateUserWithPersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
              .Which.Value.Should().Be("Domain error");
    }

    /// <summary>
    /// Tests that the service is called exactly once for a valid request.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_CallsServiceOnce()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserWithPersonRequest(dto);
        var serviceMock = new Mock<IUserWithPersonService>();
        serviceMock.Setup(s => s.CreateUserWithPersonAsync(It.IsAny<UserWithPerson>()))
                   .ReturnsAsync(true);

        await PostCreateUserWithPersonHandler.HandleAsync(serviceMock.Object, request);

        serviceMock.Verify(s => s.CreateUserWithPersonAsync(It.IsAny<UserWithPerson>()), Times.Once);
    }
}
