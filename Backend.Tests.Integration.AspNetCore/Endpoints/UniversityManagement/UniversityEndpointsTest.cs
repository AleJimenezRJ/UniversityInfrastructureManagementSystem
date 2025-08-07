using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.UniversityManagement;

/// <summary>
/// Provides integration tests for university-related API endpoints.
/// </summary>
/// <remarks>This class uses the <see cref="WebApplicationFactory{TEntryPoint}"/> to create a test server for
/// executing HTTP requests against the university API endpoints. It includes tests for adding a university, listing
/// universities, and retrieving a university by name.</remarks>
public class UniversityEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UniversityEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that a valid university creation request returns a successful response.
    /// </summary>
    /// <remarks>This test verifies that when a valid university DTO is posted to the API, the response
    /// indicates success and matches the expected result. It uses a mocked university service to simulate the addition
    /// of a university and checks the response for correctness.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidRequest_WhenPostUniversity_ReturnsSuccessResponse()
    {
        // Arrange
        var universityDto = new UniversityDto("Universidad de Costa Rica", "Costa Rica");
        var expectedResponse = new PostUniversityResponse(universityDto);

        var universityServicesMock = new Mock<IUniversityServices>();
        universityServicesMock.Setup(s => s.AddUniversityAsync(It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University>()))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUniversityServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => universityServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Universities", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PostAsJsonAsync("/add-university", universityDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostUniversityResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    /// <summary>
    /// Tests that the university list endpoint returns the expected response when universities exist.
    /// </summary>
    /// <remarks>This test verifies that the API correctly retrieves a list of universities and returns it in
    /// the expected format. It sets up a mock service to simulate existing universities and checks that the response
    /// matches the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GivenUniversitiesExist_WhenGetUniversityList_ReturnsExpectedResponse()
    {
        // Arrange
        var universities = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Universidad de Costa Rica"),
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Instituto Tecnológico de Costa Rica"),
                new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
            )
        };
        var expectedResponse = new GetUniversityResponse(new List<UniversityDto>
        {
            new UniversityDto("Universidad de Costa Rica", "Costa Rica"),
            new UniversityDto("Instituto Tecnológico de Costa Rica", "Costa Rica")
        });

        var universityServicesMock = new Mock<IUniversityServices>();
        universityServicesMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(universities);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUniversityServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => universityServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Universities", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetUniversityResponse>("/list-university");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Tests that the API returns the expected response when a valid university name is provided.
    /// </summary>
    /// <remarks>This test verifies that the service correctly retrieves a university by its name and returns
    /// the expected data. It uses a mock implementation of <see cref="IUniversityServices"/> to simulate the service
    /// behavior.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidUniversityName_WhenGetUniversityByName_ReturnsExpectedResponse()
    {
        // Arrange
        var universityName = "Universidad de Costa Rica";
        var university = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University(
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName(universityName),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityLocation("Costa Rica")
        );
        var expectedResponse = new GetUniversityByNameResponse(new UniversityDto(universityName, "Costa Rica"));

        var universityServicesMock = new Mock<IUniversityServices>();
        universityServicesMock.Setup(s => s.GetByNameAsync(universityName))
            .ReturnsAsync(university);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUniversityServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => universityServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific University", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetUniversityByNameResponse>($"/list-university/{universityName}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    /// <summary>
    /// Tests that the API returns a NotFound status code when attempting to retrieve a university by a name that does
    /// not exist.
    /// </summary>
    /// <remarks>This test verifies that the endpoint correctly handles requests for non-existent universities
    /// by returning a 404 Not Found status. It uses a mocked implementation of <see cref="IUniversityServices"/> to
    /// simulate the behavior of the service layer.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GivenNonExistentUniversityName_WhenGetUniversityByName_ReturnsNotFound()
    {
        // Arrange
        var universityName = "Universidad Inexistente";

        var universityServicesMock = new Mock<IUniversityServices>();
        universityServicesMock.Setup(s => s.GetByNameAsync(universityName))
            .ReturnsAsync((UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement.University?)null);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUniversityServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => universityServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Specific University", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetAsync($"/list-university/{universityName}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
