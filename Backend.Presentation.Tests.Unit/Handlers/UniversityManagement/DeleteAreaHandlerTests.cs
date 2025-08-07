using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

public class DeleteAreaHandlerTests
{

    [Fact]
    public async Task HandleAsync_WhenAreaDoesNotExist_ShouldReturnNotFound()
    {
        var mock = new Mock<IAreaServices>();
        var name = "NoExiste";

        mock.Setup(x => x.DeleteAreaAsync(name)).ReturnsAsync(false);

        var result = await DeleteAreaHandler.HandleAsync(mock.Object, name);

        var notFound = Assert.IsType<NotFound<DeleteAreaResponse>>(result.Result);
        notFound.Value!.ErrorMessage.Should().Contain($"Error deleting area with name {name}");
    }

    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ShouldReturnConflict()
    {
        var mock = new Mock<IAreaServices>();
        var name = "Bloqueo";

        mock.Setup(x => x.DeleteAreaAsync(name))
            .ThrowsAsync(new ConcurrencyConflictException("simulated"));

        var result = await DeleteAreaHandler.HandleAsync(mock.Object, name);

        var conflict = Assert.IsType<Conflict<string>>(result.Result);
        conflict.Value.Should().Contain("concurrency conflict");
    }

    [Fact]
    public async Task HandleAsync_ShouldCallServiceWithCorrectName()
    {
        var mock = new Mock<IAreaServices>();
        var name = "AreaPrueba";

        mock.Setup(x => x.DeleteAreaAsync(name))
            .ReturnsAsync(true)
            .Verifiable();

        await DeleteAreaHandler.HandleAsync(mock.Object, name);

        mock.Verify(x => x.DeleteAreaAsync(name), Times.Once);
    }
}
