using _4kampf.Shared.Models;

namespace _4kampf.Shared.Utilities;

/// <summary>
/// Extension methods for Project model conversions and utilities.
/// </summary>
public static class ProjectExtensions
{
    /// <summary>
    /// Converts a Synth enum value to a string identifier.
    /// </summary>
    public static string ToStringIdentifier(this Synth synth)
    {
        return synth switch
        {
            Synth.Sointu => "sointu",
            Synth.Vierklang => "4klang",
            Synth.Clinkster => "clinkster",
            Synth.Oidos => "oidos",
            _ => "sointu"
        };
    }

    /// <summary>
    /// Converts a string identifier to a Synth enum value.
    /// </summary>
    public static Synth ToSynthEnum(this string identifier)
    {
        return identifier.ToLowerInvariant() switch
        {
            "sointu" => Synth.Sointu,
            "4klang" or "vierklang" => Synth.Vierklang,
            "clinkster" => Synth.Clinkster,
            "oidos" => Synth.Oidos,
            _ => Synth.Sointu
        };
    }
}

