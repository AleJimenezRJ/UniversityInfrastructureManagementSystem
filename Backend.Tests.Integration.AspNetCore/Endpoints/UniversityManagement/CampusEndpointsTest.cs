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

public class CampusEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CampusEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // ==============================
    // POST /add-campus
    // ==============================
    /// <summary>
    /// Verifica que el endpoint POST /add-campus retorna la respuesta esperada cuando la petición es válida.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPostCampus_ReturnsSuccessResponse()
    {
        // Arrange
        var campusDto = new AddCampusDto(
            "Campus Central",
            "San Pedro",
            new AddCampusUniversityDto("Universidad de Costa Rica")
        );
        var request = new PostCampusRequest(campusDto);
        var expectedResponse = new PostCampusResponse(campusDto);

        var campusServicesMock = new Mock<ICampusServices>();
        campusServicesMock.Setup(s => s.AddCampusAsync(It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus>()))
            .ReturnsAsync(true);

        var universityServicesMock = new Mock<IUniversityServices>();
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        universityServicesMock.Setup(s => s.GetByNameAsync("Universidad de Costa Rica"))
            .ReturnsAsync(university);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var campusDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (campusDescriptor != null)
                    services.Remove(campusDescriptor);
                services.AddScoped(_ => campusServicesMock.Object);

                var universityDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUniversityServices));
                if (universityDescriptor != null)
                    services.Remove(universityDescriptor);
                services.AddScoped(_ => universityServicesMock.Object);

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Campus", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PostAsJsonAsync("/add-campus", campusDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostCampusResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /list-campus
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-campus retorna la lista esperada cuando existen campus.
    /// </summary>
    [Fact]
    public async Task GivenCampusesExist_WhenGetCampusList_ReturnsExpectedResponse()
    {
        // Arrange
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );

        var campuses = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Central"),
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
                university
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Campus Sur"),
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Pérez Zeledón"),
                university
            )
        };
        var expectedResponse = new GetCampusResponse(new List<ListCampusDto>
        {
            new ListCampusDto("Campus Central", "San Pedro", new UniversityDto("Universidad de Costa Rica", "Costa Rica")),
            new ListCampusDto("Campus Sur", "Pérez Zeledón", new UniversityDto("Universidad de Costa Rica", "Costa Rica"))
        });

        var campusServicesMock = new Mock<ICampusServices>();
        campusServicesMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(campuses);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => campusServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Campus", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetCampusResponse>("/list-campus");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /list-campus/{campusName}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-campus/{campusName} retorna la respuesta esperada para un nombre válido.
    /// </summary>
    [Fact]
    public async Task GivenValidCampusName_WhenGetCampusByName_ReturnsExpectedResponse()
    {
        // Arrange
        var campusName = "Campus Central";
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var campus = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName(campusName),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("San Pedro"),
            university
        );
        var expectedResponse = new GetCampusByNameResponse(new ListCampusDto(
            campusName,
            "San Pedro",
            new UniversityDto("Universidad de Costa Rica", "Costa Rica")
        ));

        var campusServicesMock = new Mock<ICampusServices>();
        campusServicesMock.Setup(s => s.GetByNameAsync(campusName))
            .ReturnsAsync(campus);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => campusServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific Campus", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetCampusByNameResponse>($"/list-campus/{campusName}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /list-campus/{campusName} (NotFound)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /list-campus/{campusName} retorna NotFound cuando el nombre no existe.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentCampusName_WhenGetCampusByName_ReturnsNotFound()
    {
        // Arrange
        var campusName = "Campus Inexistente";

        var campusServicesMock = new Mock<ICampusServices>();
        campusServicesMock.Setup(s => s.GetByNameAsync(campusName))
            .ReturnsAsync((UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.Campus?)null);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => campusServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific Campus", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetAsync($"/list-campus/{campusName}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    // ==============================
    // DELETE /delete-campus/{campusName}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint DELETE /delete-campus/{campusName} retorna Ok cuando la eliminación es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidCampusName_WhenDeleteCampus_ReturnsOk()
    {
        // Arrange
        var campusName = "Campus Central";
        var campusServicesMock = new Mock<ICampusServices>();
        campusServicesMock.Setup(s => s.DeleteCampusAsync(campusName))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICampusServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => campusServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Delete Campus", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.DeleteAsync($"/delete-campus/{campusName}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
