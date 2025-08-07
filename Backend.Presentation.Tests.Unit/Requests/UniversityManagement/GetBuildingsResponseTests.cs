using System.Collections.Generic;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class GetBuildingsResponseTests
{
    [Fact]
    public void Constructor_SetsBuildings()
    {
        var list = new List<ListBuildingDto> { new ListBuildingDto(
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
            ) };
        var resp = new GetBuildingResponse(list);
        resp.Buildings.Should().BeSameAs(list);
    }
}
