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
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using System.Threading.Tasks;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Endpoints.Spaces;

public class LearningSpaceEndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LearningSpaceEndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // ==============================
    // POST /floors/{floorId}/learning-spaces/
    // ==============================
    /// <summary>
    /// Verifica que el endpoint POST /floors/{floorId}/learning-spaces/ retorna la respuesta esperada cuando la petición es válida.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPostLearningSpace_ReturnsSuccessResponse()
    {
        // Arrange
        var floorId = 1;
        var learningSpaceDto = new LearningSpaceDto(
            Name: "Aula 101",
            Type: "Classroom",
            MaxCapacity: 30,
            Height: 3.0,
            Width: 5.0,
            Length: 7.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "White"
        );
        var request = new PostLearningSpaceRequest(learningSpaceDto);
        var expectedResponse = new PostLearningSpaceResponse(learningSpaceDto, "The learning space was created succesfully.");

        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.CreateLearningSpaceAsync(floorId, It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>()))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Create Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PostAsJsonAsync($"/floors/{floorId}/learning-spaces/", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PostLearningSpaceResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /learning-spaces/{learningSpaceId}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /learning-spaces/{learningSpaceId} retorna la respuesta esperada para un ID válido.
    /// </summary>
    [Fact]
    public async Task GivenValidLearningSpaceId_WhenGetLearningSpace_ReturnsExpectedResponse()
    {
        // Arrange
        var learningSpaceId = 10;
        var learningSpaceDto = new LearningSpaceDto(
            Name: "Aula Magna",
            Type: "Auditorium",
            MaxCapacity: 100,
            Height: 5.0,
            Width: 10.0,
            Length: 20.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "White"
        );
        var expectedResponse = new GetLearningSpaceResponse(learningSpaceDto);

        var learningSpace = new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace(
            id: learningSpaceId,
            name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Aula Magna"),
            type: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.LearningSpaceType.Create("Auditorium"),
            maxCapacity: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Capacity.Create(100),
            height: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(5.0),
            width: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(10.0),
            length: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(20.0),
            colorFloor: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Gray"),
            colorWalls: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White"),
            colorCeiling: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White")
        );

        var learningSpaceServicesMock = new Moq.Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.GetLearningSpaceAsync(learningSpaceId))
            .ReturnsAsync(learningSpace);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("View Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetLearningSpaceResponse>($"/learning-spaces/{learningSpaceId}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // GET /floors/{floorId}/learning-spaces/
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /floors/{floorId}/learning-spaces/ retorna la lista esperada cuando existen espacios de aprendizaje.
    /// </summary>
    [Fact]
    public async Task GivenLearningSpacesExist_WhenGetLearningSpaceList_ReturnsExpectedResponse()
    {
        // Arrange
        var floorId = 1;
        var learningSpaces = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace(
                id: 1,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Aula 1"),
                type: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.LearningSpaceType.Create("Classroom"),
                maxCapacity: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Capacity.Create(30),
                height: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(3.0),
                width: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(5.0),
                length: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(7.0),
                colorFloor: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Gray"),
                colorWalls: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White"),
                colorCeiling: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White")
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace(
                id: 2,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Aula 2"),
                type: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.LearningSpaceType.Create("Laboratory"),
                maxCapacity: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Capacity.Create(20),
                height: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(3.5),
                width: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(6.0),
                length: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(8.0),
                colorFloor: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Gray"),
                colorWalls: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White"),
                colorCeiling: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White")
            )
        };
        var expectedResponse = new GetLearningSpaceListResponse(new List<LearningSpaceListDto>
        {
            new LearningSpaceListDto(1, "Aula 1", "Classroom"),
            new LearningSpaceListDto(2, "Aula 2", "Laboratory")
        });

        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.GetLearningSpacesListAsync(floorId))
            .ReturnsAsync(learningSpaces);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                // Solo agrega la política requerida para este endpoint
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetLearningSpaceListResponse>($"/floors/{floorId}/learning-spaces/");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // GET /floors/{floorId}/learning-spaces/paginated
    // ==============================
    /// <summary>
    /// Verifica que el endpoint GET /floors/{floorId}/learning-spaces/paginated retorna la respuesta paginada esperada.
    /// </summary>
    [Fact]
    public async Task GivenPaginatedRequest_WhenGetLearningSpacePaginatedList_ReturnsExpectedPaginatedResponse()
    {
        // Arrange
        var floorId = 1;
        var pageSize = 2;
        var pageIndex = 0;
        var searchText = "";
        var learningSpaces = new List<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>
        {
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace(
                id: 1,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Aula 1"),
                type: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.LearningSpaceType.Create("Classroom"),
                maxCapacity: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Capacity.Create(30),
                height: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(3.0),
                width: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(5.0),
                length: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(7.0),
                colorFloor: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Gray"),
                colorWalls: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White"),
                colorCeiling: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White")
            ),
            new UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace(
                id: 2,
                name: new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.EntityName("Aula 2"),
                type: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.LearningSpaceType.Create("Laboratory"),
                maxCapacity: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Capacity.Create(20),
                height: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(3.5),
                width: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(6.0),
                length: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces.Size.Create(8.0),
                colorFloor: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("Gray"),
                colorWalls: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White"),
                colorCeiling: UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement.Colors.Create("White")
            )
        };
        var paginatedList = new UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.PaginatedList<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>(
            learningSpaces, 2, pageSize, pageIndex);
        var expectedResponse = new GetLearningSpacePaginatedListResponse(
            new[]
            {
                new LearningSpaceListDto(1, "Aula 1", "Classroom"),
                new LearningSpaceListDto(2, "Aula 2", "Laboratory")
            },
            pageSize,
            pageIndex,
            2,
            1
        );

        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.GetLearningSpacesListPaginatedAsync(floorId, pageSize, pageIndex, searchText))
            .ReturnsAsync(paginatedList);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("List Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.GetFromJsonAsync<GetLearningSpacePaginatedListResponse>($"/floors/{floorId}/learning-spaces/paginated?pageSize={pageSize}&pageIndex={pageIndex}&searchText={searchText}");

        // Assert
        response.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    // ==============================
    // PUT /learning-spaces/{learningSpaceId} (Success)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint PUT /learning-spaces/{learningSpaceId} retorna la respuesta esperada cuando la actualización es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidRequest_WhenPutLearningSpace_ReturnsSuccessResponse()
    {
        // Arrange
        var learningSpaceId = 20;
        var learningSpaceDto = new LearningSpaceDto(
            Name: "Aula 202",
            Type: "Classroom",
            MaxCapacity: 40,
            Height: 3.5,
            Width: 6.0,
            Length: 8.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "White"
        );
        var request = new PutLearningSpaceRequest(learningSpaceDto);
        var expectedResponse = new PutLearningSpaceResponse(learningSpaceDto, "The learning space was updated successfully");

        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.UpdateLearningSpaceAsync(learningSpaceId, It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>()))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Edit Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PutAsJsonAsync($"/learning-spaces/{learningSpaceId}", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PutLearningSpaceResponse>();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    // ==============================
    // PUT /learning-spaces/{learningSpaceId} (NotFound)
    // ==============================
    /// <summary>
    /// Verifica que el endpoint PUT /learning-spaces/{learningSpaceId} retorna NotFound cuando el ID no existe.
    /// </summary>
    [Fact]
    public async Task GivenNonExistentLearningSpaceId_WhenPutLearningSpace_ReturnsNotFound()
    {
        // Arrange
        var learningSpaceId = 999;
        var learningSpaceDto = new LearningSpaceDto(
            Name: "Aula Fantasma",
            Type: "Classroom",
            MaxCapacity: 10,
            Height: 2.5,
            Width: 4.0,
            Length: 6.0,
            ColorFloor: "Gray",
            ColorWalls: "White",
            ColorCeiling: "White"
        );
        var request = new PutLearningSpaceRequest(learningSpaceDto);

        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.UpdateLearningSpaceAsync(learningSpaceId, It.IsAny<UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces.LearningSpace>()))
            .ThrowsAsync(new UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions.NotFoundException("Learning space not found."));

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Edit Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.PutAsJsonAsync($"/learning-spaces/{learningSpaceId}", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    // ==============================
    // DELETE /learning-spaces/{learningSpaceId}
    // ==============================
    /// <summary>
    /// Verifica que el endpoint DELETE /learning-spaces/{learningSpaceId} retorna NoContent cuando la eliminación es exitosa.
    /// </summary>
    [Fact]
    public async Task GivenValidLearningSpaceId_WhenDeleteLearningSpace_ReturnsNoContent()
    {
        // Arrange
        var learningSpaceId = 5;
        var learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        learningSpaceServicesMock.Setup(s => s.DeleteLearningSpaceAsync(learningSpaceId))
            .ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILearningSpaceServices));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddScoped(_ => learningSpaceServicesMock.Object);
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Delete Learning Space", policy => policy.RequireAssertion(_ => true));
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
        var response = await client.DeleteAsync($"/learning-spaces/{learningSpaceId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
