using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using System.Threading.Tasks;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.UniversityManagement;

public class BuildingEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BuildingEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // ==============================
    // POST /add-building
    // ==============================
    /// <summary>
    /// Verifica que el endpoint POST /add-building retorna la respuesta esperada cuando la petición es válida.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPostBuilding_ReturnsSuccessResponse()
    {
        // Arrange
        var buildingDto = new AddBuildingDto(
            "Edificio Principal",
            10.0,
            -84.0,
            0.0,
            50.0,
            100.0,
            20.0,
            "Red",
            new AddBuildingAreaDto("Finca 1")
        );
        var request = new PostBuildingRequest(buildingDto);
        var expectedResponse = new PostBuildingResponse(buildingDto);

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.AddBuildingAsync(It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building>()))
            .ReturnsAsync(true);

        var areaServicesMock = new Mock<IAreaServices>();
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var area = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
            campus
        );
        areaServicesMock.Setup(s => s.GetByNameAsync("Finca 1"))
            .ReturnsAsync(area);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var buildingDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (buildingDescriptor != null)
                    services.Remove(buildingDescriptor);
                services.AddScoped(_ => buildingServicesMock.Object);

                var areaDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (areaDescriptor != null)
                    services.Remove(areaDescriptor);
                services.AddScoped(_ => areaServicesMock.Object);

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/add-building", buildingDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostBuildingResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /list-buildings
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-buildings retorna la lista esperada cuando existen edificios.
    /// </summary>
    [Fact]
    public async Task GivenBuildingsExist_WhenGetBuildingList_ReturnsExpectedResponse()
    {
        // Arrange
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var area = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
            campus
        );

        var buildings = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
                buildingInternalId: 1,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Principal"),
                coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(10.0, -84.0, 0.0),
                dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(50, 100, 20),
                color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Red"),
                area: area
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
                buildingInternalId: 2,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Anexo"),
                coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(11.0, -84.1, 0.0),
                dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(40, 80, 15),
                color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Blue"),
                area: area
            )
        };
        var expectedResponse = new GetBuildingResponse(new List<ListBuildingDto>
        {
            new ListBuildingDto(1, "Edificio Principal", 10.0, -84.0, 0.0, 50.0, 100.0, 20.0, "Red", new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")))),
            new ListBuildingDto(2, "Edificio Anexo", 11.0, -84.1, 0.0, 40.0, 80.0, 15.0, "Blue", new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica"))))
        });

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.ListBuildingAsync())
            .ReturnsAsync(buildings);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<GetBuildingResponse>("/list-buildings");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /list-building/{buildingId}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-building/{buildingId} retorna la respuesta esperada para un ID válido.
    /// </summary>
    [Fact]
    public async Task GivenValidBuildingId_WhenGetBuildingById_ReturnsExpectedResponse()
    {
        // Arrange
        var buildingId = 1;
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var area = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
            campus
        );
        var building = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
            buildingInternalId: buildingId,
            name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Principal"),
            coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(10.0, -84.0, 0.0),
            dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(50, 100, 20),
            color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Red"),
            area: area
        );
        var expectedResponse = new GetBuildingByNameResponse(new ListBuildingDto(
            buildingId,
            "Edificio Principal",
            10.0,
            -84.0,
            0.0,
            50.0,
            100.0,
            20.0,
            "Red",
            new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")))
        ));

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.DisplayBuildingAsync(buildingId))
            .ReturnsAsync(building);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific Building", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<GetBuildingByNameResponse>($"/list-building/{buildingId}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET list-paginated-buildings
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET list-paginated-buildings retorna la respuesta paginada esperada.
    /// </summary>
    [Fact]
    public async Task GivenPaginatedRequest_WhenGetBuildingPaginatedList_ReturnsExpectedPaginatedResponse()
    {
        // Arrange
        var pageSize = 2;
        var pageIndex = 0;
        var searchText = "";
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var area = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
            campus
        );
        var buildings = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
                buildingInternalId: 1,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Principal"),
                coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(10.0, -84.0, 0.0),
                dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(50, 100, 20),
                color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Red"),
                area: area
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
                buildingInternalId: 2,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Anexo"),
                coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(11.0, -84.1, 0.0),
                dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(40, 80, 15),
                color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Blue"),
                area: area
            )
        };
        var paginatedList = new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.PaginatedList<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building>(
            buildings, 2, pageSize, pageIndex);
        var expectedResponse = new GetBuildingPaginatedListResponse(
            new[]
            {
                new ListBuildingDto(1, "Edificio Principal", 10.0, -84.0, 0.0, 50.0, 100.0, 20.0, "Red", new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")))),
                new ListBuildingDto(2, "Edificio Anexo", 11.0, -84.1, 0.0, 40.0, 80.0, 15.0, "Blue", new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica"))))
            },
            pageSize,
            pageIndex,
            2,
            1
        );

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.GetBuildingsListPaginatedAsync(pageSize, pageIndex, searchText))
            .ReturnsAsync(paginatedList);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<GetBuildingPaginatedListResponse>($"list-paginated-buildings?pageSize={pageSize}&pageIndex={pageIndex}&searchText={searchText}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // PUT /update-building/{buildingId} (Success)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint PUT /update-building/{buildingId} retorna la respuesta esperada cuando la actualización es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPutBuilding_ReturnsSuccessResponse()
    {
        // Arrange
        var buildingId = 1;
        var buildingDto = new AddBuildingDto(
            "Edificio Actualizado",
            10.5,
            -84.5,
            0.5,
            55.0,
            105.0,
            25.0,
            "Green",
            new AddBuildingAreaDto("Finca 1")
        );
        var request = new PutBuildingRequest(new ListBuildingDto(
            buildingId,
            "Edificio Actualizado",
            10.5,
            -84.5,
            0.5,
            55.0,
            105.0,
            25.0,
            "Green",
            new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")))
        ));
        var expectedResponse = new PutBuildingResponse(buildingDto);

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.UpdateBuildingAsync(It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building>(), buildingId))
            .ReturnsAsync(true);
        
        // Mock DisplayBuildingAsync to return an existing building
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var area = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
            campus
        );
        var existingBuilding = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building(
            buildingInternalId: buildingId,
            name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Edificio Original"),
            coordinates: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Coordinates.Create(10.0, -84.0, 0.0),
            dimensions: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Dimension.Create(50, 100, 20),
            color: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Red"),
            area: area
        );
        buildingServicesMock.Setup(s => s.DisplayBuildingAsync(buildingId))
            .ReturnsAsync(existingBuilding);

        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.GetByNameAsync("Finca 1"))
            .ReturnsAsync(area);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                
                var areaDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (areaDescriptor != null)
                    services.Remove(areaDescriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Edit Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.PutAsJsonAsync($"/update-building/{buildingId}", buildingDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PutBuildingResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // PUT /update-building/{buildingId} (NotFound)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint PUT /update-building/{buildingId} retorna BadRequest cuando el ID no existe.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentBuildingId_WhenPutBuilding_ReturnsBadRequest()
    {
        // Arrange
        var buildingId = 999;
        var buildingDto = new AddBuildingDto(
            "Edificio Fantasma",
            0.0,
            0.0,
            0.0,
            10.0,
            10.0,
            10.0,
            "Gray",
            new AddBuildingAreaDto("Finca 1")
        );

        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.DisplayBuildingAsync(buildingId))
            .ReturnsAsync((UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Building?)null);

        var areaServicesMock = new Mock<IAreaServices>();

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                
                var areaDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (areaDescriptor != null)
                    services.Remove(areaDescriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Edit Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.PutAsJsonAsync($"/update-building/{buildingId}", buildingDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    // ==============================
    // DELETE /delete-building/{buildingId}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint DELETE /delete-building/{buildingId} retorna Ok cuando la eliminación es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidBuildingId_WhenDeleteBuilding_ReturnsOk()
    {
        // Arrange
        var buildingId = 1;
        var buildingServicesMock = new Mock<IBuildingsServices>();
        buildingServicesMock.Setup(s => s.DeleteBuildingAsync(buildingId))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IBuildingsServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => buildingServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Delete Buildings", policy => policy.RequireAssertion(_ => true));
                });
            });
            builder.ConfigureServices(services =>
            {
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            });
        }).CreateClient();

        // Act
        var response = await client.DeleteAsync($"/delete-building/{buildingId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
