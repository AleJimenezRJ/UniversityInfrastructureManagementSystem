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

/// <summary>
/// Unit tests for the <see cref="PostCreateStudentHandler"/> class.
/// </summary>
public class PostCreateStudentHandlerTests
{
    /// <summary>
    /// Creates a valid <see cref="CreateStudentDto"/> instance for testing purposes.
    /// </summary>
    /// <returns>A valid <see cref="CreateStudentDto"/> instance.</returns>
    private static CreateStudentDto CreateValidDto() => new(
        StudentId: "B12345",
        Email: "student@example.com",
        FirstName: "Isabella",
        LastName: "González",
        Phone: "8888-1234",
        BirthDate: new DateOnly(2000, 5, 10),
        IdentityNumber: "1-9999-9999",
        InstitutionalEmail: "isabella.gonzalez@ucr.ac.cr"
    );

    /// <summary>
    /// Tests that a valid request returns an <see cref="Ok{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStudentRequest(dto);

        var serviceMock = new Mock<IStudentService>();
        serviceMock.Setup(s => s.CreateStudentAsync(It.IsAny<Student>()))
                   .ReturnsAsync(true);

        var result = await PostCreateStudentHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Ok<PostCreateStudentResponse>>();
    }

    /// <summary>
    /// Tests that an invalid request returns a <see cref="BadRequest{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenInvalidRequest_WhenHandleAsync_ReturnsBadRequest()
    {
        var invalidDto = new CreateStudentDto(
            StudentId: "",
            Email: "invalid.com",
            FirstName: "",
            LastName: "",
            Phone: "123",
            BirthDate: new DateOnly(3000, 1, 1),
            IdentityNumber: "abc",
            InstitutionalEmail: "not-an-email"
        );

        var request = new PostCreateStudentRequest(invalidDto);
        var serviceMock = new Mock<IStudentService>();

        var result = await PostCreateStudentHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that a request for an existing student returns a <see cref="Conflict{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenExistingStudent_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStudentRequest(dto);

        var serviceMock = new Mock<IStudentService>();
        serviceMock.Setup(s => s.CreateStudentAsync(It.IsAny<Student>()))
                   .ReturnsAsync(false);

        var result = await PostCreateStudentHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("A student with this identity or institutional email already exists.");
    }

    /// <summary>
    /// Tests that a domain exception returns a <see cref="Conflict{T}"/> result.
    /// </summary>
    [Fact]
    public async Task GivenDomainException_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStudentRequest(dto);

        var serviceMock = new Mock<IStudentService>();
        serviceMock.Setup(s => s.CreateStudentAsync(It.IsAny<Student>()))
                   .ThrowsAsync(new DomainException("Domain rule violated."));

        var result = await PostCreateStudentHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Domain rule violated.");
    }

    /// <summary>
    /// Tests that the <see cref="IStudentService.CreateStudentAsync"/> method is called exactly once for a valid request.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_CallsStudentServiceOnce()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStudentRequest(dto);

        var serviceMock = new Mock<IStudentService>();
        serviceMock.Setup(s => s.CreateStudentAsync(It.IsAny<Student>()))
                   .ReturnsAsync(true);

        await PostCreateStudentHandler.HandleAsync(serviceMock.Object, request);

        serviceMock.Verify(s => s.CreateStudentAsync(It.IsAny<Student>()), Times.Once);
    }
}
