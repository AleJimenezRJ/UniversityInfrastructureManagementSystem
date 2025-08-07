using System;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Helpers;

public static class LearningComponentFormatter
{
    /// <summary>
    /// Formats the ID of a learning component based on its type for display purposes.
    /// </summary>
    /// <param name="component"> component to format the ID for.
    /// </param>
    /// <returns></returns>
    public static string GetFormattedId(LearningComponent component)
    {
        return component switch
        {
            Projector p => $"PROJ-{p.ComponentId}",
            Whiteboard w => $"WB-{w.ComponentId}",
            _ => component.ComponentId.ToString()
        };
    }
}