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
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using System.Threading.Tasks;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.Spaces;

/// <summary>
/// Integration tests for the Floor endpoints in the ASP.NET Core presentation layer.
/// </summary>
public class FloorEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FloorEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // ==============================
    // GET /buildings/{buildingId}/floors
    // ==============================
    /// <summary>
    /// Verifies that when floors exist, the GET /buildings/{buildingId}/floors endpoint returns the expected list of floors.
    /// </summary>
    [Fact]
    public async Task GivenFloorsExist_WhenGetFloors_ReturnsExpectedResponse()
    {
        var expectedFloors = new List<FloorDto>
        {
            new FloorDto(1, 1),
            new FloorDto(2, 2)
        };
        var expectedResponse = new GetFloorListResponse(expectedFloors);

        var floorServicesMock = new Mock<IFloorServices>();
        floorServicesMock.Setup(s => s.GetFloorsListAsync(It.IsAny<int>())).ReturnsAsync(new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1)),
            new Floor(2, FloorNumber.Create(2))
        });

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFloorServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => floorServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Floors", policy => policy.RequireAssertion(_ => true));
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

        var response = await client.GetFromJsonAsync<GetFloorListResponse>("/buildings/1/floors");

        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /buildings/{buildingId}/floors 
    // ==============================
    /// <summary>
    /// Verifies that when no floors exist, the GET /buildings/{buildingId}/floors endpoint returns an empty list.
    /// </summary>
    [Fact]
    public async Task GivenNoFloorsExist_WhenGetFloors_ReturnsEmptyList()
    {
        var expectedResponse = new GetFloorListResponse(new List<FloorDto>());

        var floorServicesMock = new Mock<IFloorServices>();
        floorServicesMock.Setup(s => s.GetFloorsListAsync(It.IsAny<int>())).ReturnsAsync(new List<Floor>());

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFloorServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => floorServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Floors", policy => policy.RequireAssertion(_ => true));
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

        var response = await client.GetFromJsonAsync<GetFloorListResponse>("/buildings/1/floors");

        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /buildings/{buildingId}/floors/paginated
    // ==============================
    /// <summary>
    /// Verifies that when a paginated request is made, the GET /buildings/{buildingId}/floors/paginated endpoint returns the expected paginated response.
    /// </summary>
    [Fact]
    public async Task GivenPaginatedRequest_WhenGetFloorPaginatedList_ReturnsExpectedPaginatedResponse()
    {
        // Arrange
        var buildingId = 1;
        var pageSize = 2;
        var pageIndex = 0;
        var floors = new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1)),
            new Floor(2, FloorNumber.Create(2))
        };
        var paginatedList = new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.PaginatedList<Floor>(
            floors, 2, pageSize, pageIndex);
        var expectedResponse = new GetFloorPaginatedListResponse(
            new[]
            {
                new FloorDto(1, 1),
                new FloorDto(2, 2)
            },
            pageSize,
            pageIndex,
            2,
            1
        );

        var floorServicesMock = new Mock<IFloorServices>();
        floorServicesMock.Setup(s => s.GetFloorsListPaginatedAsync(buildingId, pageSize, pageIndex))
            .ReturnsAsync(paginatedList);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFloorServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => floorServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Floors", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetFloorPaginatedListResponse>($"/buildings/{buildingId}/floors/paginated?pageSize={pageSize}&pageIndex={pageIndex}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // DELETE /floors/{floorId}
    // ==============================
    /// <summary>
    /// Verifies that when a valid floor ID is provided, the DELETE /floors/{floorId} endpoint successfully deletes the floor.
    /// </summary>
    [Fact]
    public async Task GivenValidFloorId_WhenDeleteFloor_ReturnsNoContent()
    {
        // Arrange
        var floorId = 3;
        var floorServicesMock = new Mock<IFloorServices>();
        floorServicesMock.Setup(s => s.DeleteFloorAsync(floorId))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFloorServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => floorServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Delete Floors", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.DeleteAsync($"/floors/{floorId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    // ==============================
    // POST /buildings/{buildingId}/floors
    // ==============================
    /// <summary>
    /// Verifies that when a valid building ID is provided, the POST /buildings/{buildingId}/floors endpoint successfully creates a floor.
    /// </summary>
    [Fact]
    public async Task GivenValidBuildingId_WhenPostFloor_ReturnsSuccessResponse()
    {
        // Arrange
        var buildingId = 1;
        var expectedResponse = new PostFloorResponse("The floor was created successfully.");

        var floorServicesMock = new Mock<IFloorServices>();
        floorServicesMock.Setup(s => s.CreateFloorAsync(buildingId))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFloorServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => floorServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Floors", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PostAsync($"/buildings/{buildingId}/floors", null);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostFloorResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
