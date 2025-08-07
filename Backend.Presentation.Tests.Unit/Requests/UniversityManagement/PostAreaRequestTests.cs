using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class PostAreaRequestTests
{
    [Fact]
    public void Constructor_SetsArea()
    {
        var dto = new AddAreaDto("Area", new AddAreaCampusDto("Campus"));
        var req = new PostAreaRequest(dto);
        req.Area.Should().BeSameAs(dto);
    }
}
