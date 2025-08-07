using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Helpers;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.ComponentsManagement;

public class GetSingleLearningComponentByIdEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Factory for creating a test client for the ThemePark application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSingleLearningComponentByIdEndpointTests"/> class.
    /// </summary>
    /// <param name="factory"></param>
    public GetSingleLearningComponentByIdEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that the GetSingleLearningComponentById endpoint returns the expected component when a valid ID is provided.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidComponentId_WhenGet_ReturnsExpectedComponent()
    {
        // Arrange
        const int componentId = 1;

        var projector = new Projector(
            projectedContent: "Physics Slide",
            projectionArea: Area2D.Create(2, 3),
            orientation: Orientation.Create("East"),
            position: Coordinates.Create(2, 4, 1),
            dimensions: Dimension.Create(1, 1.5, 0.5)
        );

        var expectedDto = new ProjectorDto(
            Id: projector.ComponentId,
            Orientation: projector.Orientation.ToString(),
            Position: new PositionDto(projector.Position.X, projector.Position.Y, projector.Position.Z),
            Dimensions: new DimensionsDto(projector.Dimensions.Width, projector.Dimensions.Length, projector.Dimensions.Height),
            ProjectionArea: new ProjectionAreaDto(projector.ProjectionArea!.Length, projector.ProjectionArea.Height),
            ProjectedContent: projector.ProjectedContent!
        );

        var expectedResponse = new GetSingleLearningComponentByIdResponse(expectedDto);

        var serviceMock = new Mock<ILearningComponentServices>();
        serviceMock
            .Setup(s => s.GetSingleLearningComponentByIdAsync(componentId))
            .ReturnsAsync(projector);

        var realMapper = new GlobalMapper();

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<ILearningComponentServices>(_ => serviceMock.Object);
                services.AddScoped<GlobalMapper>(_ => realMapper);

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
        }).CreateClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        client.DefaultRequestHeaders.Add("X-Test-Permissions", "View Specific Component");

        // Act
        var response = await client.GetFromJsonAsync<GetSingleLearningComponentByIdResponse>(
            $"/learning-component/{componentId}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }
}

