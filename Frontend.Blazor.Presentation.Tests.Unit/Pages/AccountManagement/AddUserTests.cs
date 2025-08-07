using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.AccountManagement;

/// <summary>
/// Unit tests for the AddUser component.
/// </summary>
public class AddUserTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserTests"/> class.
    /// </summary>
    public AddUserTests()
    {
        Services.AddMudServices();

        // Ignore JavaScript interop calls that are not relevant to the tests
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);
    }

    /// <summary>
    /// Tests that the AddUser component renders correctly with no initial data.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenFirstNameIsEmpty()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());

        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);

        // Render AddUser with MudPopoverProvider 
        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<AddUser>(1);
            builder.CloseComponent();
        });

        // Act
        cut.Find("button:contains('GUARDAR')").Click();

        // Assert
        Assert.Contains("El nombre es obligatorio", cut.Markup);
    }

    /// <summary>
    /// Tests that the AddUser component shows a validation error when the last name is empty.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<AddUser>(1);
            builder.CloseComponent();
        });

        // Act
        cut.FindAll("input")[0].Change("Juan");
        cut.FindAll("input")[1].Change("Pérez");
        cut.FindAll("input")[2].Change("1-1234-5678"); 
        cut.FindAll("input")[3].Change("1234-5678"); 
        cut.FindAll("input")[4].Change("correo@invalido"); 
        cut.FindAll("input")[5].Change("juan_23");

        cut.Find("button:contains('GUARDAR')").Click();

        // Assert
        Assert.Contains("El correo electrónico no tiene un formato válido", cut.Markup);
    }

    /// <summary>
    /// Tests that the AddUser component shows a validation error when the identity number is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenIdentityNumberIsInvalid()
    {
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<AddUser>(1);
            builder.CloseComponent();
        });

        cut.FindAll("input")[2].Change("abc");
        cut.Find("button:contains('GUARDAR')").Click();

        Assert.Contains("El número de identificación debe tener el formato", cut.Markup);
    }

    /// <summary>
    /// Tests that the AddUser component shows a validation error when the phone number is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenPhoneIsInvalid()
    {
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<AddUser>(1);
            builder.CloseComponent();
        });

        var phoneInput = cut.Find("input[placeholder='1234-5678']");
        phoneInput.Change("abcd-1234");
        phoneInput.Blur();
        cut.Find("button:contains('GUARDAR')").Click();


        Assert.Contains("El teléfono debe tener el formato", cut.Markup);
    }

    /// <summary>
    /// Tests that the AddUser component shows a validation error when the username is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenUserNameIsInvalid()
    {
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<AddUser>(1);
            builder.CloseComponent();
        });

        var usernameInput = cut.Find("input[placeholder='ej. juan.ramirez o juan_23']");
        usernameInput.Change("JUAN*123");
        usernameInput.Blur();

        cut.Find("button:contains('GUARDAR')").Click();

        Assert.Contains("El nombre de usuario solo puede contener letras", cut.Markup);
    }
}
