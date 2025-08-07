using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// A test class for the GetAllUserAuditHandler.
/// </summary>
public class GetAllUserAuditHandlerTests
{
    /// <summary>
    /// Mock service for user audit operations.
    /// </summary>
    private readonly Mock<IUserAuditService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUserAuditHandler to ensure it returns Ok with a list of user audits when audits exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOkWithList_WhenAuditsExist()
    {
        var audits = new List<UserAudit>
        {
            new UserAudit
            {
                AuditId = 1,
                UserName = "B12345",
                FirstName = "Ana",
                LastName = "Ramírez",
                Email = "ana@ucr.ac.cr",
                Phone = "8888-0000",
                IdentityNumber = "C123456789",
                BirthDate = new DateTime(1995, 4, 23),
                ModifiedAt = DateTime.Now,
                Action = "login"
            },
            new UserAudit
            {
                AuditId = 2,
                UserName = "B12345",
                FirstName = "Ana",
                LastName = "Ramírez",
                Email = "ana@ucr.ac.cr",
                Phone = "8888-0000",
                IdentityNumber = "C123456789",
                BirthDate = new DateTime(1995, 4, 23),
                ModifiedAt = DateTime.Now,
                Action = "logout"
            }
        };

        _mockService.Setup(s => s.ListUserAuditAsync()).ReturnsAsync(audits);

        var result = await GetAllUserAuditHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Ok<List<UserAuditDto>>>();
        var value = ((Ok<List<UserAuditDto>>)result.Result!).Value;
        value.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUserAuditHandler to ensure it returns Ok with a message when no audits exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOkWithMessage_WhenNoAuditsExist()
    {
        _mockService.Setup(s => s.ListUserAuditAsync()).ReturnsAsync(new List<UserAudit>());

        var result = await GetAllUserAuditHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Ok<string>>();
        var value = ((Ok<string>)result.Result!).Value!;
        value.ToLower().Should().Contain("no registered user audits");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllUserAuditHandler to ensure it returns Ok with a message when the audit list is null.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOkWithMessage_WhenAuditListIsNull()
    {
        _mockService.Setup(s => s.ListUserAuditAsync()).ReturnsAsync((List<UserAudit>?)null);

        var result = await GetAllUserAuditHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Ok<string>>();
        var value = ((Ok<string>)result.Result!).Value!;
        value.ToLower().Should().Contain("no registered user audits");
    }
}
