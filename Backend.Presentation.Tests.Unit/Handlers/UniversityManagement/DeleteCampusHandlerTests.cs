using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

public class DeleteCampusHandlerTests
{

    /// <summary>
    /// Tests that the handler returns an OK result when the campus is successfully deleted.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusDoesNotExist_ShouldReturnNotFound()
    {
        var mock = new Mock<ICampusServices>();
        var name = "NoExiste";

        mock.Setup(x => x.DeleteCampusAsync(name)).ReturnsAsync(false);

        var result = await DeleteCampusHandler.HandleAsync(mock.Object, name);

        var notFound = Assert.IsType<NotFound<DeleteCampusResponse>>(result.Result);
        notFound.Value!.ErrorMessage.Should().Contain($"Error deleting campus with name {name}");
    }


    /// <summary>
    /// Tests that the handler returns a Conflict result when a concurrency conflict occurs.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldCallServiceWithCorrectName()
    {
        var mock = new Mock<ICampusServices>();
        var name = "CampusPrueba";

        mock.Setup(x => x.DeleteCampusAsync(name))
            .ReturnsAsync(true)
            .Verifiable();

        await DeleteCampusHandler.HandleAsync(mock.Object, name);

        mock.Verify(x => x.DeleteCampusAsync(name), Times.Once);
    }
}
