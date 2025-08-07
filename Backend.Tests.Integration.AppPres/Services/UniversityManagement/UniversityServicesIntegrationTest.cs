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
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppPres.Services.UniversityManagement;

/// <summary>
/// Integration tests for University services and presentation layer handlers.
/// Tests the interaction between Application services and Presentation handlers.
/// </summary>
public class UniversityServicesIntegrationTest
{
    private readonly Mock<IUniversityRepository> _universityRepositoryMock;
    private readonly IUniversityServices _universityServices;

    public UniversityServicesIntegrationTest()
    {
        _universityRepositoryMock = new Mock<IUniversityRepository>();
        
        // Setup service collection for dependency injection
        var services = new ServiceCollection();
        services.AddScoped(_ => _universityRepositoryMock.Object);
        services.AddScoped<IUniversityServices, UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations.UniversityServices>();
        
        var serviceProvider = services.BuildServiceProvider();
        _universityServices = serviceProvider.GetRequiredService<IUniversityServices>();
    }

    // ==============================
    // POST University Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between PostUniversityHandler and UniversityServices for successful university creation.
    /// </summary>
    [Fact]
    public async Task GivenValidUniversityDto_WhenHandlerCallsService_ReturnsSuccessResponse()
    {
        // Arrange
        var universityDto = new UniversityDto("Universidad de Costa Rica", "Costa Rica");
        
        _universityRepositoryMock.Setup(r => r.AddUniversityAsync(It.IsAny<University>()))
            .ReturnsAsync(true);

        // Act - Test handler integration with service
        var result = await PostUniversityHandler.HandleAsync(_universityServices, universityDto);

        // Assert
        Assert.IsType<Ok<PostUniversityResponse>>(result.Result);
        var okResult = (Ok<PostUniversityResponse>)result.Result;
        okResult.Value!.University.Should().BeEquivalentTo(universityDto);
        
        _universityRepositoryMock.Verify(r => r.AddUniversityAsync(It.Is<University>(u => 
            u.Name!.Name == "Universidad de Costa Rica")), Times.Once);
    }

    /// <summary>
    /// Tests the integration when university creation fails in the repository.
    /// </summary>
    [Fact]
    public async Task GivenRepositoryFailure_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var universityDto = new UniversityDto("Universidad Existente", "Costa Rica");
        
        _universityRepositoryMock.Setup(r => r.AddUniversityAsync(It.IsAny<University>()))
            .ReturnsAsync(false);

        // Act
        var result = await PostUniversityHandler.HandleAsync(_universityServices, universityDto);

        // Assert
        Assert.IsType<Conflict<UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ErrorResponse>)result.Result;
        conflictResult.Value!.ErrorMessages.Should().Contain("University could not be added.");
    }

    /// <summary>
    /// Tests the integration when a duplicate university exception is thrown.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateUniversity_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var universityDto = new UniversityDto("Universidad Duplicada", "Costa Rica");
        
        _universityRepositoryMock.Setup(r => r.AddUniversityAsync(It.IsAny<University>()))
            .ThrowsAsync(new DuplicatedEntityException("University with this name already exists"));

        // Act
        var result = await PostUniversityHandler.HandleAsync(_universityServices, universityDto);

        // Assert
        Assert.IsType<Conflict<UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ErrorResponse>)result.Result;
        conflictResult.Value!.ErrorMessages.Should().Contain("University with this name already exists");
    }

    // ==============================
    // GET All Universities Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetUniversityHandler and UniversityServices for retrieving all universities.
    /// </summary>
    [Fact]
    public async Task GivenExistingUniversities_WhenHandlerCallsService_ReturnsUniversitiesList()
    {
        // Arrange
        var universities = new List<University>
        {
            new University(new EntityName("Universidad de Costa Rica"), new EntityLocation("Costa Rica")),
            new University(new EntityName("Universidad Nacional"), new EntityLocation("Costa Rica"))
        };
        
        _universityRepositoryMock.Setup(r => r.ListUniversityAsync())
            .ReturnsAsync(universities);

        // Act
        var result = await GetUniversityHandler.HandleAsync(_universityServices);

        // Assert
        Assert.IsType<Ok<GetUniversityResponse>>(result.Result);
        var okResult = (Ok<GetUniversityResponse>)result.Result;
        okResult.Value!.University.Should().HaveCount(2);
        okResult.Value.University.Should().Contain(u => u.Name == "Universidad de Costa Rica");
        okResult.Value.University.Should().Contain(u => u.Name == "Universidad Nacional");
    }

    /// <summary>
    /// Tests the integration when no universities exist.
    /// </summary>
    [Fact]
    public async Task GivenNoUniversities_WhenHandlerCallsService_ReturnsEmptyList()
    {
        // Arrange
        _universityRepositoryMock.Setup(r => r.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        // Act
        var result = await GetUniversityHandler.HandleAsync(_universityServices);

        // Assert
        Assert.IsType<Ok<GetUniversityResponse>>(result.Result);
        var okResult = (Ok<GetUniversityResponse>)result.Result;
        okResult.Value!.University.Should().BeEmpty();
    }

    // ==============================
    // GET University By Name Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetUniversityByNameHandler and UniversityServices for retrieving a specific university.
    /// </summary>
    [Fact]
    public async Task GivenExistingUniversityName_WhenHandlerCallsService_ReturnsUniversity()
    {
        // Arrange
        var universityName = "Universidad de Costa Rica";
        var university = new University(new EntityName(universityName), new EntityLocation("Costa Rica"));
        
        _universityRepositoryMock.Setup(r => r.GetByNameAsync(universityName))
            .ReturnsAsync(university);

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_universityServices, universityName);

        // Assert
        Assert.IsType<Ok<GetUniversityByNameResponse>>(result.Result);
        var okResult = (Ok<GetUniversityByNameResponse>)result.Result;
        okResult.Value!.University.Name.Should().Be(universityName);
    }

    /// <summary>
    /// Tests the integration when the requested university does not exist.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentUniversityName_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var universityName = "Universidad Inexistente";
        
        _universityRepositoryMock.Setup(r => r.GetByNameAsync(universityName))
            .ReturnsAsync((University?)null);

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_universityServices, universityName);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
        var notFoundResult = (NotFound<string>)result.Result;
        notFoundResult.Value.Should().Contain("not found");
    }

    // ==============================
    // DELETE University Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between DeleteUniversityHandler and UniversityServices for successful university deletion.
    /// </summary>
    [Fact]
    public async Task GivenExistingUniversity_WhenHandlerCallsService_ReturnsSuccessMessage()
    {
        // Arrange
        var universityName = "Universidad a Eliminar";
        
        _universityRepositoryMock.Setup(r => r.DeleteUniversityAsync(universityName))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteUniversityHandler.HandleAsync(_universityServices, universityName);

        // Assert
        Assert.IsType<Ok<string>>(result.Result);
        var okResult = (Ok<string>)result.Result;
        okResult.Value.Should().Contain("has been deleted from the system successfully");
        
        _universityRepositoryMock.Verify(r => r.DeleteUniversityAsync(universityName), Times.Once);
    }

    /// <summary>
    /// Tests the integration when the university to delete does not exist.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentUniversity_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var universityName = "Universidad Inexistente";
        
        _universityRepositoryMock.Setup(r => r.DeleteUniversityAsync(universityName))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteUniversityHandler.HandleAsync(_universityServices, universityName);

        // Assert
        Assert.IsType<NotFound<DeleteUniversityResponse>>(result.Result);
        var notFoundResult = (NotFound<DeleteUniversityResponse>)result.Result;
        notFoundResult.Value!.ErrorMessage.Should().Contain("Error deleting university");
    }

    // ==============================
    // Service Layer Only Tests
    // ==============================

    /// <summary>
    /// Tests the university service directly for adding universities.
    /// </summary>
    [Fact]
    public async Task GivenValidUniversity_WhenServiceAddsUniversity_ReturnsTrue()
    {
        // Arrange
        var university = new University(new EntityName("Universidad Test"));
        
        _universityRepositoryMock.Setup(r => r.AddUniversityAsync(university))
            .ReturnsAsync(true);

        // Act
        var result = await _universityServices.AddUniversityAsync(university);

        // Assert
        result.Should().BeTrue();
        _universityRepositoryMock.Verify(r => r.AddUniversityAsync(university), Times.Once);
    }

    /// <summary>
    /// Tests the university service directly for listing universities.
    /// </summary>
    [Fact]
    public async Task GivenMultipleUniversities_WhenServiceListsUniversities_ReturnsAllUniversities()
    {
        // Arrange
        var expectedUniversities = new List<University>
        {
            new University(new EntityName("Universidad 1")),
            new University(new EntityName("Universidad 2"))
        };
        
        _universityRepositoryMock.Setup(r => r.ListUniversityAsync())
            .ReturnsAsync(expectedUniversities);

        // Act
        var result = await _universityServices.ListUniversityAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedUniversities);
        _universityRepositoryMock.Verify(r => r.ListUniversityAsync(), Times.Once);
    }

    /// <summary>
    /// Tests the university service directly for getting a university by name.
    /// </summary>
    [Fact]
    public async Task GivenUniversityName_WhenServiceGetsUniversityByName_ReturnsUniversity()
    {
        // Arrange
        var universityName = "Universidad Específica";
        var expectedUniversity = new University(new EntityName(universityName));
        
        _universityRepositoryMock.Setup(r => r.GetByNameAsync(universityName))
            .ReturnsAsync(expectedUniversity);

        // Act
        var result = await _universityServices.GetByNameAsync(universityName);

        // Assert
        result.Should().BeEquivalentTo(expectedUniversity);
        _universityRepositoryMock.Verify(r => r.GetByNameAsync(universityName), Times.Once);
    }
}
