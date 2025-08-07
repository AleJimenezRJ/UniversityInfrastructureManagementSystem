using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Handlers;

/// <summary>
/// Unit tests for the <see cref="PostCreatePersonHandler"/> class.
/// </summary>
public class PostCreatePersonHandlerTests
{
    /// <summary>
    /// Creates a valid <see cref="CreatePersonDto"/> instance for testing purposes.
    /// </summary>
    /// <returns>A valid <see cref="CreatePersonDto"/> object.</returns>
    private static CreatePersonDto CreateValidDto() => new(
        Email: "valid@email.com",
        FirstName: "Andres",
        LastName: "Murillo",
        Phone: "8888-9999",
        BirthDate: new DateOnly(2000, 1, 1),
        IdentityNumber: "1-1234-5678"
    );

    /// <summary>
    /// Tests that a valid request returns an <see cref="Ok{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostCreatePersonRequest(dto);

        var serviceMock = new Mock<IPersonService>();
        serviceMock.Setup(s => s.CreatePersonAsync(It.IsAny<Person>()))
                   .ReturnsAsync(true);

        var result = await PostCreatePersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Ok<PostCreatePersonResponse>>();
    }

    /// <summary>
    /// Tests that an invalid request returns a <see cref="BadRequest{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenInvalidRequest_WhenHandleAsync_ReturnsBadRequest()
    {
        var invalidDto = new CreatePersonDto(
            Email: "correo_malo",
            FirstName: "",
            LastName: "",
            Phone: "abc",
            BirthDate: new DateOnly(3024, 1, 1),
            IdentityNumber: ""
        );

        var request = new PostCreatePersonRequest(invalidDto);
        var serviceMock = new Mock<IPersonService>();

        var result = await PostCreatePersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that a duplicated person request returns a <see cref="Conflict{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenDuplicatedPerson_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreatePersonRequest(dto);

        var serviceMock = new Mock<IPersonService>();
        serviceMock.Setup(s => s.CreatePersonAsync(It.IsAny<Person>()))
                   .ThrowsAsync(new DuplicatedEntityException("Person already exists"));

        var result = await PostCreatePersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Person already exists");
        serviceMock.Verify(s => s.CreatePersonAsync(It.IsAny<Person>()), Times.Once);
    }

    /// <summary>
    /// Tests that a domain error returns a <see cref="Conflict{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenDomainError_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreatePersonRequest(dto);

        var serviceMock = new Mock<IPersonService>();
        serviceMock.Setup(s => s.CreatePersonAsync(It.IsAny<Person>()))
                   .ThrowsAsync(new DomainException("Invalid operation"));

        var result = await PostCreatePersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Invalid operation");
        serviceMock.Verify(s => s.CreatePersonAsync(It.IsAny<Person>()), Times.Once);
    }

    /// <summary>
    /// Tests that an unexpected error returns a <see cref="Conflict{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenUnexpectedError_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreatePersonRequest(dto);

        var serviceMock = new Mock<IPersonService>();
        serviceMock.Setup(s => s.CreatePersonAsync(It.IsAny<Person>()))
                   .ThrowsAsync(new Exception("Unexpected error"));

        var result = await PostCreatePersonHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Unexpected error");
        serviceMock.Verify(s => s.CreatePersonAsync(It.IsAny<Person>()), Times.Once);
    }
}
