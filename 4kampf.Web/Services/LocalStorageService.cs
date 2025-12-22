namespace _4kampf.Web.Services;

/// <summary>
/// Local file system storage implementation.
/// Used for development and non-Heroku deployments.
/// </summary>
public class LocalStorageService : IStorageService
{
    private readonly ILogger<LocalStorageService>? _logger;
    private readonly string _baseDirectory;

    public string StorageType => "Local File System";

    public LocalStorageService(ILogger<LocalStorageService>? logger = null)
    {
        _logger = logger;
        
        // Use environment variable for custom path, or default to wwwroot/projects
        var customPath = Environment.GetEnvironmentVariable("PROJECTS_DIRECTORY");
        if (!string.IsNullOrEmpty(customPath))
        {
            _baseDirectory = customPath;
        }
        else
        {
            _baseDirectory = Path.Combine("wwwroot", "projects");
        }
        
        // Ensure directory exists
        if (!Directory.Exists(_baseDirectory))
        {
            try
            {
                Directory.CreateDirectory(_baseDirectory);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to create projects directory: {Directory}", _baseDirectory);
                // Fallback to temp directory if default fails
                _baseDirectory = Path.Combine(Path.GetTempPath(), "4kampf-projects");
                Directory.CreateDirectory(_baseDirectory);
                _logger?.LogWarning("Using temporary directory for projects: {Directory}", _baseDirectory);
            }
        }
        
        _logger?.LogInformation("Local storage initialized: {Directory}", _baseDirectory);
    }

    public async Task<bool> SaveTextAsync(string path, string content)
    {
        try
        {
            string fullPath = GetFullPath(path);
            string? directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            await File.WriteAllTextAsync(fullPath, content);
            _logger?.LogDebug("Saved file: {Path}", fullPath);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error saving file: {Path}", path);
            return false;
        }
    }

    public async Task<string?> LoadTextAsync(string path)
    {
        try
        {
            string fullPath = GetFullPath(path);
            if (!File.Exists(fullPath))
            {
                _logger?.LogDebug("File not found: {Path}", fullPath);
                return null;
            }
            
            string content = await File.ReadAllTextAsync(fullPath);
            _logger?.LogDebug("Loaded file: {Path}", fullPath);
            return content;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading file: {Path}", path);
            return null;
        }
    }

    public Task<bool> FileExistsAsync(string path)
    {
        string fullPath = GetFullPath(path);
        return Task.FromResult(File.Exists(fullPath));
    }

    public Task<IEnumerable<string>> ListFilesAsync(string directory, string pattern)
    {
        try
        {
            string fullPath = GetFullPath(directory);
            if (!Directory.Exists(fullPath))
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }
            
            var files = Directory.GetFiles(fullPath, pattern)
                .Select(Path.GetFileName)
                .Where(name => !string.IsNullOrEmpty(name))
                .Cast<string>();
            
            return Task.FromResult(files);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error listing files in: {Directory}", directory);
            return Task.FromResult(Enumerable.Empty<string>());
        }
    }

    public async Task<bool> DeleteFileAsync(string path)
    {
        try
        {
            string fullPath = GetFullPath(path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger?.LogDebug("Deleted file: {Path}", fullPath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error deleting file: {Path}", path);
            return false;
        }
    }

    private string GetFullPath(string path)
    {
        // If path is already absolute, return as-is
        if (Path.IsPathRooted(path))
        {
            return path;
        }
        
        // Otherwise, combine with base directory
        return Path.Combine(_baseDirectory, path);
    }
}

