using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses.UniversityManagement;
public class DeleteBuildingResponseTests
{
    [Fact]
    public void DefaultConstructor_SetsErrorMessageNull()
    {
        var resp = new DeleteBuildingResponse();
        resp.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Constructor_SetsErrorMessage()
    {
        var resp = new DeleteBuildingResponse("error");
        resp.ErrorMessage.Should().Be("error");
    }
}
