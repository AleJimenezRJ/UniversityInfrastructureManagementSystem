using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;
public class DeleteCampusRequestTests
{
    [Fact]
    public void Constructor_SetsCampus()
    {
        var req = new DeleteCampusRequest("TestCampus");
        req.campus.Should().Be("TestCampus");
    }
}
