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
/// Tests for the PutWhiteboard endpoint in the ThemePark application.
/// </summary>
public class PutWhiteboardEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{

    /// <summary>
    /// Factory for creating a test client for the ThemePark application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PutWhiteboardEndpointTests"/> class.
    /// </summary>
    /// <param name="factory"></param>
    public PutWhiteboardEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that the PutWhiteboard endpoint returns an OK response with the expected whiteboard DTO when a valid whiteboard is updated.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidWhiteboard_WhenPut_ReturnsOkWithResponse()
    {
        // Arrange
        const int learningSpaceId = 1;
        const int learningComponentId = 100;

        var dto = new WhiteboardNoIdDto(
            Orientation: "East",
            Position: new PositionDto(1.0, 1.0, 0.0),
            Dimensions: new DimensionsDto(3.0, 1.5, 0.1),
            MarkerColor: "Blue"
        );

        var whiteboard = new Whiteboard(
            markerColor: Colors.Create(dto.MarkerColor),
            orientation: Orientation.Create(dto.Orientation),
            position: Coordinates.Create(dto.Position.X, dto.Position.Y, dto.Position.Z),
            dimensions: Dimension.Create(dto.Dimensions.Width, dto.Dimensions.Length, dto.Dimensions.Height)
        );

        var serviceMock = new Mock<IWhiteboardServices>();
        serviceMock
            .Setup(s => s.UpdateWhiteboardAsync(learningSpaceId, learningComponentId, It.IsAny<Whiteboard>()))
            .ReturnsAsync(true);

        var realMapper = new GlobalMapper();

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IWhiteboardServices>(_ => serviceMock.Object);
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

        var request = new PutWhiteboardRequest(dto);

        // Act
        var response = await client.PutAsJsonAsync(
            $"/learning-spaces/{learningSpaceId}/learning-component/whiteboard/{learningComponentId}",
            request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PutWhiteboardResponse>();
        body.Should().BeEquivalentTo(new PutWhiteboardResponse(dto));
    }
}
