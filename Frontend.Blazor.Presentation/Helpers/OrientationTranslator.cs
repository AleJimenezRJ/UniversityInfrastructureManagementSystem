using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Helpers;
public static class OrientationTranslator
{
    private static readonly Dictionary<string, string> OrientationToSpanish = new()
    {
        ["North"] = "Norte",
        ["South"] = "Sur",
        ["East"] = "Este",
        ["West"] = "Oeste"
    };

    public static string ToSpanish(string orientation) =>
        OrientationToSpanish.TryGetValue(orientation, out var result) ? result : orientation;

    public static IReadOnlyList<string> AllOrientations => OrientationToSpanish.Keys.ToList();
}
