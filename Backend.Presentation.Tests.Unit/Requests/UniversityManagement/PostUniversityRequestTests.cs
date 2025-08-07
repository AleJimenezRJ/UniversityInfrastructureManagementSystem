using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class PostUniversityRequestTests
{
    [Fact]
    public void Constructor_SetsUniversity()
    {
        var dto = new AddCampusUniversityDto("University");
        var req = new PostUniversityRequest(dto);
        req.University.Should().BeSameAs(dto);
    }
}
