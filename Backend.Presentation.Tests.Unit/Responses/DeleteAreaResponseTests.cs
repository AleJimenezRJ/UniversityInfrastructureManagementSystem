using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;
public class DeleteAreaResponseTests
{
    [Fact]
    public void DefaultConstructor_SetsErrorMessageNull()
    {
        var resp = new DeleteAreaResponse();
        resp.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Constructor_SetsErrorMessage()
    {
        var resp = new DeleteAreaResponse("error");
        resp.ErrorMessage.Should().Be("error");
    }
}
