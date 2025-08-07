using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Handlers;

public class PostCreateUserHandlerTests
{
    private static CreateUserDto CreateValidDto() => new(
        UserName: "andresmurillo",
        PersonId: 123
    );

    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserRequest(dto);
        var serviceMock = new Mock<IUserService>();

        serviceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>()));

        var result = await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Ok<PostCreateUserResponse>>();
    }

    [Fact]
    public async Task GivenInvalidRequest_WhenHandleAsync_ReturnsBadRequest()
    {
        var invalidDto = new CreateUserDto(UserName: "", PersonId: -1);
        var request = new PostCreateUserRequest(invalidDto);
        var serviceMock = new Mock<IUserService>();

        var result = await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    [Fact]
    public async Task GivenNonExistentPerson_WhenHandleAsync_ReturnsNotFound()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserRequest(dto);
        var serviceMock = new Mock<IUserService>();

        serviceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>()))
                   .ThrowsAsync(new NotFoundException("Person not found."));

        var result = await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<NotFound<string>>()
            .Which.Value.Should().Be("Person not found.");
    }

    [Fact]
    public async Task GivenDuplicatedUser_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserRequest(dto);
        var serviceMock = new Mock<IUserService>();

        serviceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>()))
                   .ThrowsAsync(new DuplicatedEntityException("User already exists."));

        var result = await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("User already exists.");
    }

    [Fact]
    public async Task GivenDomainError_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserRequest(dto);
        var serviceMock = new Mock<IUserService>();

        serviceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>()))
                   .ThrowsAsync(new DomainException("Username not allowed."));

        var result = await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Username not allowed.");
    }

    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_CallsServiceOnce()
    {
        var dto = CreateValidDto();
        var request = new PostCreateUserRequest(dto);
        var serviceMock = new Mock<IUserService>();

        serviceMock.Setup(s => s.CreateUserAsync(It.IsAny<User>()));

        await PostCreateUserHandler.HandleAsync(serviceMock.Object, request);

        serviceMock.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }
}
