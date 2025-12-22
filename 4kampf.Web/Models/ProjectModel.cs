using System.Text.Json.Serialization;

namespace _4kampf.Web.Models;

/// <summary>
/// Project model for web application.
/// Supports Sointu YAML song files and all original project features.
/// </summary>
public class ProjectModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Untitled Project";

    [JsonPropertyName("synth")]
    public string Synth { get; set; } = "sointu"; // sointu, 4klang, clinkster, oidos

    [JsonPropertyName("sointuSongFile")]
    public string? SointuSongFile { get; set; } // Path to .yml file

    [JsonPropertyName("enableStandardUniforms")]
    public bool EnableStandardUniforms { get; set; } = true;

    [JsonPropertyName("enableCamControls")]
    public bool EnableCamControls { get; set; } = true;

    [JsonPropertyName("enableEnvelopeSync")]
    public bool EnableEnvelopeSync { get; set; } = false;

    [JsonPropertyName("usePostProcess")]
    public bool UsePostProcess { get; set; } = true;

    [JsonPropertyName("useVertexShader")]
    public bool UseVertexShader { get; set; } = true;

    [JsonPropertyName("enableLooping")]
    public bool EnableLooping { get; set; } = false;

    [JsonPropertyName("vertexShader")]
    public string VertexShader { get; set; } = "";

    [JsonPropertyName("fragmentShader")]
    public string FragmentShader { get; set; } = "";

    [JsonPropertyName("postProcessShader")]
    public string PostProcessShader { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("modifiedAt")]
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}

