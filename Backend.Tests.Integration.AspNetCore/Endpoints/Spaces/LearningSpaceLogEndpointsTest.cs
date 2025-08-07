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
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.Spaces;

/// <summary>
/// Integration tests for LearningSpaceLog endpoints.
/// </summary>
public class LearningSpaceLogEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LearningSpaceLogEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Verifica que el endpoint GET /list-learning-space-logs retorna la lista esperada de logs cuando existen logs.
    /// </summary>
    [Fact]
    public async Task GivenLogsExist_WhenGetLearningSpaceLogs_ReturnsExpectedLogList()
    {
        // Arrange
        var logs = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpaceLog>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpaceLog
            {
                LearningSpaceLogInternalId = 1,
                Name = "Aula Magna",
                Type = "Auditorium",
                MaxCapacity = 100,
                Width = 10.5m,
                Height = 4.2m,
                Length = 15.0m,
                ColorFloor = "Red",
                ColorWalls = "White",
                ColorCeiling = "Gray",
                ModifiedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                Action = "Created"
            },
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpaceLog
            {
                LearningSpaceLogInternalId = 2,
                Name = "Lab 202",
                Type = "Laboratory",
                MaxCapacity = 25,
                Width = 7.5m,
                Height = 3.5m,
                Length = 9.0m,
                ColorFloor = "Blue",
                ColorWalls = "Gray",
                ColorCeiling = "White",
                ModifiedAt = new DateTime(2024, 1, 2, 14, 0, 0, DateTimeKind.Utc),
                Action = "Updated"
            }
        };
        var expectedResponse = new List<LearningSpaceLogDto>
        {
            new LearningSpaceLogDto(1, "Aula Magna", 100, "Auditorium", 10.5m, 4.2m, 15.0m, "Red", "White", "Gray", new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc), "Created"),
            new LearningSpaceLogDto(2, "Lab 202", 25, "Laboratory", 7.5m, 3.5m, 9.0m, "Blue", "Gray", "White", new DateTime(2024, 1, 2, 14, 0, 0, DateTimeKind.Utc), "Updated")
        };

        var learningSpaceLogServicesMock = new Mock<ILearningSpaceLogServices>();
        learningSpaceLogServicesMock.Setup(s => s.ListLearningSpaceLogsAsync())
            .ReturnsAsync(logs);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceLogServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceLogServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Audit", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetLearningSpaceLogResponse>("/list-learning-space-logs");

        // Assert
        response.Logs.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }
}
