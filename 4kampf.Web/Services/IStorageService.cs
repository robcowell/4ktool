namespace _4kampf.Web.Services;

/// <summary>
/// Storage service abstraction for project files.
/// Supports both AWS S3 (for Heroku) and local file system (for development).
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves text content to a file path.
    /// </summary>
    Task<bool> SaveTextAsync(string path, string content);
    
    /// <summary>
    /// Loads text content from a file path.
    /// </summary>
    Task<string?> LoadTextAsync(string path);
    
    /// <summary>
    /// Checks if a file exists at the given path.
    /// </summary>
    Task<bool> FileExistsAsync(string path);
    
    /// <summary>
    /// Lists all files matching a pattern in a directory.
    /// </summary>
    Task<IEnumerable<string>> ListFilesAsync(string directory, string pattern);
    
    /// <summary>
    /// Deletes a file at the given path.
    /// </summary>
    Task<bool> DeleteFileAsync(string path);
    
    /// <summary>
    /// Gets the storage type name (for logging/debugging).
    /// </summary>
    string StorageType { get; }
}

