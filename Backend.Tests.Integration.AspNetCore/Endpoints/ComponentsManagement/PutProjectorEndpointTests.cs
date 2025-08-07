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
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Helpers;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.ComponentsManagement;

/// <summary>
/// Tests for the PutProjector endpoint in the ThemePark application.
/// </summary>
public class PutProjectorEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{

    /// <summary>
    /// Factory for creating a test client for the ThemePark application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PutProjectorEndpointTests"/> class.
    /// </summary>
    /// <param name="factory"></param>
    public PutProjectorEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that the PutProjector endpoint returns an OK response with the expected projector DTO when a valid projector is updated.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidProjector_WhenPut_ReturnsOkWithResponse()
    {
        // Arrange
        const int learningSpaceId = 1;
        const int learningComponentId = 42;

        var dto = new ProjectorNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1.0, 2.0, 0.0),
            Dimensions: new DimensionsDto(2.0, 1.5, 0.5),
            ProjectionArea: new ProjectionAreaDto(2, 2),
            ProjectedContent: "Updated Content"
        );

        var projector = new Projector(
            projectedContent: dto.ProjectedContent!,
            projectionArea: Area2D.Create(dto.ProjectionArea.ProjectedHeight, dto.ProjectionArea.ProjectedWidth),
            orientation: Orientation.Create(dto.Orientation),
            position: Coordinates.Create(dto.Position.X, dto.Position.Y, dto.Position.Z),
            dimensions: Dimension.Create(dto.Dimensions.Width, dto.Dimensions.Length, dto.Dimensions.Height)
        );

        var serviceMock = new Mock<IProjectorServices>();
        serviceMock
            .Setup(s => s.UpdateProjectorAsync(learningSpaceId, learningComponentId, It.IsAny<Projector>()))
            .ReturnsAsync(true);

        var realMapper = new GlobalMapper();

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IProjectorServices>(_ => serviceMock.Object);
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
        client.DefaultRequestHeaders.Add("X-Test-Permissions", "Edit Components");

        var request = new PutProjectorRequest(dto);

        // Act
        var response = await client.PutAsJsonAsync($"/learning-spaces/{learningSpaceId}/learning-component/projector/{learningComponentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PutProjectorResponse>();
        body.Should().BeEquivalentTo(new PutProjectorResponse(dto));
    }
}
