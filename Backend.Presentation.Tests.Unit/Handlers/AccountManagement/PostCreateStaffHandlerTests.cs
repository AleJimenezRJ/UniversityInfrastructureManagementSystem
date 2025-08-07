using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Handlers;

/// <summary>
/// Unit tests for the PostCreateStaffHandler class.
/// </summary>
public class PostCreateStaffHandlerTests
{
    /// <summary>
    /// Creates a valid CreateStaffDto object for testing purposes.
    /// </summary>
    /// <returns>A valid <see cref="CreateStaffDto"/> object.</returns>
    private static CreateStaffDto CreateValidDto() => new(
        Type: "Admin",
        Email: "personal@email.com",
        FirstName: "Tatiana",
        LastName: "Paramo",
        Phone: "8888-9999",
        BirthDate: new DateOnly(1998, 6, 15),
        IdentityNumber: "1-1234-5678",
        InstitutionalEmail: "staff@ucr.ac.cr"
    );

    /// <summary>
    /// Tests that the handler returns an Ok result when given a valid request.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenHandleAsync_ReturnsOk()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStaffRequest(dto);

        var serviceMock = new Mock<IStaffService>();
        serviceMock.Setup(s => s.CreateStaffAsync(It.IsAny<Staff>()))
                   .ReturnsAsync(true);

        var result = await PostCreateStaffHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Ok<PostCreateStaffResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest result when given an invalid request.
    /// </summary>
    [Fact]
    public async Task GivenInvalidRequest_WhenHandleAsync_ReturnsBadRequest()
    {
        var invalidDto = new CreateStaffDto(
            Type: "",
            Email: "invalido.com",
            FirstName: "",
            LastName: "",
            Phone: "123456",
            BirthDate: new DateOnly(3000, 1, 1),
            IdentityNumber: "abcd",
            InstitutionalEmail: "invalidmail"
        );

        var request = new PostCreateStaffRequest(invalidDto);
        var serviceMock = new Mock<IStaffService>();

        var result = await PostCreateStaffHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that the handler returns a Conflict result when the staff already exists.
    /// </summary>
    [Fact]
    public async Task GivenExistingStaff_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStaffRequest(dto);

        var serviceMock = new Mock<IStaffService>();
        serviceMock.Setup(s => s.CreateStaffAsync(It.IsAny<Staff>()))
                   .ReturnsAsync(false);

        var result = await PostCreateStaffHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("A staff with this identity or institutional email already exists.");
    }

    /// <summary>
    /// Tests that the handler returns a Conflict result when a domain error occurs.
    /// </summary>
    [Fact]
    public async Task GivenDomainError_WhenHandleAsync_ReturnsConflict()
    {
        var dto = CreateValidDto();
        var request = new PostCreateStaffRequest(dto);

        var serviceMock = new Mock<IStaffService>();
        serviceMock.Setup(s => s.CreateStaffAsync(It.IsAny<Staff>()))
                   .ThrowsAsync(new DomainException("Domain rule violated."));

        var result = await PostCreateStaffHandler.HandleAsync(serviceMock.Object, request);

        result.Result.Should().BeOfType<Conflict<string>>()
            .Which.Value.Should().Be("Domain rule violated.");
    }
}
