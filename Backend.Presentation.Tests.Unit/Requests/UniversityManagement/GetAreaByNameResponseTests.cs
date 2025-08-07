using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class GetAreaByNameResponseTests
{
    [Fact]
    public void Constructor_SetsArea()
    {
        var dto = new ListAreaDto("Area", new ListCampusDto("Campus", "Location", new UniversityDto("University", "Costa Rica")));
        var resp = new GetAreaByNameResponse(dto);
        resp.Area.Should().BeSameAs(dto);
    }
}
