using System;
using System.Text.Json.Serialization;

namespace _4kampf.Shared.Models;

/// <summary>
/// Unified project model for 4kampf applications.
/// Supports both Windows (XML) and Web (JSON) serialization.
/// </summary>
public class Project
{
    /// <summary>
    /// Project name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Untitled Project";

    /// <summary>
    /// Synthesizer type
    /// </summary>
    [JsonPropertyName("synth")]
    public Synth Synth { get; set; } = Synth.Sointu;

    /// <summary>
    /// Sointu YAML song file path (for Sointu synthesizer)
    /// </summary>
    [JsonPropertyName("sointuSongFile")]
    public string? SointuSongFile { get; set; }

    /// <summary>
    /// Enable standard uniforms (vec3 u {resX, resY, time})
    /// </summary>
    [JsonPropertyName("enableStandardUniforms")]
    public bool EnableStandardUniforms { get; set; } = true;

    /// <summary>
    /// Enable camera controls
    /// </summary>
    [JsonPropertyName("enableCamControls")]
    public bool EnableCamControls { get; set; } = true;

    /// <summary>
    /// Enable envelope synchronization (for music-driven visuals)
    /// </summary>
    [JsonPropertyName("enableEnvelopeSync")]
    public bool EnableEnvelopeSync { get; set; } = false;

    /// <summary>
    /// Use post-process shader
    /// </summary>
    [JsonPropertyName("usePostProcess")]
    public bool UsePostProcess { get; set; } = true;

    /// <summary>
    /// Use custom vertex shader (vs default pass-through)
    /// </summary>
    [JsonPropertyName("useVertexShader")]
    public bool UseVertexShader { get; set; } = true;

    /// <summary>
    /// Enable music looping
    /// </summary>
    [JsonPropertyName("enableLooping")]
    public bool EnableLooping { get; set; } = false;

    /// <summary>
    /// Vertex shader code
    /// </summary>
    [JsonPropertyName("vertexShader")]
    public string VertexShader { get; set; } = "";

    /// <summary>
    /// Fragment shader code
    /// </summary>
    [JsonPropertyName("fragmentShader")]
    public string FragmentShader { get; set; } = "";

    /// <summary>
    /// Post-process shader code
    /// </summary>
    [JsonPropertyName("postProcessShader")]
    public string PostProcessShader { get; set; } = "";

    /// <summary>
    /// Project creation timestamp
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Project last modification timestamp
    /// </summary>
    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    // Legacy Windows app properties (for compatibility)
    /// <summary>
    /// Legacy: Use Clinkster (deprecated, use Synth instead)
    /// </summary>
    [JsonIgnore]
    public bool UseClinkster
    {
        get => Synth == Synth.Clinkster;
        set
        {
            if (value && Synth == Synth.Undefined)
            {
                Synth = Synth.Clinkster;
            }
        }
    }

    /// <summary>
    /// Legacy: Use 4klang envelope sync (deprecated, use EnableEnvelopeSync instead)
    /// </summary>
    [JsonIgnore]
    public bool Use4klangEnv
    {
        get => EnableEnvelopeSync && Synth == Synth.Vierklang;
        set => EnableEnvelopeSync = value;
    }

    /// <summary>
    /// Legacy: Use post-process (deprecated, use UsePostProcess instead)
    /// </summary>
    [JsonIgnore]
    public bool UsePP
    {
        get => UsePostProcess;
        set => UsePostProcess = value;
    }

    /// <summary>
    /// Legacy: Use vertex shader (deprecated, use UseVertexShader instead)
    /// </summary>
    [JsonIgnore]
    public bool UseVertShader
    {
        get => UseVertexShader;
        set => UseVertexShader = value;
    }

    /// <summary>
    /// Git remote URL (for Windows app compatibility)
    /// </summary>
    [JsonPropertyName("gitRemote")]
    public string? GitRemote { get; set; }

    /// <summary>
    /// Fixes legacy project data by migrating old properties to new structure.
    /// </summary>
    /// <returns>True if any migration was performed</returns>
    public bool FixLegacy()
    {
        bool changed = false;

        // Migrate undefined synth
        if (Synth == Synth.Undefined)
        {
            Synth = UseClinkster ? Synth.Clinkster : Synth.Vierklang;
            changed = true;
        }

        return changed;
    }
}

