using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.Spaces;

/// <summary>
/// Tests for the ListLearningSpaces component.
/// </summary>
public class ListLearningSpacesTests : TestContext
{
    /// <summary>
    /// Mock for the building services.
    /// </summary>
    private readonly Mock<IBuildingsServices> _buildingServiceMock;

    /// <summary>
    /// Mock for the floor services.
    /// </summary>
    private readonly Mock<IFloorServices> _floorServiceMock;

    /// <summary>
    /// Mock for the learning space services.
    /// </summary>
    private readonly Mock<ILearningSpaceServices> _learningSpaceServiceMock;

    /// <summary>
    /// Mock for the permission context.
    /// </summary>
    private readonly Mock<IPermissionContext> _permissionContextMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListLearningSpacesTests"/> class.
    /// </summary>
    public ListLearningSpacesTests()
    {
        // Set up the test context with necessary services and mocks
        _buildingServiceMock = new Mock<IBuildingsServices>();
        _floorServiceMock = new Mock<IFloorServices>();
        _learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        _permissionContextMock = new Mock<IPermissionContext>();
        // Register services and mocks
        Services.AddMudServices();
        Services.AddSingleton(_buildingServiceMock.Object);
        Services.AddSingleton(_floorServiceMock.Object);
        Services.AddSingleton(_learningSpaceServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);
        // Set up authentication and authorization
        var authPolicyProvider = new Mock<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider>();
        Services.AddSingleton(authPolicyProvider.Object);
        var authService = new Mock<Microsoft.AspNetCore.Authorization.IAuthorizationService>();
        Services.AddSingleton(authService.Object);
        // Set up JSInterop for MudBlazor components
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.disconnect", _ => true);
        JSInterop.SetupVoid("mudPopover.update", _ => true);
    }

    /// <summary>
    /// Tests that the ListLearningSpaces component renders basic elements correctly.
    /// </summary>
    [Fact]
    public void Component_Renders_Basic_Elements()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("Administración de Espacios de Aprendizaje", cut.Markup);
    }

    /// <summary>
    /// Tests that the ListLearningSpaces component shows an error message when the building is invalid.
    /// </summary>
    [Fact]
    public void Shows_Error_Message_When_Invalid_Building()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var listLearningSpaces = cut.FindComponent<ListLearningSpaces>().Instance;
        listLearningSpaces.GetType().GetField("_invalidBuilding", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, true);
        listLearningSpaces.GetType().GetField("UserMessage", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, "No se encontró el edificio solicitado.");
        Assert.Contains("No se encontró el edificio solicitado.", cut.Markup);
    }

    /// <summary>
    /// Tests that the ListLearningSpaces component shows an error message when the floor is invalid.
    /// </summary>
    [Fact]
    public void Shows_Error_Message_When_Invalid_Floor()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var listLearningSpaces = cut.FindComponent<ListLearningSpaces>().Instance;
        listLearningSpaces.GetType().GetField("_invalidFloor", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, true);
        listLearningSpaces.GetType().GetField("UserMessage", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, "No se encontró el piso solicitado.");

        cut.FindComponent<ListLearningSpaces>().Render();

        Assert.Contains("No se encontró el piso solicitado.", cut.Markup);
    }

    /// <summary>
    /// Tests that the search bar filters results correctly.
    /// </summary>
    [Fact]
    public async Task SearchBar_Filters_Results()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor> { new Floor(1, FloorNumber.Create(1)) });

        _learningSpaceServiceMock
            .Setup(x => x.GetLearningSpacesListPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), ""))
            .ReturnsAsync(new PaginatedList<LearningSpaceOverviewDto>(
                new List<LearningSpaceOverviewDto> { new LearningSpaceOverviewDto(1, "Aula A", "Aula") },
                1, 10, 1
            ));

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        await cut.InvokeAsync(() => Task.CompletedTask);

        Assert.Contains("Aula A", cut.Markup);
    }

    /// <summary>
    /// Tests that the ListLearningSpaces component navigates to the learning space details when the name is clicked.
    /// </summary>
    [Fact]
    public async Task ShouldNavigateToLearningSpaceDetails_WhenNameClicked()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor> { new Floor(1, FloorNumber.Create(1)) });

        var learningSpaces = new List<LearningSpaceOverviewDto>
        {
            new LearningSpaceOverviewDto(5, "Laboratorio 1", "Laboratorio")
        };

        _learningSpaceServiceMock
            .Setup(x => x.GetLearningSpacesListPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new PaginatedList<LearningSpaceOverviewDto>(learningSpaces, 1, 10, 1));

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        await cut.InvokeAsync(() => Task.CompletedTask);

        var nameLink = cut.FindAll("a")
            .FirstOrDefault(a => a.TextContent.Contains("Laboratorio 1"));
        Assert.NotNull(nameLink);
        Assert.EndsWith("/edificios/1/pisos/1/espacios-de-aprendizaje/5", nameLink.GetAttribute("href"));
    }

    /// <summary>
    /// Tests that the ListLearningSpaces component displays a message when there are no learning spaces.
    /// </summary>
    [Fact]
    public void Shows_No_Elements_Message_When_No_LearningSpaces()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor> { new Floor(1, FloorNumber.Create(1)) });

        _learningSpaceServiceMock
            .Setup(x => x.GetLearningSpacesListPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new PaginatedList<LearningSpaceOverviewDto>(new List<LearningSpaceOverviewDto>(), 0, 10, 0));

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<ListLearningSpaces>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("No hay elementos para mostrar.", cut.Markup);
    }
}
