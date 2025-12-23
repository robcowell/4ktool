using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using _4kampf.Shared.Models;
using System.Text.Json;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for interacting with BitBucket API.
/// Handles repository creation, listing, and authentication.
/// </summary>
public class BitBucketService
{
    private readonly ILogger<BitBucketService>? _logger;
    private const string BitBucketBaseUrl = "https://bitbucket.org/";

    public BitBucketService(ILogger<BitBucketService>? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Creates a new BitBucket repository.
    /// </summary>
    /// <param name="data">BitBucket configuration data</param>
    /// <param name="credentials">Network credentials (username/password)</param>
    /// <returns>Git remote URL if successful, null otherwise</returns>
    public async Task<string?> CreateRepositoryAsync(BitBucketData data, NetworkCredential credentials)
    {
        try
        {
            _logger?.LogInformation("Creating BitBucket repository: {Team}/{RepoSlug}", data.Team, data.RepoSlug);

            var client = new RestClient(new RestClientOptions(BitBucketBaseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password)
            });

            var request = new RestRequest($"api/2.0/repositories/{data.Team}/{data.RepoSlug}", Method.Post);
            request.AddParameter("name", data.RepoSlug);
            request.AddParameter("is_private", "true");
            request.AddParameter("scm", "git");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string remoteUrl = $"https://bitbucket.org/{data.Team}/{data.RepoSlug}.git";
                _logger?.LogInformation("Created BitBucket repository: {RemoteUrl}", remoteUrl);
                return remoteUrl;
            }
            else
            {
                _logger?.LogWarning("Failed to create BitBucket repository. Status: {StatusCode}, Content: {Content}",
                    response.StatusCode, response.Content);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error creating BitBucket repository");
            return null;
        }
    }

    /// <summary>
    /// Lists repositories for a BitBucket team/workspace.
    /// </summary>
    /// <param name="team">Team/workspace name</param>
    /// <param name="credentials">Network credentials</param>
    /// <returns>List of repository descriptors, or null if failed</returns>
    public async Task<List<RepoDescriptor>?> ListRepositoriesAsync(string team, NetworkCredential credentials)
    {
        try
        {
            _logger?.LogInformation("Listing BitBucket repositories for team: {Team}", team);

            var client = new RestClient(new RestClientOptions(BitBucketBaseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password)
            });

            var request = new RestRequest($"api/2.0/repositories/{team}", Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonDoc = JsonDocument.Parse(response.Content!);
                var values = jsonDoc.RootElement.GetProperty("values");

                var repos = new List<RepoDescriptor>();
                foreach (var repo in values.EnumerateArray())
                {
                    var descriptor = new RepoDescriptor
                    {
                        Name = repo.GetProperty("name").GetString() ?? "",
                        Description = repo.TryGetProperty("description", out var desc) ? desc.GetString() : null
                    };

                    // Extract slug from self link
                    if (repo.TryGetProperty("links", out var links) &&
                        links.TryGetProperty("self", out var self) &&
                        self.TryGetProperty("href", out var href))
                    {
                        var hrefStr = href.GetString() ?? "";
                        descriptor.Slug = ExtractSlugFromUrl(hrefStr);
                    }

                    // Extract clone URL
                    if (links.TryGetProperty("clone", out var clone))
                    {
                        foreach (var cloneLink in clone.EnumerateArray())
                        {
                            if (cloneLink.TryGetProperty("name", out var name) &&
                                name.GetString() == "https" &&
                                cloneLink.TryGetProperty("href", out var cloneHref))
                            {
                                descriptor.Clone = cloneHref.GetString() ?? "";
                                break;
                            }
                        }
                    }

                    repos.Add(descriptor);
                }

                _logger?.LogInformation("Found {Count} repositories for team {Team}", repos.Count, team);
                return repos;
            }
            else
            {
                _logger?.LogWarning("Failed to list BitBucket repositories. Status: {StatusCode}",
                    response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error listing BitBucket repositories");
            return null;
        }
    }

    /// <summary>
    /// Validates BitBucket credentials.
    /// </summary>
    public async Task<bool> ValidateCredentialsAsync(string team, NetworkCredential credentials)
    {
        try
        {
            var client = new RestClient(new RestClientOptions(BitBucketBaseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password)
            });

            var request = new RestRequest($"api/2.0/user", Method.Get);
            var response = await client.ExecuteAsync(request);

            return response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts repository slug from BitBucket URL.
    /// </summary>
    private string ExtractSlugFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "";

        var uri = new Uri(url);
        var segments = uri.Segments;
        return segments.Length > 0 ? segments[^1].TrimEnd('/') : "";
    }
}

