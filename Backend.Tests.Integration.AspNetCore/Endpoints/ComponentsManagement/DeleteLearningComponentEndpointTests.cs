using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Helpers;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.ComponentsManagement;

/// <summary>
/// Tests for the DeleteLearningComponent endpoint in the ThemePark application.
/// </summary>
public class DeleteLearningComponentEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Factory for creating a test client for the ThemePark application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLearningComponentEndpointTests"/> class.
    /// </summary>
    /// <param name="factory"></param>
    public DeleteLearningComponentEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that a valid ID returns a NoContent response when deleting a learning component.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenValidId_WhenDelete_ReturnsNoContent()
    {
        // Arrange
        var componentId = 1;

        var serviceMock = new Mock<ILearningComponentServices>();
        serviceMock
            .Setup(s => s.DeleteLearningComponentAsync(componentId))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
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
        }).CreateClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        client.DefaultRequestHeaders.Add("X-Test-Permissions", "Delete Components");


        // Act
        var response = await client.DeleteAsync($"/learning-component/{componentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Tests that an invalid ID returns a NotFound response with a specific error message when deleting a learning component.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenInvalidId_WhenDelete_ReturnsNotFoundWithMessage()
    {
        // Arrange
        var componentId = 999;

        var serviceMock = new Mock<ILearningComponentServices>();
        serviceMock
            .Setup(s => s.DeleteLearningComponentAsync(componentId))
            .ReturnsAsync(false);

        var client = _factory.WithWebHostBuilder(builder =>
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
        }).CreateClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        client.DefaultRequestHeaders.Add("X-Test-Permissions", "Delete Components");

        // Act
        var httpResponse = await client.DeleteAsync($"/learning-component/{componentId}");

        // Assert: Status code should be 404
        httpResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var response = await httpResponse.Content.ReadFromJsonAsync<DeleteLearningComponentResponse>();
        response.Should().BeEquivalentTo(new DeleteLearningComponentResponse(
            $"Error deleting whiteboard with id {componentId}. Please check if the id is correct."));
    }

}
