namespace _4kampf.Shared.Models;

/// <summary>
/// Synthesizer type enumeration.
/// </summary>
public enum Synth
{
    /// <summary>
    /// Undefined/not set
    /// </summary>
    Undefined = 0,
    
    /// <summary>
    /// 4klang synthesizer (original, Windows-only)
    /// </summary>
    Vierklang = 1,
    
    /// <summary>
    /// Clinkster synthesizer
    /// </summary>
    Clinkster = 2,
    
    /// <summary>
    /// Oidos synthesizer
    /// </summary>
    Oidos = 3,
    
    /// <summary>
    /// Sointu synthesizer (cross-platform 4klang fork)
    /// </summary>
    Sointu = 4
}

