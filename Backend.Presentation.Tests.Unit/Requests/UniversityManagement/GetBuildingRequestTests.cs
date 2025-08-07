using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class GetBuildingRequestTests
{
    [Fact]
    public void Constructor_SetsBuilding()
    {
        var dto = new ListBuildingDto(
            1,
            "Building A",
            10.0,
            20.0,
            30.0,
            50.0,
            60.0,
            70.0,
            "Red",
            new ListAreaDto("Area", new ListCampusDto("Campus", "Location", new UniversityDto("University", "Costa Rica")))
            ); 
        var req = new GetBuildingRequest(dto);
        req.Building.Should().BeSameAs(dto);
    }
}
