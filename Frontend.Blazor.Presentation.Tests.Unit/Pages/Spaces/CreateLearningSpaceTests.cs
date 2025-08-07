using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Spaces;
using SpacesVO = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UMVO = UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.Spaces;

/// <summary>
/// Unit tests for the CreateLearningSpace component.
/// </summary>
public class CreateLearningSpaceTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLearningSpaceTests"/> class.
    /// </summary>
    public CreateLearningSpaceTests()
    {
        Services.AddMudServices();

        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the name is empty.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("El nombre es obligatorio y debe tener menos de 100 caracteres.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the width is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenWidthIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        var widthInput = cut.Find("input[type='number']");
        widthInput.Change("0");
        widthInput.Blur();

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("El ancho debe ser un número positivo mayor a 0.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the max capacity is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenMaxCapacityIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        var capacityInputs = cut.FindAll("input[type='number']");
        var maxCapacityInput = capacityInputs.Count > 1 ? capacityInputs[1] : capacityInputs.Last();
        maxCapacityInput.Change("-1");
        maxCapacityInput.Blur();

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("La capacidad máxima debe ser un número entero positivo.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the height is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenHeightIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        var heightInputs = cut.FindAll("input[type='number']");
        var heightInput = heightInputs.Count > 2 ? heightInputs[2] : heightInputs.Last();
        heightInput.Change("0");
        heightInput.Blur();

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("La altura debe ser un número positivo mayor a 0.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the type is not selected.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenTypeIsNotSelected()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("Debe seleccionar un tipo de espacio válido.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the length is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenLengthIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        var lengthInputs = cut.FindAll("input[type='number']");
        var lengthInput = lengthInputs.Count > 3 ? lengthInputs[3] : lengthInputs.Last();
        lengthInput.Change("-5");
        lengthInput.Blur();

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("El largo debe ser un número positivo mayor a 0.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the floor color is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenColorFloorIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("Debe seleccionar un color válido para el piso.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the walls color is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenColorWallsIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("Debe seleccionar un color válido para las paredes.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows a validation error when the ceiling color is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenColorCeilingIsInvalid()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula 101");

        cut.Find("button:contains('Agregar')").Click();

        // Assert
        Assert.Contains("Debe seleccionar un color válido para el cielo raso.", cut.Markup);
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows an error message when building is not found.
    /// </summary>
    [Fact]
    public void ShouldShowErrorMessage_WhenBuildingNotFound()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync((Building?)null);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No se encontró el edificio solicitado.", cut.Markup);
        });
    }

    /// <summary>
    /// Tests that the CreateLearningSpace component shows an error message when floor is not found.
    /// </summary>
    [Fact]
    public void ShouldShowErrorMessage_WhenFloorNotFound()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var building = CreateTestBuilding();

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(building);

        mockFloorService.Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor>());

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No se encontró el piso solicitado.", cut.Markup);
        });
    }

    /// <summary>
    /// Debug test to check how many numeric inputs are rendered
    /// </summary>
    [Fact]
    public void Debug_CountNumericInputs()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

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

        // Assert - just to verify we have some inputs
        Assert.True(numericInputs.Count > 0, $"Expected at least 1 numeric input, but found {numericInputs.Count}");
    }

    /// <summary>
    /// Debug test to check the component's rendered markup
    /// </summary>
    [Fact]
    public void Debug_CheckRenderedMarkup()
    {
        // Arrange
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        SetupValidBuildingAndFloor(mockBuildingService, mockFloorService);

        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1);

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
        Assert.True(numericInputs.Count >= 1, $"Expected at least 1 numeric input, but found {numericInputs.Count}");
    }

    /// <summary>
    /// Helper method to set up valid building and floor data for tests.
    /// </summary>
    /// <param name="mockBuildingService">Mocked building service</param>
    /// <param name="mockFloorService">Mocked floor service</param>
    private void SetupValidBuildingAndFloor(Mock<IBuildingsServices> mockBuildingService, Mock<IFloorServices> mockFloorService)
    {
        var building = CreateTestBuilding();

        var floors = new List<Floor>
        {
            new Floor(
                1,
                SpacesVO.FloorNumber.Create(1)
            )
        };

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(building);

        mockFloorService.Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(floors);
    }

    /// <summary>
    /// Helper method to create a test building with valid data.
    /// </summary>
    private Building CreateTestBuilding()
    {
        // Create the area dependency
        var area = CreateTestArea();

        return new Building(
            UMVO.EntityName.Create("Edificio Test"),
            null, // coordinates
            null, // dimensions
            null, // color
            area // area object, not EntityName
        );
    }

    /// <summary>
    /// Helper method to create a test area with valid data.
    /// </summary>
    private Area CreateTestArea()
    {
        // Create University
        var university = new University(
            UMVO.EntityName.Create("UCR"),
            UMVO.EntityLocation.Create("Costa Rica")
        );

        // Create Campus
        var campus = new Campus(
            UMVO.EntityName.Create("Campus Central"),
            UMVO.EntityLocation.Create("San Pedro"),
            university
        );

        // Create Area
        return new Area(
            UMVO.EntityName.Create("Area Test"),
            campus
        );
    }

    /// <summary>
    /// Helper method to render the CreateLearningSpace component with the specified parameters.
    /// </summary>
    /// <param name="buildingId">Building ID parameter</param>
    /// <param name="floorId">Floor ID parameter</param>
    /// <returns>Rendered component</returns>
    private IRenderedComponent<CreateLearningSpace> RenderComponent(int buildingId, int floorId)
    {
        var rendered = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<CreateLearningSpace>(1);
            builder.AddAttribute(2, "buildingId", buildingId);
            builder.AddAttribute(3, "floorId", floorId);
            builder.CloseComponent();
        });

        return rendered.FindComponent<CreateLearningSpace>();
    }
}
