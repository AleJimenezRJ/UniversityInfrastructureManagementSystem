using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class DeleteUniversityRequestTests
{
    [Fact]
    public void Constructor_SetsUniversityName()
    {
        var req = new DeleteUniversityRequest("TestUniversity");
        req.UniversityName.Should().Be("TestUniversity");
    }
}
