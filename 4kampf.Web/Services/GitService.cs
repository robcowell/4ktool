using System.Net;
using LibGit2Sharp;
using _4kampf.Shared.Models;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for Git operations using LibGit2Sharp.
/// Handles repository initialization, commits, push, pull, and status.
/// </summary>
public class GitService : IDisposable
{
    private readonly ILogger<GitService>? _logger;
    private readonly IStorageService _storage;
    private Repository? _repository;

    public GitService(IStorageService storage, ILogger<GitService>? logger = null)
    {
        _storage = storage;
        _logger = logger;
    }

    /// <summary>
    /// Initializes a Git repository for a project.
    /// </summary>
    public async Task<bool> InitializeRepositoryAsync(string projectName, string? gitRemote = null)
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            // Check if repository already exists
            if (Repository.IsValid(projectPath))
            {
                _logger?.LogInformation("Git repository already exists for project: {ProjectName}", projectName);
                return true;
            }

            _logger?.LogInformation("Initializing Git repository for project: {ProjectName}", projectName);
            
            // Initialize repository
            Repository.Init(projectPath);
            _repository = new Repository(projectPath);

            // Add all files
            foreach (var file in Directory.GetFiles(projectPath, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(projectPath, file);
                if (!relativePath.StartsWith(".git"))
                {
                    _repository.Index.Add(relativePath);
                }
            }

            // Create initial commit
            var signature = new Signature("4kampf Web", "web@4kampf.local", DateTimeOffset.Now);
            _repository.Commit("Initial commit", signature, signature);

            // Set remote if provided
            if (!string.IsNullOrEmpty(gitRemote))
            {
                _repository.Network.Remotes.Add("origin", gitRemote);
            }

            _logger?.LogInformation("Git repository initialized successfully for project: {ProjectName}", projectName);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error initializing Git repository for project: {ProjectName}", projectName);
            return false;
        }
    }

    /// <summary>
    /// Gets the current Git status for a project.
    /// </summary>
    public async Task<GitStatus> GetStatusAsync(string projectName)
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            if (!Repository.IsValid(projectPath))
            {
                return new GitStatus { IsInitialized = false };
            }

            using var repo = new Repository(projectPath);
            var status = repo.RetrieveStatus();

            return new GitStatus
            {
                IsInitialized = true,
                IsDirty = status.IsDirty,
                ModifiedFiles = status.Modified.Select(f => f.FilePath).ToList(),
                UntrackedFiles = status.Untracked.Select(f => f.FilePath).ToList(),
                HasRemote = repo.Network.Remotes.Any(),
                CurrentBranch = repo.Head.FriendlyName,
                RemoteUrl = repo.Network.Remotes.FirstOrDefault()?.Url
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting Git status for project: {ProjectName}", projectName);
            return new GitStatus { IsInitialized = false, Error = ex.Message };
        }
    }

    /// <summary>
    /// Commits changes to the repository.
    /// </summary>
    public async Task<bool> CommitAsync(string projectName, string message, string authorName = "4kampf Web", string authorEmail = "web@4kampf.local")
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            if (!Repository.IsValid(projectPath))
            {
                _logger?.LogWarning("Cannot commit: Git repository not initialized for project: {ProjectName}", projectName);
                return false;
            }

            using var repo = new Repository(projectPath);
            var status = repo.RetrieveStatus();

            // Add modified files
            foreach (var item in status.Modified)
            {
                repo.Index.Add(item.FilePath);
            }

            // Add untracked files
            foreach (var item in status.Untracked)
            {
                repo.Index.Add(item.FilePath);
            }

            if (repo.Index.Count == 0)
            {
                _logger?.LogInformation("No changes to commit for project: {ProjectName}", projectName);
                return false;
            }

            var signature = new Signature(authorName, authorEmail, DateTimeOffset.Now);
            repo.Commit(message, signature, signature);

            _logger?.LogInformation("Committed changes to project: {ProjectName}", projectName);
            return true;
        }
        catch (EmptyCommitException)
        {
            _logger?.LogInformation("Nothing to commit for project: {ProjectName}", projectName);
            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error committing changes for project: {ProjectName}", projectName);
            return false;
        }
    }

    /// <summary>
    /// Pushes changes to the remote repository.
    /// </summary>
    public async Task<GitOperationResult> PushAsync(string projectName, NetworkCredential? credentials = null)
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            if (!Repository.IsValid(projectPath))
            {
                return new GitOperationResult { Success = false, Message = "Git repository not initialized" };
            }

            using var repo = new Repository(projectPath);
            var remote = repo.Network.Remotes.FirstOrDefault();
            
            if (remote == null)
            {
                return new GitOperationResult { Success = false, Message = "No remote repository configured" };
            }

            var options = new PushOptions();
            if (credentials != null)
            {
                options.CredentialsProvider = (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials
                    {
                        Username = credentials.UserName,
                        Password = credentials.Password
                    };
            }

            repo.Network.Push(remote, repo.Head.CanonicalName, options);

            _logger?.LogInformation("Pushed changes to remote for project: {ProjectName}", projectName);
            return new GitOperationResult { Success = true, Message = "Push successful" };
        }
        catch (NonFastForwardException)
        {
            return new GitOperationResult { Success = false, Message = "Remote has changes. Please pull first." };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error pushing changes for project: {ProjectName}", projectName);
            return new GitOperationResult { Success = false, Message = ex.Message };
        }
    }

    /// <summary>
    /// Pulls changes from the remote repository.
    /// </summary>
    public async Task<GitOperationResult> PullAsync(string projectName, NetworkCredential? credentials = null, string authorName = "4kampf Web", string authorEmail = "web@4kampf.local")
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            if (!Repository.IsValid(projectPath))
            {
                return new GitOperationResult { Success = false, Message = "Git repository not initialized" };
            }

            using var repo = new Repository(projectPath);
            var remote = repo.Network.Remotes.FirstOrDefault();
            
            if (remote == null)
            {
                return new GitOperationResult { Success = false, Message = "No remote repository configured" };
            }

            var options = new PullOptions
            {
                MergeOptions = new MergeOptions
                {
                    CommitOnSuccess = true,
                    FileConflictStrategy = CheckoutFileConflictStrategy.Merge
                }
            };

            if (credentials != null)
            {
                options.FetchOptions = new FetchOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials
                        {
                            Username = credentials.UserName,
                            Password = credentials.Password
                        }
                };
            }

            var signature = new Signature(authorName, authorEmail, DateTimeOffset.Now);
            Commands.Pull(repo, signature, options);

            _logger?.LogInformation("Pulled changes from remote for project: {ProjectName}", projectName);
            return new GitOperationResult { Success = true, Message = "Pull successful" };
        }
        catch (MergeFetchHeadNotFoundException)
        {
            return new GitOperationResult { Success = false, Message = "No changes to pull" };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error pulling changes for project: {ProjectName}", projectName);
            return new GitOperationResult { Success = false, Message = ex.Message };
        }
    }

    /// <summary>
    /// Sets the remote URL for the repository.
    /// </summary>
    public async Task<bool> SetRemoteAsync(string projectName, string remoteUrl, string remoteName = "origin")
    {
        try
        {
            string projectPath = await GetProjectPathAsync(projectName);
            
            if (!Repository.IsValid(projectPath))
            {
                return false;
            }

            using var repo = new Repository(projectPath);
            var existingRemote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
            
            if (existingRemote != null)
            {
                repo.Network.Remotes.Update(remoteName, updater => updater.Url = remoteUrl);
            }
            else
            {
                repo.Network.Remotes.Add(remoteName, remoteUrl);
            }

            _logger?.LogInformation("Set remote '{RemoteName}' to {RemoteUrl} for project: {ProjectName}", 
                remoteName, remoteUrl, projectName);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error setting remote for project: {ProjectName}", projectName);
            return false;
        }
    }

    /// <summary>
    /// Gets the local file system path for a project.
    /// </summary>
    private async Task<string> GetProjectPathAsync(string projectName)
    {
        // For now, assume projects are stored locally
        // In production with S3, we'd need to clone to a temp directory first
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "projects", projectName);
        return basePath;
    }

    public void Dispose()
    {
        _repository?.Dispose();
    }
}

/// <summary>
/// Git repository status information.
/// </summary>
public class GitStatus
{
    public bool IsInitialized { get; set; }
    public bool IsDirty { get; set; }
    public List<string> ModifiedFiles { get; set; } = new();
    public List<string> UntrackedFiles { get; set; } = new();
    public bool HasRemote { get; set; }
    public string? CurrentBranch { get; set; }
    public string? RemoteUrl { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Result of a Git operation.
/// </summary>
public class GitOperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
}

