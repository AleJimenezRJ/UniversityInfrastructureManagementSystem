using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
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
/// Unit tests for the FloorManagement component.
/// These tests cover rendering, data fetching, and interaction with the component.
/// </summary>
public class FloorManagementTests : TestContext
{
    /// <summary>
    /// Mock for the building services to simulate fetching building data.
    /// </summary>
    private readonly Mock<IBuildingsServices> _buildingServiceMock;


    /// <summary>
    /// Mock for the floor services to simulate fetching floor data.
    /// </summary>
    private readonly Mock<IFloorServices> _floorServiceMock;

    /// <summary>
    /// Mock for the learning space services to simulate fetching learning space data.
    /// </summary>
    private readonly Mock<ILearningSpaceServices> _learningSpaceServiceMock;

    /// <summary>
    /// Mock for the permission context to simulate user permissions.
    /// </summary>
    private readonly Mock<IPermissionContext> _permissionContextMock;

    /// <summary>
    /// Constructor to set up the test context with necessary services and mocks.
    /// </summary>
    public FloorManagementTests()
    {
        // Set up the test context with necessary services and mocks
        _buildingServiceMock = new Mock<IBuildingsServices>();
        _floorServiceMock = new Mock<IFloorServices>();
        _learningSpaceServiceMock = new Mock<ILearningSpaceServices>();
        _permissionContextMock = new Mock<IPermissionContext>();
        // Add necessary services and mocks to the test context
        Services.AddMudServices();
        Services.AddSingleton(_buildingServiceMock.Object);
        Services.AddSingleton(_floorServiceMock.Object);
        Services.AddSingleton(_learningSpaceServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);
        // Set up the permission context to allow all permissions for testing
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
    /// Tests that the FloorManagement component renders basic elements correctly.
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

                childBuilder.OpenComponent<FloorManagement>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("Administración de Pisos", cut.Markup);
    }

    /// <summary>
    /// Tests that the FloorManagement component displays an error message when the building is invalid.
    /// </summary>
    [Fact]
    public void Shows_Error_Message_When_Invalid_Building()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync((Building?)null);

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<FloorManagement>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("No se encontró el edificio solicitado", cut.Markup);
    }

    /// <summary>
    /// Tests that the FloorManagement component navigates to the learning spaces list when the view link is clicked.
    /// </summary>
    [Fact]
    public void ShouldNavigateToLearningSpacesList_WhenViewLinkClicked()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new PaginatedList<Floor>(
                new List<Floor> { new Floor(5, FloorNumber.Create(2)) }, 1, 10, 0
            ));

        Services.AddSingleton<NavigationManager, FakeNavigationManager>();

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<FloorManagement>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var viewLink = cut.FindAll("a")
        .FirstOrDefault(a => a.TextContent.Contains("Ver"));
        Assert.NotNull(viewLink);
        Assert.EndsWith("/edificios/1/pisos/5/espacios-de-aprendizaje", viewLink.GetAttribute("href"));
    }

    /// <summary>
    /// Tests that the FloorManagement component displays a message when there are no floors.
    /// </summary>
    [Fact]
    public void Shows_No_Elements_Message_When_No_Floors()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new PaginatedList<Floor>(new List<Floor>(), 0, 10, 0));

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<FloorManagement>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("No hay elementos para mostrar.", cut.Markup);
    }

}
