using Microsoft.Extensions.DependencyInjection;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation;

/// <summary>
/// This class contains extension methods for mapping tools in the presentation layer.
/// </summary>
public static class ToolsMappings
{
    /// <summary>
    /// Maps the tools for the presentation layer using the Model Context Protocol (MCP).
    /// </summary>
    /// <param name="services">The service collection to add the tools to.</param>
    /// <returns>A reference to the same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection MapPresentationLayerMcpTools(this IServiceCollection services)
    {
        services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        return services;
    }
}
