using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.DependencyInjection;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient("AuthenticatedApiHttpClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("ApiBaseUrl").Value!);
});


await builder.Build().RunAsync();
