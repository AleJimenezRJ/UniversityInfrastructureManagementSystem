using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppPres.Services.UniversityManagement;

/// <summary>
/// Integration tests for Area services and presentation layer handlers.
/// Tests the interaction between Application services and Presentation handlers.
/// </summary>
public class AreaServicesIntegrationTest
{
    private readonly Mock<IAreaRepository> _areaRepositoryMock;
    private readonly Mock<ICampusServices> _campusServicesMock;
    private readonly IAreaServices _areaServices;

    public AreaServicesIntegrationTest()
    {
        _areaRepositoryMock = new Mock<IAreaRepository>();
        _campusServicesMock = new Mock<ICampusServices>();
        
        // Setup service collection for dependency injection
        var services = new ServiceCollection();
        services.AddScoped(_ => _areaRepositoryMock.Object);
        services.AddScoped<IAreaServices, UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations.AreaServices>();
        
        var serviceProvider = services.BuildServiceProvider();
        _areaServices = serviceProvider.GetRequiredService<IAreaServices>();
    }

    // ==============================
    // POST Area Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between PostAreaHandler and AreaServices for successful area creation.
    /// </summary>
    [Fact]
    public async Task GivenValidAreaDto_WhenHandlerCallsService_ReturnsSuccessResponse()
    {
        // Arrange
        var areaDto = new AddAreaDto(
            "Finca 1",
            new AddAreaCampusDto("Campus Central")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );

        _campusServicesMock.Setup(s => s.GetByNameAsync("Campus Central"))
            .ReturnsAsync(campus);
        
        _areaRepositoryMock.Setup(r => r.AddAreaAsync(It.IsAny<Area>()))
            .ReturnsAsync(true);

        // Act - Test handler integration with service
        var result = await PostAreaHandler.HandleAsync(_areaServices, _campusServicesMock.Object, areaDto);

        // Assert
        Assert.IsType<Ok<PostAreaResponse>>(result.Result);
        var okResult = (Ok<PostAreaResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Area.Should().BeEquivalentTo(areaDto);
        
        _areaRepositoryMock.Verify(r => r.AddAreaAsync(It.Is<Area>(a => 
            a.Name!.Name == "Finca 1")), Times.Once);
    }

    /// <summary>
    /// Tests the integration when area creation fails in the repository.
    /// </summary>
    [Fact]
    public async Task GivenRepositoryFailure_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var areaDto = new AddAreaDto(
            "Area Existente",
            new AddAreaCampusDto("Campus Central")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );

        _campusServicesMock.Setup(s => s.GetByNameAsync("Campus Central"))
            .ReturnsAsync(campus);
        
        _areaRepositoryMock.Setup(r => r.AddAreaAsync(It.IsAny<Area>()))
            .ReturnsAsync(false);

        // Act
        var result = await PostAreaHandler.HandleAsync(_areaServices, _campusServicesMock.Object, areaDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value.Should().NotBeNull();
        conflictResult.Value!.ErrorMessages.Should().Contain("Area could not be added.");
    }

    /// <summary>
    /// Tests the integration when a duplicate area exception is thrown.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateArea_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var areaDto = new AddAreaDto(
            "Area Duplicada",
            new AddAreaCampusDto("Campus Central")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );

        _campusServicesMock.Setup(s => s.GetByNameAsync("Campus Central"))
            .ReturnsAsync(campus);
        
        _areaRepositoryMock.Setup(r => r.AddAreaAsync(It.IsAny<Area>()))
            .ThrowsAsync(new DuplicatedEntityException("Area with this name already exists"));

        // Act
        var result = await PostAreaHandler.HandleAsync(_areaServices, _campusServicesMock.Object, areaDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value.Should().NotBeNull();
        conflictResult.Value!.ErrorMessages.Should().Contain("Area with this name already exists");
    }

    // ==============================
    // GET Area Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetAreaHandler and AreaServices for successful area retrieval.
    /// </summary>
    [Fact]
    public async Task GivenExistingAreas_WhenHandlerCallsService_ReturnsAreaList()
    {
        // Arrange
        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );

        var areas = new List<Area>
        {
            new Area(new EntityName("Finca 1"), campus),
            new Area(new EntityName("Finca 2"), campus)
        };

        _areaRepositoryMock.Setup(r => r.ListAreaAsync())
            .ReturnsAsync(areas);

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServices);

        // Assert
        Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        var okResult = (Ok<GetAreaResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Area.Should().HaveCount(2);
        okResult.Value.Area.Should().Contain(a => a.Name == "Finca 1");
        okResult.Value.Area.Should().Contain(a => a.Name == "Finca 2");
    }

    /// <summary>
    /// Tests the integration when no areas exist in the repository.
    /// </summary>
    [Fact]
    public async Task GivenNoAreas_WhenHandlerCallsService_ReturnsEmptyList()
    {
        // Arrange
        _areaRepositoryMock.Setup(r => r.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServices);

        // Assert
        Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        var okResult = (Ok<GetAreaResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Area.Should().BeEmpty();
    }

    // ==============================
    // GET Area By Name Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetAreaByNameHandler and AreaServices for successful area retrieval by name.
    /// </summary>
    [Fact]
    public async Task GivenExistingAreaName_WhenHandlerCallsService_ReturnsArea()
    {
        // Arrange
        var areaName = "Finca 1";
        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(new EntityName(areaName), campus);

        _areaRepositoryMock.Setup(r => r.GetByNameAsync(areaName))
            .ReturnsAsync(area);

        // Act
        var result = await GetAreaByNameHandler.HandleAsync(_areaServices, areaName);

        // Assert
        Assert.IsType<Ok<GetAreaByNameResponse>>(result.Result);
        var okResult = (Ok<GetAreaByNameResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Area.Name.Should().Be(areaName);
    }

    /// <summary>
    /// Tests the integration when area is not found by name.
    /// </summary>
    [Fact]
    public async Task GivenNonExistingAreaName_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var areaName = "Area Inexistente";
        
        _areaRepositoryMock.Setup(r => r.GetByNameAsync(areaName))
            .ReturnsAsync((Area?)null);

        // Act
        var result = await GetAreaByNameHandler.HandleAsync(_areaServices, areaName);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
        var notFoundResult = (NotFound<string>)result.Result;
        notFoundResult.Value.Should().Contain("not found");
    }

    // ==============================
    // DELETE Area Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between DeleteAreaHandler and AreaServices for successful area deletion.
    /// </summary>
    [Fact]
    public async Task GivenExistingAreaName_WhenHandlerCallsService_ReturnsSuccessMessage()
    {
        // Arrange
        var areaName = "Finca 1";
        
        _areaRepositoryMock.Setup(r => r.DeleteAreaAsync(areaName))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteAreaHandler.HandleAsync(_areaServices, areaName);

        // Assert
        Assert.IsType<Ok<string>>(result.Result);
        var okResult = (Ok<string>)result.Result;
        okResult.Value.Should().Contain("has been deleted from the system successfully");
    }

    /// <summary>
    /// Tests the integration when area deletion fails (area not found).
    /// </summary>
    [Fact]
    public async Task GivenNonExistingAreaName_WhenHandlerCallsDeleteService_ReturnsNotFound()
    {
        // Arrange
        var areaName = "Area Inexistente";
        
        _areaRepositoryMock.Setup(r => r.DeleteAreaAsync(areaName))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteAreaHandler.HandleAsync(_areaServices, areaName);

        // Assert
        Assert.IsType<NotFound<DeleteAreaResponse>>(result.Result);
        var notFoundResult = (NotFound<DeleteAreaResponse>)result.Result;
        notFoundResult.Value!.ErrorMessage.Should().Contain("Error deleting area");
    }
}
