using System.Text.Json.Serialization;

namespace _4kampf.Shared.Models;

/// <summary>
/// BitBucket repository configuration data.
/// </summary>
public class BitBucketData
{
    /// <summary>
    /// Repository slug (name)
    /// </summary>
    [JsonPropertyName("repoSlug")]
    public string RepoSlug { get; set; } = "";

    /// <summary>
    /// BitBucket team/workspace name
    /// </summary>
    [JsonPropertyName("team")]
    public string Team { get; set; } = "";

    /// <summary>
    /// BitBucket username
    /// </summary>
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = "";
}

/// <summary>
/// BitBucket repository descriptor (for listing repos)
/// </summary>
public class RepoDescriptor
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = "";

    [JsonPropertyName("clone")]
    public string Clone { get; set; } = "";

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

