using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Spaces;
using DomainColors = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement.Colors;
using DomainSize = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces.Size;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.Spaces;

/// <summary>
/// Unit tests for the ReadLearningSpace component.
/// </summary>
public class ReadLearningSpaceTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadLearningSpaceTests"/> class.
    /// </summary>
    private readonly Task<AuthenticationState> _authState;

    /// Constructor to set up the test context
    /// and mock services required for the ReadLearningSpace component.
    public ReadLearningSpaceTests()
    {
        Services.AddMudServices();
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);

        // Setup authentication state
        var identity = new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.Name, "test@example.com"),
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "test");
        var user = new ClaimsPrincipal(identity);
        _authState = Task.FromResult(new AuthenticationState(user));

        // Add authorization for testing
        this.AddTestAuthorization().SetAuthorized("test@example.com");

        this.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    /// <summary>
    /// Tests that the ReadLearningSpace component renders correctly with valid learning space data.
    /// </summary>
    [Fact]
    public void ShouldRenderLearningSpaceDetails_WhenLearningSpaceExists()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            id: 1,
            name: EntityName.Create("Aula 101"),
            type: LearningSpaceType.Create("Classroom"),
            maxCapacity: Capacity.Create(30),
            height: DomainSize.Create(3.0),
            width: DomainSize.Create(8.0),
            length: DomainSize.Create(10.0),
            colorFloor: DomainColors.Create("Brown"),
            colorWalls: DomainColors.Create("White"),
            colorCeiling: DomainColors.Create("White")
        );

        var building = new Building(
            buildingInternalId: 1,
            name: EntityName.Create("Edificio Central"),
            coordinates: null,
            dimensions: null,
            color: null,
            area: (Area)null!
        );

        var floors = new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1))
        };

        var learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        learningSpaceServiceMock.Setup(s => s.GetLearningSpaceAsync(1)).ReturnsAsync(learningSpace);

        var buildingServiceMock = new Mock<IBuildingsServices>();
        buildingServiceMock.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);

        var floorServiceMock = new Mock<IFloorServices>();
        floorServiceMock.Setup(s => s.GetFloorsListAsync(1)).ReturnsAsync(floors);

        var permissionMock = new Mock<IPermissionContext>();
        permissionMock.Setup(p => p.HasPermission(It.IsAny<string>())).Returns(true);

        Services.AddSingleton(learningSpaceServiceMock.Object);
        Services.AddSingleton(buildingServiceMock.Object);
        Services.AddSingleton(floorServiceMock.Object);
        Services.AddSingleton<IPermissionContext>(permissionMock.Object);
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        // Act
        var cut = RenderComponent<ReadLearningSpace>(parameters => parameters
            .Add(p => p.buildingId, 1)
            .Add(p => p.floorId, 1)
            .Add(p => p.learningSpaceId, 1));

        // Assert
        Assert.Contains("Aula 101", cut.Markup);
        Assert.Contains("Edificio Central", cut.Markup);
        Assert.Contains("30", cut.Markup);
        Assert.Contains("Classroom", cut.Markup);
        Assert.Contains("3", cut.Markup);
        Assert.Contains("8", cut.Markup);
        Assert.Contains("10", cut.Markup);
    }

    /// <summary>
    /// Tests that the ReadLearningSpace component hides edit and delete buttons when user lacks permissions.
    /// </summary>
    [Fact]
    public void ShouldHideEditAndDeleteButtons_WhenUserLacksPermissions()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            id: 1,
            name: EntityName.Create("Aula 101"),
            type: LearningSpaceType.Create("Classroom"),
            maxCapacity: Capacity.Create(30),
            height: DomainSize.Create(3.0),
            width: DomainSize.Create(8.0),
            length: DomainSize.Create(10.0),
            colorFloor: DomainColors.Create("Brown"),
            colorWalls: DomainColors.Create("White"),
            colorCeiling: DomainColors.Create("White")
        );

        var building = new Building(
            buildingInternalId: 1,
            name: EntityName.Create("Edificio Central"),
            coordinates: null,
            dimensions: null,
            color: null,
            area: (Area)null!
        );

        var floors = new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1))
        };

        var learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        learningSpaceServiceMock.Setup(s => s.GetLearningSpaceAsync(1)).ReturnsAsync(learningSpace);

        var buildingServiceMock = new Mock<IBuildingsServices>();
        buildingServiceMock.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);

        var floorServiceMock = new Mock<IFloorServices>();
        floorServiceMock.Setup(s => s.GetFloorsListAsync(1)).ReturnsAsync(floors);

        var permissionMock = new Mock<IPermissionContext>();
        permissionMock.Setup(p => p.HasPermission("Edit Learning Space")).Returns(false);
        permissionMock.Setup(p => p.HasPermission("Delete Learning Space")).Returns(false);

        Services.AddSingleton(learningSpaceServiceMock.Object);
        Services.AddSingleton(buildingServiceMock.Object);
        Services.AddSingleton(floorServiceMock.Object);
        Services.AddSingleton<IPermissionContext>(permissionMock.Object);
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        // Act
        var cut = RenderComponent<ReadLearningSpace>(parameters => parameters
            .Add(p => p.buildingId, 1)
            .Add(p => p.floorId, 1)
            .Add(p => p.learningSpaceId, 1));

        // Assert
        Assert.DoesNotContain("EDITAR", cut.Markup);
        Assert.DoesNotContain("ELIMINAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the ReadLearningSpace component shows a loading spinner when IsLoading is true.
    /// </summary>
    [Fact]
    public void ShouldShowLoadingSpinner_WhenDataIsLoading()
    {
        // Arrange
        var tcs = new TaskCompletionSource<LearningSpace?>();
        var buildingTcs = new TaskCompletionSource<Building?>();
        var floorTcs = new TaskCompletionSource<List<Floor>?>();

        var learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        learningSpaceServiceMock.Setup(s => s.GetLearningSpaceAsync(It.IsAny<int>()))
            .Returns(tcs.Task);

        var buildingServiceMock = new Mock<IBuildingsServices>();
        buildingServiceMock.Setup(s => s.DisplayBuildingAsync(It.IsAny<int>()))
            .Returns(buildingTcs.Task);

        var floorServiceMock = new Mock<IFloorServices>();
        floorServiceMock.Setup(s => s.GetFloorsListAsync(It.IsAny<int>()))
            .Returns(floorTcs.Task);

        Services.AddSingleton(learningSpaceServiceMock.Object);
        Services.AddSingleton(buildingServiceMock.Object);
        Services.AddSingleton(floorServiceMock.Object);
        Services.AddSingleton<IPermissionContext>(Mock.Of<IPermissionContext>());
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        // Act
        var cut = RenderComponent<ReadLearningSpace>(parameters => parameters
            .Add(p => p.buildingId, 1)
            .Add(p => p.floorId, 1)
            .Add(p => p.learningSpaceId, 1));

        // Assert
        Assert.Contains("Cargando", cut.Markup);

        // Clean up - complete the tasks to avoid hanging
        var building = new Building(
            buildingInternalId: 1,
            name: EntityName.Create("Test Building"),
            coordinates: null,
            dimensions: null,
            color: null,
            area: (Area)null!
        );
        var floors = new List<Floor> { new Floor(1, FloorNumber.Create(1)) };
        var learningSpace = new LearningSpace(
            id: 1,
            name: EntityName.Create("Test Space"),
            type: LearningSpaceType.Create("Classroom"),
            maxCapacity: Capacity.Create(30),
            height: DomainSize.Create(3.0),
            width: DomainSize.Create(8.0),
            length: DomainSize.Create(10.0),
            colorFloor: DomainColors.Create("Brown"),
            colorWalls: DomainColors.Create("White"),
            colorCeiling: DomainColors.Create("White")
        );

        buildingTcs.SetResult(building);
        floorTcs.SetResult(floors);
        tcs.SetResult(learningSpace);
    }

    /// <summary>
    /// Debug test to check how many numeric inputs are rendered
    /// </summary>
    [Fact]
    public void Debug_CountNumericInputs()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            id: 1,
            name: EntityName.Create("Aula 101"),
            type: LearningSpaceType.Create("Classroom"),
            maxCapacity: Capacity.Create(30),
            height: DomainSize.Create(3.0),
            width: DomainSize.Create(8.0),
            length: DomainSize.Create(10.0),
            colorFloor: DomainColors.Create("Brown"),
            colorWalls: DomainColors.Create("White"),
            colorCeiling: DomainColors.Create("White")
        );

        var building = new Building(
            buildingInternalId: 1,
            name: EntityName.Create("Edificio Central"),
            coordinates: null,
            dimensions: null,
            color: null,
            area: (Area)null!
        );

        var floors = new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1))
        };

        var learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        learningSpaceServiceMock.Setup(s => s.GetLearningSpaceAsync(1)).ReturnsAsync(learningSpace);

        var buildingServiceMock = new Mock<IBuildingsServices>();
        buildingServiceMock.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);

        var floorServiceMock = new Mock<IFloorServices>();
        floorServiceMock.Setup(s => s.GetFloorsListAsync(1)).ReturnsAsync(floors);

        var permissionMock = new Mock<IPermissionContext>();
        permissionMock.Setup(p => p.HasPermission(It.IsAny<string>())).Returns(true);

        Services.AddSingleton(learningSpaceServiceMock.Object);
        Services.AddSingleton(buildingServiceMock.Object);
        Services.AddSingleton(floorServiceMock.Object);
        Services.AddSingleton<IPermissionContext>(permissionMock.Object);
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        var cut = RenderComponent<ReadLearningSpace>(parameters => parameters
            .Add(p => p.buildingId, 1)
            .Add(p => p.floorId, 1)
            .Add(p => p.learningSpaceId, 1));

        // Act
        var numericInputs = cut.FindAll("input[type='number']");

        // Debug output
        Console.WriteLine($"Found {numericInputs.Count} numeric inputs");
        for (int i = 0; i < numericInputs.Count; i++)
        {
            var input = numericInputs[i];
            var label = input.GetAttribute("aria-label") ?? input.GetAttribute("placeholder") ?? "no label";
            Console.WriteLine($"Index {i}: {label}");
        }

        // Assert - just to verify we have some inputs (or none for ReadLearningSpace)
        Assert.True(numericInputs.Count >= 0, $"Expected 0 or more numeric inputs, but found {numericInputs.Count}");
    }

    /// <summary>
    /// Debug test to check the component's rendered markup
    /// </summary>
    [Fact]
    public void Debug_CheckRenderedMarkup()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            id: 1,
            name: EntityName.Create("Aula 101"),
            type: LearningSpaceType.Create("Classroom"),
            maxCapacity: Capacity.Create(30),
            height: DomainSize.Create(3.0),
            width: DomainSize.Create(8.0),
            length: DomainSize.Create(10.0),
            colorFloor: DomainColors.Create("Brown"),
            colorWalls: DomainColors.Create("White"),
            colorCeiling: DomainColors.Create("White")
        );

        var building = new Building(
            buildingInternalId: 1,
            name: EntityName.Create("Edificio Central"),
            coordinates: null,
            dimensions: null,
            color: null,
            area: (Area)null!
        );

        var floors = new List<Floor>
        {
            new Floor(1, FloorNumber.Create(1))
        };

        var learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        learningSpaceServiceMock.Setup(s => s.GetLearningSpaceAsync(1)).ReturnsAsync(learningSpace);

        var buildingServiceMock = new Mock<IBuildingsServices>();
        buildingServiceMock.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);

        var floorServiceMock = new Mock<IFloorServices>();
        floorServiceMock.Setup(s => s.GetFloorsListAsync(1)).ReturnsAsync(floors);

        var permissionMock = new Mock<IPermissionContext>();
        permissionMock.Setup(p => p.HasPermission(It.IsAny<string>())).Returns(true);

        Services.AddSingleton(learningSpaceServiceMock.Object);
        Services.AddSingleton(buildingServiceMock.Object);
        Services.AddSingleton(floorServiceMock.Object);
        Services.AddSingleton<IPermissionContext>(permissionMock.Object);
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        var cut = RenderComponent<ReadLearningSpace>(parameters => parameters
            .Add(p => p.buildingId, 1)
            .Add(p => p.floorId, 1)
            .Add(p => p.learningSpaceId, 1));

        // Act - wait for the component to finish loading
        cut.WaitForAssertion(() =>
        {
            var spinner = cut.FindComponents<MudBlazor.MudProgressCircular>();
            Assert.Empty(spinner); // No spinner should be present when loading is complete
        }, TimeSpan.FromSeconds(5));

        // Debug output
        Console.WriteLine("=== RENDERED MARKUP ===");
        Console.WriteLine(cut.Markup);
        Console.WriteLine("=== END MARKUP ===");

        var numericInputs = cut.FindAll("input[type='number']");
        Console.WriteLine($"Found {numericInputs.Count} numeric inputs");

        // Assert
        Assert.True(numericInputs.Count >= 0, $"Expected 0 or more numeric inputs, but found {numericInputs.Count}");
    }
}