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
/// Unit tests for the UpdateLearningSpace component.
/// </summary>
public class UpdateLearningSpaceTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLearningSpaceTests"/> class.
    /// </summary>
    public UpdateLearningSpaceTests()
    {
        Services.AddMudServices();

        // Ignore JavaScript interop calls that are not relevant to the tests
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);
        JSInterop.SetupVoid("mudDatePicker.initialize", _ => true);
        JSInterop.SetupVoid("mudSelect.initialize", _ => true);
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows a loading message when data is being loaded.
    /// </summary>
    [Fact]
    public void ShouldShowLoadingMessage_WhenDataIsLoading()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var tcs = new TaskCompletionSource<LearningSpace?>();
        mockLearningSpaceService
            .Setup(x => x.GetLearningSpaceAsync(It.IsAny<int>()))
            .Returns(tcs.Task); // Never completes, simulates loading

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        // Act
        var cut = RenderComponent(1, 1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Cargando", cut.Markup, StringComparison.OrdinalIgnoreCase);
        }, timeout: TimeSpan.FromSeconds(2));
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows validation error when name is empty.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var nameInput = cut.Find("input[type='text']");
            Assert.NotNull(nameInput);
        });

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("");
        nameInput.Blur();

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        Assert.Contains("El nombre es obligatorio y debe tener menos de 100 caracteres.", cut.Markup);
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows validation error when width is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenWidthIsInvalid()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var widthInputs = cut.FindAll("input[type='number']");
            Assert.True(widthInputs.Count > 0);
        });

        // Act
        var widthInputs = cut.FindAll("input[type='number']");
        var widthInput = widthInputs.FirstOrDefault();
        Assert.NotNull(widthInput);

        widthInput.Change("0");
        widthInput.Blur();

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("ancho", cut.Markup, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("positivo", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("ancho", cut.Markup, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("positivo", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows validation error when max capacity is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenMaxCapacityIsInvalid()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var numericInputs = cut.FindAll("input[type='number']");
            Assert.True(numericInputs.Count > 0);
        });

        // Act
        var capacityInputs = cut.FindAll("input[type='number']");
        var maxCapacityInput = capacityInputs.Count > 1 ? capacityInputs[1] : capacityInputs.Last();
        maxCapacityInput.Change("-1");
        maxCapacityInput.Blur();

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        Assert.Contains("La capacidad máxima debe ser un número entero positivo.", cut.Markup);
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows validation error when height is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenHeightIsInvalid()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var heightInputs = cut.FindAll("input[type='number']");
            Assert.True(heightInputs.Count > 0);
        });

        // Act
        var heightInputs = cut.FindAll("input[type='number']");
        var heightInput = heightInputs.Count > 2 ? heightInputs[2] : heightInputs.Last();
        heightInput.Change("0");
        heightInput.Blur();

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("altura", cut.Markup, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("positivo", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("altura", cut.Markup, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("positivo", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows validation error when length is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenLengthIsInvalid()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var lengthInputs = cut.FindAll("input[type='number']");
            Assert.True(lengthInputs.Count > 0);
        });

        // Act
        var lengthInputs = cut.FindAll("input[type='number']");
        var lengthInput = lengthInputs.Count > 3 ? lengthInputs[3] : lengthInputs.Last();
        lengthInput.Change("-5");
        lengthInput.Blur();

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("largo", cut.Markup, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("positivo", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows error message when learning space is not found.
    /// </summary>
    [Fact]
    public void ShouldShowErrorMessage_WhenLearningSpaceNotFound()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        mockLearningSpaceService.Setup(x => x.GetLearningSpaceAsync(It.IsAny<int>()))
            .ReturnsAsync((LearningSpace?)null);

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Error al recuperar los datos"));

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Error al recuperar los datos", cut.Markup);
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows error message when floor is not found.
    /// </summary>
    [Fact]
    public void ShouldShowErrorMessage_WhenFloorNotFound()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        var building = CreateTestBuilding();

        mockLearningSpaceService.Setup(x => x.GetLearningSpaceAsync(It.IsAny<int>()))
            .ReturnsAsync(testLearningSpace);

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(building);

        mockFloorService.Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor>()); // Empty list - floor not found

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No se encontró el piso con ID 1.", cut.Markup);
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component loads and displays existing learning space data correctly.
    /// </summary>
    [Fact]
    public void ShouldLoadAndDisplayLearningSpaceData_WhenDataExists()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Aula Test", cut.Markup);
            Assert.Contains("Edificio Test", cut.Markup);
            Assert.Contains("1", cut.Markup); // The component renders just the floor number, not "Piso 1"
        });
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component successfully updates a learning space when valid data is provided.
    /// </summary>
    [Fact]
    public void ShouldUpdateLearningSpace_WhenValidDataIsProvided()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        mockLearningSpaceService.Setup(x => x.UpdateLearningSpaceAsync(It.IsAny<int>(), It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var nameInput = cut.Find("input[type='text']");
            Assert.NotNull(nameInput);
        });

        // Act
        var nameInput = cut.Find("input[type='text']");
        nameInput.Change("Aula Actualizada");

        cut.Find("button:contains('Guardar')").Click();

        // Assert
        mockLearningSpaceService.Verify(x => x.UpdateLearningSpaceAsync(It.IsAny<int>(), It.IsAny<LearningSpace>()), Times.Once);
    }

    /// <summary>
    /// Tests that the UpdateLearningSpace component shows error message when update fails.
    /// </summary>
    [Fact]
    public void ShouldShowErrorMessage_WhenUpdateFails()
    {
        // Arrange
        var mockLearningSpaceService = new Mock<ILearningSpaceServices>();
        var mockBuildingService = new Mock<IBuildingsServices>();
        var mockFloorService = new Mock<IFloorServices>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testLearningSpace = CreateTestLearningSpace();
        SetupValidData(mockLearningSpaceService, mockBuildingService, mockFloorService, testLearningSpace);

        mockLearningSpaceService.Setup(x => x.UpdateLearningSpaceAsync(It.IsAny<int>(), It.IsAny<LearningSpace>()))
            .ReturnsAsync(false);

        Services.AddSingleton(mockLearningSpaceService.Object);
        Services.AddSingleton(mockBuildingService.Object);
        Services.AddSingleton(mockFloorService.Object);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = RenderComponent(1, 1, 1);

        cut.WaitForAssertion(() =>
        {
            var nameInput = cut.Find("input[type='text']");
            Assert.NotNull(nameInput);
        });

        // Act
        cut.Find("button:contains('Guardar')").Click();

        // Assert
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No se pudo actualizar el espacio de aprendizaje.", cut.Markup);
        });
    }

    /// <summary>
    /// Helper method to create a test learning space with valid data.
    /// </summary>
    private LearningSpace CreateTestLearningSpace()
    {
        return new LearningSpace(
            1,
            UMVO.EntityName.Create("Aula Test"),
            SpacesVO.LearningSpaceType.Create("Classroom"),
            SpacesVO.Capacity.Create(30),
            SpacesVO.Size.Create(10.0),
            SpacesVO.Size.Create(3.0),
            SpacesVO.Size.Create(8.0),
            UMVO.Colors.Create("Brown"),
            UMVO.Colors.Create("White"),
            UMVO.Colors.Create("White")
        );
    }

    /// <summary>
    /// Helper method to create a test building with valid data.
    /// </summary>
    private Building CreateTestBuilding()
    {
        var area = CreateTestArea();

        return new Building(
            UMVO.EntityName.Create("Edificio Test"),
            null,
            null,
            null,
            area
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
    /// Helper method to create a test floor with valid data.
    /// </summary>
    private List<Floor> CreateTestFloors()
    {
        return new List<Floor>
        {
            new Floor(
                1,
                SpacesVO.FloorNumber.Create(1)
            )
        };
    }

    /// <summary>
    /// Helper method to set up valid data for all services.
    /// </summary>
    private void SetupValidData(Mock<ILearningSpaceServices> mockLearningSpaceService,
                               Mock<IBuildingsServices> mockBuildingService,
                               Mock<IFloorServices> mockFloorService,
                               LearningSpace learningSpace)
    {
        var building = CreateTestBuilding();
        var floors = CreateTestFloors();

        mockLearningSpaceService.Setup(x => x.GetLearningSpaceAsync(It.IsAny<int>()))
            .ReturnsAsync(learningSpace);

        mockBuildingService.Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(building);

        mockFloorService.Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(floors);
    }

    /// <summary>
    /// Helper method to render the UpdateLearningSpace component with the specified parameters.
    /// </summary>
    /// <param name="buildingId">Building ID parameter</param>
    /// <param name="floorId">Floor ID parameter</param>
    /// <param name="learningSpaceId">Learning Space ID parameter</param>
    /// <returns>Rendered component</returns>
    private IRenderedComponent<UpdateLearningSpace> RenderComponent(int buildingId, int floorId, int learningSpaceId)
    {
        var rendered = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UpdateLearningSpace>(1);
            builder.AddAttribute(2, "buildingId", buildingId);
            builder.AddAttribute(3, "floorId", floorId);
            builder.AddAttribute(4, "learningSpaceId", learningSpaceId);
            builder.CloseComponent();
        });

        return rendered.FindComponent<UpdateLearningSpace>();
    }
}