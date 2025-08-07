using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Requests.UniversityManagement;

public class PostBuildingRequestTests
{
    [Fact]
    public void Constructor_SetsBuilding()
    {
        var dto = new AddBuildingDto(
            "Building A",
            10.0,
            20.0,
            30.0,
            50.0,
            60.0,
            70.0,
            "Red", 
            new AddBuildingAreaDto("Area")
        );
        var req = new PostBuildingRequest(dto);
        req.Building.Should().BeSameAs(dto);
    }
}
