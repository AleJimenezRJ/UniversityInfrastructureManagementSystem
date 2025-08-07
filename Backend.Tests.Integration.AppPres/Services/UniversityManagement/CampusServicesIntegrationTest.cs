using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppPres.Services.UniversityManagement;

/// <summary>
/// Integration tests for Campus services and presentation layer handlers.
/// Tests the interaction between Application services and Presentation handlers.
/// </summary>
public class CampusServicesIntegrationTest
{
    private readonly Mock<ICampusRepository> _campusRepositoryMock;
    private readonly Mock<IUniversityRepository> _universityRepositoryMock;
    private readonly ICampusServices _campusServices;
    private readonly IUniversityServices _universityServices;

    public CampusServicesIntegrationTest()
    {
        _campusRepositoryMock = new Mock<ICampusRepository>();
        _universityRepositoryMock = new Mock<IUniversityRepository>();
        
        // Setup service collection for dependency injection
        var services = new ServiceCollection();
        services.AddScoped(_ => _campusRepositoryMock.Object);
        services.AddScoped(_ => _universityRepositoryMock.Object);
        services.AddScoped<ICampusServices, UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations.CampusServices>();
        services.AddScoped<IUniversityServices, UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations.UniversityServices>();
        
        var serviceProvider = services.BuildServiceProvider();
        _campusServices = serviceProvider.GetRequiredService<ICampusServices>();
        _universityServices = serviceProvider.GetRequiredService<IUniversityServices>();
    }

    // ==============================
    // POST Campus Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between PostCampusHandler and CampusServices for successful campus creation.
    /// </summary>
    [Fact]
    public async Task GivenValidCampusDto_WhenHandlerCallsService_ReturnsSuccessResponse()
    {
        // Arrange
        var campusDto = new AddCampusDto(
            "Campus Central",
            "San Pedro",
            new AddCampusUniversityDto("Universidad de Costa Rica")
        );
        
        var university = new University(new EntityName("Universidad de Costa Rica"));
        _universityRepositoryMock.Setup(r => r.GetByNameAsync("Universidad de Costa Rica"))
            .ReturnsAsync(university);
        
        _campusRepositoryMock.Setup(r => r.AddCampusAsync(It.IsAny<Campus>()))
            .ReturnsAsync(true);

        // Act - Test handler integration with service
        var result = await PostCampusHandler.HandleAsync(_campusServices, _universityServices, campusDto);

        // Assert
        Assert.IsType<Ok<PostCampusResponse>>(result.Result);
        var okResult = (Ok<PostCampusResponse>)result.Result;
        okResult.Value!.Campus.Name.Should().Be("Campus Central");
        okResult.Value.Campus.Location.Should().Be("San Pedro");
        
        _campusRepositoryMock.Verify(r => r.AddCampusAsync(It.Is<Campus>(c => 
            c.Name!.Name == "Campus Central" && 
            c.Location!.Location == "San Pedro")), Times.Once);
    }

    /// <summary>
    /// Tests the integration when campus creation fails in the repository.
    /// </summary>
    [Fact]
    public async Task GivenRepositoryFailure_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var campusDto = new AddCampusDto(
            "Campus Existente",
            "Cartago",
            new AddCampusUniversityDto("Universidad de Costa Rica")
        );
        
        var university = new University(new EntityName("Universidad de Costa Rica"));
        _universityRepositoryMock.Setup(r => r.GetByNameAsync("Universidad de Costa Rica"))
            .ReturnsAsync(university);
        
        _campusRepositoryMock.Setup(r => r.AddCampusAsync(It.IsAny<Campus>()))
            .ReturnsAsync(false);

        // Act
        var result = await PostCampusHandler.HandleAsync(_campusServices, _universityServices, campusDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value!.ErrorMessages.Should().Contain("Campus could not be added.");
    }

    /// <summary>
    /// Tests the integration when a duplicate campus exception is thrown.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateCampus_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var campusDto = new AddCampusDto(
            "Campus Duplicado",
            "Heredia",
            new AddCampusUniversityDto("Universidad de Costa Rica")
        );
        
        var university = new University(new EntityName("Universidad de Costa Rica"));
        _universityRepositoryMock.Setup(r => r.GetByNameAsync("Universidad de Costa Rica"))
            .ReturnsAsync(university);
        
        _campusRepositoryMock.Setup(r => r.AddCampusAsync(It.IsAny<Campus>()))
            .ThrowsAsync(new DuplicatedEntityException("Campus with this name already exists"));

        // Act
        var result = await PostCampusHandler.HandleAsync(_campusServices, _universityServices, campusDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value!.ErrorMessages.Should().Contain("Campus with this name already exists");
    }

    /// <summary>
    /// Tests the integration when the specified university does not exist.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentUniversity_WhenHandlerCallsService_ReturnsBadRequest()
    {
        // Arrange
        var campusDto = new AddCampusDto(
            "Campus Test",
            "Test Location",
            new AddCampusUniversityDto("Universidad Inexistente")
        );
        
        _universityRepositoryMock.Setup(r => r.GetByNameAsync("Universidad Inexistente"))
            .ReturnsAsync((University?)null);

        // Act
        var result = await PostCampusHandler.HandleAsync(_campusServices, _universityServices, campusDto);

        // Assert
        Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        var badRequestResult = (BadRequest<ErrorResponse>)result.Result;
        badRequestResult.Value!.ErrorMessages.Should().Contain("The specified university does not exist.");
    }

    // ==============================
    // GET All Campus Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetCampusHandler and CampusServices for retrieving all campuses.
    /// </summary>
    [Fact]
    public async Task GivenExistingCampuses_WhenHandlerCallsService_ReturnsCampusList()
    {
        // Arrange
        var university = new University(new EntityName("Universidad de Costa Rica"), new EntityLocation("Costa Rica"));
        var campuses = new List<Campus>
        {
            new Campus(new EntityName("Campus Central"), new EntityLocation("San Pedro"), university),
            new Campus(new EntityName("Campus Sur"), new EntityLocation("Cartago"), university)
        };
        
        _campusRepositoryMock.Setup(r => r.ListCampusAsync())
            .ReturnsAsync(campuses);

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServices);

        // Assert
        Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        var okResult = (Ok<GetCampusResponse>)result.Result;
        okResult.Value!.Campus.Should().HaveCount(2);
        okResult.Value.Campus.Should().Contain(c => c.Name == "Campus Central");
        okResult.Value.Campus.Should().Contain(c => c.Name == "Campus Sur");
    }

    /// <summary>
    /// Tests the integration when no campuses exist.
    /// </summary>
    [Fact]
    public async Task GivenNoCampuses_WhenHandlerCallsService_ReturnsEmptyList()
    {
        // Arrange
        _campusRepositoryMock.Setup(r => r.ListCampusAsync())
            .ReturnsAsync(new List<Campus>());

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServices);

        // Assert
        Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        var okResult = (Ok<GetCampusResponse>)result.Result;
        okResult.Value!.Campus.Should().BeEmpty();
    }

    // ==============================
    // GET Campus By Name Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetCampusByNameHandler and CampusServices for retrieving a specific campus.
    /// </summary>
    [Fact]
    public async Task GivenExistingCampusName_WhenHandlerCallsService_ReturnsCampus()
    {
        // Arrange
        var campusName = "Campus Central";
        var university = new University(new EntityName("Universidad de Costa Rica"), new EntityLocation("Costa Rica"));
        var campus = new Campus(new EntityName(campusName), new EntityLocation("San Pedro"), university);
        
        _campusRepositoryMock.Setup(r => r.GetByNameAsync(campusName))
            .ReturnsAsync(campus);

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(_campusServices, campusName);

        // Assert
        Assert.IsType<Ok<GetCampusByNameResponse>>(result.Result);
        var okResult = (Ok<GetCampusByNameResponse>)result.Result;
        okResult.Value!.Campus.Name.Should().Be(campusName);
    }

    /// <summary>
    /// Tests the integration when the requested campus does not exist.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentCampusName_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var campusName = "Campus Inexistente";
        
        _campusRepositoryMock.Setup(r => r.GetByNameAsync(campusName))
            .ReturnsAsync((Campus?)null);

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(_campusServices, campusName);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
        var notFoundResult = (NotFound<string>)result.Result;
        notFoundResult.Value.Should().Contain("not found");
    }

    // ==============================
    // DELETE Campus Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between DeleteCampusHandler and CampusServices for successful campus deletion.
    /// </summary>
    [Fact]
    public async Task GivenExistingCampus_WhenHandlerCallsService_ReturnsSuccessMessage()
    {
        // Arrange
        var campusName = "Campus a Eliminar";
        
        _campusRepositoryMock.Setup(r => r.DeleteCampusAsync(campusName))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteCampusHandler.HandleAsync(_campusServices, campusName);

        // Assert
        Assert.IsType<Ok<string>>(result.Result);
        var okResult = (Ok<string>)result.Result;
        okResult.Value.Should().Contain("has been deleted from the system successfully");
        
        _campusRepositoryMock.Verify(r => r.DeleteCampusAsync(campusName), Times.Once);
    }

    /// <summary>
    /// Tests the integration when the campus to delete does not exist.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentCampus_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var campusName = "Campus Inexistente";
        
        _campusRepositoryMock.Setup(r => r.DeleteCampusAsync(campusName))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteCampusHandler.HandleAsync(_campusServices, campusName);

        // Assert
        Assert.IsType<NotFound<DeleteCampusResponse>>(result.Result);
        var notFoundResult = (NotFound<DeleteCampusResponse>)result.Result;
        notFoundResult.Value!.ErrorMessage.Should().Contain("Error deleting campus");
    }

    // ==============================
    // Service Layer Only Tests
    // ==============================

    /// <summary>
    /// Tests the campus service directly for adding campuses.
    /// </summary>
    [Fact]
    public async Task GivenValidCampus_WhenServiceAddsCampus_ReturnsTrue()
    {
        // Arrange
        var university = new University(new EntityName("Universidad de Costa Rica"));
        var campus = new Campus(new EntityName("Campus Test"), new EntityLocation("Test Location"), university);
        
        _campusRepositoryMock.Setup(r => r.AddCampusAsync(campus))
            .ReturnsAsync(true);

        // Act
        var result = await _campusServices.AddCampusAsync(campus);

        // Assert
        result.Should().BeTrue();
        _campusRepositoryMock.Verify(r => r.AddCampusAsync(campus), Times.Once);
    }

    /// <summary>
    /// Tests the campus service directly for listing campuses.
    /// </summary>
    [Fact]
    public async Task GivenMultipleCampuses_WhenServiceListsCampuses_ReturnsAllCampuses()
    {
        // Arrange
        var university = new University(new EntityName("Universidad de Costa Rica"));
        var expectedCampuses = new List<Campus>
        {
            new Campus(new EntityName("Campus 1"), new EntityLocation("Location 1"), university),
            new Campus(new EntityName("Campus 2"), new EntityLocation("Location 2"), university)
        };
        
        _campusRepositoryMock.Setup(r => r.ListCampusAsync())
            .ReturnsAsync(expectedCampuses);

        // Act
        var result = await _campusServices.ListCampusAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedCampuses);
        _campusRepositoryMock.Verify(r => r.ListCampusAsync(), Times.Once);
    }

    /// <summary>
    /// Tests the campus service directly for getting a campus by name.
    /// </summary>
    [Fact]
    public async Task GivenCampusName_WhenServiceGetsCampusByName_ReturnsCampus()
    {
        // Arrange
        var campusName = "Campus Específico";
        var university = new University(new EntityName("Universidad de Costa Rica"));
        var expectedCampus = new Campus(new EntityName(campusName), new EntityLocation("Test Location"), university);
        
        _campusRepositoryMock.Setup(r => r.GetByNameAsync(campusName))
            .ReturnsAsync(expectedCampus);

        // Act
        var result = await _campusServices.GetByNameAsync(campusName);

        // Assert
        result.Should().BeEquivalentTo(expectedCampus);
        _campusRepositoryMock.Verify(r => r.GetByNameAsync(campusName), Times.Once);
    }
}
