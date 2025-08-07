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

public class AreaEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AreaEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // ==============================
    // POST /add-area
    // ==============================
    /// <summary>
    /// Verifica que el endpoint POST /add-area retorna la respuesta esperada cuando la petición es válida.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPostArea_ReturnsSuccessResponse()
    {
        // Arrange
        var areaDto = new AddAreaDto(
            "Finca 1",
            new AddAreaCampusDto("Campus Central")
        );
        var request = new PostAreaRequest(areaDto);
        var expectedResponse = new PostAreaResponse(areaDto);

        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.AddAreaAsync(It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area>()))
            .ReturnsAsync(true);

        var campusServicesMock = new Mock<ICampusServices>();
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        campusServicesMock.Setup(s => s.GetByNameAsync("Campus Central"))
            .ReturnsAsync(campus);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var areaDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (areaDescriptor != null)
                    services.Remove(areaDescriptor);
                services.AddScoped(_ => areaServicesMock.Object);

                var campusDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (campusDescriptor != null)
                    services.Remove(campusDescriptor);
                services.AddScoped(_ => campusServicesMock.Object);

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Area", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PostAsJsonAsync("/add-area", areaDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostAreaResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /list-areas
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-areas retorna la lista esperada cuando existen áreas.
    /// </summary>
    [Fact]
    public async Task GivenAreasExist_WhenGetAreaList_ReturnsExpectedResponse()
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

        var areas = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 1"),
                campus
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Finca 2"),
                campus
            )
        };
        var expectedResponse = new GetAreaResponse(new List<ListAreaDto>
        {
            new ListAreaDto("Finca 1", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica"))),
            new ListAreaDto("Finca 2", new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")))
        });

        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(areas);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Areas", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetAreaResponse>("/list-areas");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /list-areas/{areaName}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-areas/{areaName} retorna la respuesta esperada para un nombre válido.
    /// </summary>
    [Fact]
    public async Task GivenValidAreaName_WhenGetAreaByName_ReturnsExpectedResponse()
    {
        // Arrange
        var areaName = "Finca 1";
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
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName(areaName),
            campus
        );
        var expectedResponse = new GetAreaByNameResponse(new ListAreaDto(
            areaName,
            new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica"))
        ));

        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.GetByNameAsync(areaName))
            .ReturnsAsync(area);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific Area", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetAreaByNameResponse>($"/list-areas/{areaName}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /list-areas/{areaName} (NotFound)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-areas/{areaName} retorna NotFound cuando el nombre no existe.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentAreaName_WhenGetAreaByName_ReturnsNotFound()
    {
        // Arrange
        var areaName = "Área Inexistente";

        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.GetByNameAsync(areaName))
            .ReturnsAsync((UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Area?)null);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific Area", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetAsync($"/list-areas/{areaName}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    // ==============================
    // DELETE /area/{areaName}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint DELETE /area/{areaName} retorna Ok cuando la eliminación es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidAreaName_WhenDeleteArea_ReturnsOk()
    {
        // Arrange
        var areaName = "Finca 1";
        var areaServicesMock = new Mock<IAreaServices>();
        areaServicesMock.Setup(s => s.DeleteAreaAsync(areaName))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAreaServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => areaServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Delete Areas", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.DeleteAsync($"/area/{areaName}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
