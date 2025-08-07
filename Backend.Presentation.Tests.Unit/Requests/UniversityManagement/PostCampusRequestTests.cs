using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class PostCampusRequestTests
{
    [Fact]
    public void Constructor_SetsCampus()
    {
        var dto = new AddCampusDto("Campus", "Loc", new AddCampusUniversityDto("Uni"));
        var req = new PostCampusRequest(dto);
        req.Campus.Should().BeSameAs(dto);
    }
}
