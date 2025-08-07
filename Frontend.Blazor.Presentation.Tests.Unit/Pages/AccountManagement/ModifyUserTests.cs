using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.AccountManagement;

/// <summary>
/// Unit tests for the ModifyUser component.
/// </summary>
public class ModifyUserTests : TestContext
{
    private Mock<IUserWithPersonService> _mockUserService;
    private Mock<IRoleService> _mockRoleService;
    private Mock<UserNavigationContext> _mockNavContext;
    private Mock<ISnackbar> _mockSnackbar;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifyUserTests"/> class.
    /// </summary>
    public ModifyUserTests()
    {
        Services.AddMudServices();

        // Setup authorization
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("TestUser");
        authContext.SetPolicies("Edit Users");

        // Ignore JavaScript interop calls that are not relevant to the tests
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);
        JSInterop.SetupVoid("mudDatePicker.initialize", _ => true);
        JSInterop.SetupVoid("mudSelect.initialize", _ => true);
    }

    // <summary>
    /// Tests that the ModifyUser component renders loading message when user is null.
    /// </summary>
    [Fact]
    public void ShouldShowLoadingMessage_WhenUserIsNull()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        var mockNavContext = new Mock<UserNavigationContext>();
        var mockSnackbar = new Mock<ISnackbar>();

        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());

        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);
        Services.AddSingleton(mockNavContext.Object);
        Services.AddSingleton(mockSnackbar.Object);

        // Act
        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<ModifyUser>(1);
            builder.CloseComponent();
        });

        // Assert
        Assert.Contains("Cargando información del usuario...", cut.Markup);
    }

    /// <summary>
    /// Tests that the ModifyUser component shows validation error when first name is empty.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenFirstNameIsEmpty()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        var mockSnackbar = new Mock<ISnackbar>();
        var testUser = CreateTestUser();
        testUser.FirstName = "ValidName"; // Start with a valid name instead of empty
        var navContext = new UserNavigationContext
        {
            SelectedUserId = testUser.UserId // Add this if needed, like in your email test
        };
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        mockUserService.Setup(x => x.GetAllUserWithPersonAsync())
            .ReturnsAsync(new List<UserWithPerson> { testUser });
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);
        Services.AddSingleton(navContext);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<ModifyUser>(1);
            builder.CloseComponent();
        });

        // Act - Find the first name input and clear it (change from valid to empty)
        var firstNameInput = cut.Find("#first-name-input");
        firstNameInput.Change(""); // Change from "ValidName" to empty
        firstNameInput.Blur();

        // Try to save
        cut.Find("button:contains('GUARDAR')").Click();

        // Assert
        Assert.Contains("El nombre es obligatorio", cut.Markup);
    }

    /// <summary>
    /// Tests that the ModifyUser component shows validation error when email format is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenEmailFormatIsInvalid()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        var mockSnackbar = new Mock<ISnackbar>();

        var testUser = CreateTestUser();

        var navContext = new UserNavigationContext
        {
            SelectedUserId = testUser.UserId
        };

        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        mockUserService.Setup(x => x.GetAllUserWithPersonAsync())
            .ReturnsAsync(new List<UserWithPerson> { testUser });

        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);
        Services.AddSingleton(navContext);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<ModifyUser>(1);
            builder.CloseComponent();
        });

        // Act - Find email input using the correct selector and set invalid email
        var emailInput = cut.Find("#email-input");
        emailInput.Change("invalid-email");
        emailInput.Blur();

        // Try to save
        cut.Find("button:contains('GUARDAR')").Click();

        // Assert
        Assert.Contains("El correo electrónico no tiene un formato válido.", cut.Markup);
    }

    /// <summary>
    /// Tests that the ModifyUser component shows validation error when phone format is invalid.
    /// </summary>
    [Fact]
    public void ShouldShowValidationError_WhenPhoneFormatIsInvalid()
    {
        // Arrange
        var mockUserService = new Mock<IUserWithPersonService>();
        var mockRoleService = new Mock<IRoleService>();
        var mockSnackbar = new Mock<ISnackbar>();
        var testUser = CreateTestUser();
        testUser.Phone.Value = "1234-5678"; // Start with a valid phone format
        var navContext = new UserNavigationContext
        {
            SelectedUserId = testUser.UserId
        };
        mockRoleService.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(new List<Role>());
        mockUserService.Setup(x => x.GetAllUserWithPersonAsync())
            .ReturnsAsync(new List<UserWithPerson> { testUser });
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockRoleService.Object);
        Services.AddSingleton(navContext);
        Services.AddSingleton(mockSnackbar.Object);

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<ModifyUser>(1);
            builder.CloseComponent();
        });

        // Act - Find phone input using InputId and set invalid phone
        var phoneInput = cut.Find("#phone-input");
        phoneInput.Change("invalid-phone");
        phoneInput.Blur();

        // Try to save
        cut.Find("button:contains('GUARDAR')").Click();

        // Assert
        Assert.Contains("El teléfono debe tener el formato ####-####", cut.Markup);
    }


    /// <summary>
    /// Helper method to create a test user with valid data.
    /// </summary>
    private UserWithPerson CreateTestUser()
    {
        return new UserWithPerson
        (
            UserName.Create("juan_perez"),
            "Juan",
            "Pérez",
            Email.Create("test@example.com"),
            Phone.Create("1234-5678"),
            IdentityNumber.Create("1-1234-5678"),
            BirthDate.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25))),
            new List<string> { "User" });
    }
}