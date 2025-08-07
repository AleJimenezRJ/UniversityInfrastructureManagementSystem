using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using Microsoft.AspNetCore.Authentication;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Helpers;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.ComponentsManagement;

/// <summary>
/// Tests for the GetLearningComponent endpoint in the ThemePark application.
/// </summary>
public class GetLearningComponentEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Factory for creating a test client for the ThemePark application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningComponentEndpointTests"/> class.
    /// </summary>
    /// <param name="factory"></param>
    public GetLearningComponentEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// tests that the GetLearningComponentsById endpoint returns a paginated list of learning components, including a projector.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenExistingProjector_WhenGetById_ReturnsExpectedProjectorDto()
    {
        // Arrange
        const int learningSpaceId = 1;
        const int pageSize = 10;
        const int pageIndex = 0;

        var projector = new Projector(
            projectedContent: "Math Presentation",
            projectionArea: Area2D.Create(2, 2),
            orientation: Orientation.Create("North"),
            position: Coordinates.Create(1.0, 2.0, 0.0),
            dimensions: Dimension.Create(2.0, 1.5, 0.5)
        );

        var paginated = new PaginatedList<LearningComponent>(
            items: new List<LearningComponent> { projector },
            totalCount: 1,
            pageIndex: pageIndex,
            pageSize: pageSize
        );

        var expectedDto = new ProjectorDto(
            Id: projector.ComponentId,
            Orientation: projector.Orientation.ToString(),
            Position: new PositionDto(projector.Position.X, projector.Position.Y, projector.Position.Z),
            Dimensions: new DimensionsDto(projector.Dimensions.Width, projector.Dimensions.Length, projector.Dimensions.Height),
            ProjectionArea: new ProjectionAreaDto(projector.ProjectionArea!.Length, projector.ProjectionArea.Height),
            ProjectedContent: projector.ProjectedContent!
        );

        var expectedResponse = new GetLearningComponentsByIdResponse(
            LearningComponents: new List<LearningComponentDto> { expectedDto },
            PageSize: pageSize,
            PageIndex: pageIndex,
            TotalCount: 1,
            TotalPages: 1
        );

        var serviceMock = new Mock<ILearningComponentServices>();
        serviceMock
            .Setup(service => service.GetLearningComponentsByIdAsync(learningSpaceId, pageSize, pageIndex, ""))
            .ReturnsAsync(paginated);

        var client = _factory
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            services.AddScoped(_ => serviceMock.Object);

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
        });

        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
        });
    })
    .CreateClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        client.DefaultRequestHeaders.Add("X-Test-Permissions", "List Components");

        // Act
        var response = await client.GetFromJsonAsync<GetLearningComponentsByIdResponse>(
            $"/learning-spaces/{learningSpaceId}/learning-component?pageSize={pageSize}&pageIndex={pageIndex}"
        );

        // Assert
        response.Should().BeEquivalentTo(expectedResponse,
            because: "the API should return a paginated list including the expected projector");
    }
}
