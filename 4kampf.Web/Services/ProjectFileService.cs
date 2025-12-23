using System.Text.Json;
using _4kampf.Shared.Models;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for managing project files, including Sointu YAML song files.
/// Uses IStorageService abstraction to support both S3 (Heroku) and local storage (development).
/// </summary>
public class ProjectFileService
{
    private readonly ILogger<ProjectFileService>? _logger;
    private readonly IStorageService _storage;

    public ProjectFileService(IStorageService storage, ILogger<ProjectFileService>? logger = null)
    {
        _logger = logger;
        _storage = storage;
        
        _logger?.LogInformation("ProjectFileService initialized with storage: {StorageType}", _storage.StorageType);
    }

    /// <summary>
    /// Saves a project to a JSON file.
    /// </summary>
    public async Task<bool> SaveProjectAsync(Project project, string? projectPath = null)
    {
        try
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                projectPath = $"{project.Name}/{project.Name}.json";
            }

            project.ModifiedAt = DateTime.UtcNow;
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string json = JsonSerializer.Serialize(project, options);
            bool success = await _storage.SaveTextAsync(projectPath, json);
            
            if (success)
            {
                _logger?.LogInformation("Project saved: {ProjectPath} ({StorageType})", projectPath, _storage.StorageType);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error saving project");
            return false;
        }
    }

    /// <summary>
    /// Loads a project from a JSON file.
    /// </summary>
    public async Task<Project?> LoadProjectAsync(string projectPath)
    {
        try
        {
            string? json = await _storage.LoadTextAsync(projectPath);
            if (json == null)
            {
                _logger?.LogWarning("Project file not found: {ProjectPath}", projectPath);
                return null;
            }
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var project = JsonSerializer.Deserialize<Project>(json, options);
            
            if (project != null)
            {
                _logger?.LogInformation("Project loaded: {ProjectPath} ({StorageType})", projectPath, _storage.StorageType);
            }
            
            return project;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading project");
            return null;
        }
    }

    /// <summary>
    /// Lists all available projects.
    /// </summary>
    public async Task<IEnumerable<string>> ListProjectsAsync()
    {
        try
        {
            var files = await _storage.ListFilesAsync(".", "*.json");
            return files.Select(f => Path.GetFileNameWithoutExtension(f))
                .Where(name => !string.IsNullOrEmpty(name))
                .Cast<string>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error listing projects");
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Saves a Sointu YAML song file to the project directory.
    /// </summary>
    public async Task<bool> SaveSointuSongAsync(string projectName, string yamlContent, string? fileName = null)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "song.yml";
            }

            string filePath = $"{projectName}/{fileName}";
            bool success = await _storage.SaveTextAsync(filePath, yamlContent);
            
            if (success)
            {
                _logger?.LogInformation("Sointu song saved: {FilePath} ({StorageType})", filePath, _storage.StorageType);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error saving Sointu song");
            return false;
        }
    }

    /// <summary>
    /// Loads a Sointu YAML song file from the project directory.
    /// </summary>
    public async Task<string?> LoadSointuSongAsync(string projectName, string? fileName = null)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                // Try to find any .yml file in the project directory
                var ymlFiles = await _storage.ListFilesAsync(projectName, "*.yml");
                if (ymlFiles.Any())
                {
                    fileName = ymlFiles.First();
                }
                else
                {
                    return null;
                }
            }

            string filePath = $"{projectName}/{fileName}";
            string? content = await _storage.LoadTextAsync(filePath);
            
            if (content != null)
            {
                _logger?.LogInformation("Sointu song loaded: {FilePath} ({StorageType})", filePath, _storage.StorageType);
            }
            
            return content;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading Sointu song");
            return null;
        }
    }

    /// <summary>
    /// Gets the storage path for a Sointu song file (for compatibility with SointuService).
    /// Note: For S3, this returns a logical path, not a physical file path.
    /// </summary>
    public string GetSointuSongPath(string projectName, string? fileName = null)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "song.yml";
        }
        
        return $"{projectName}/{fileName}";
    }

    /// <summary>
    /// Checks if a Sointu song file exists for a project.
    /// </summary>
    public async Task<bool> HasSointuSongAsync(string projectName, string? fileName = null)
    {
        string path = GetSointuSongPath(projectName, fileName);
        return await _storage.FileExistsAsync(path);
    }

    /// <summary>
    /// Creates a new project with default settings.
    /// </summary>
    public Project CreateNewProject(string name)
    {
        return new Project
        {
            Name = name,
            Synth = Synth.Sointu,
            EnableStandardUniforms = true,
            EnableCamControls = true,
            UsePostProcess = true,
            UseVertexShader = true,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Gets the project directory path (logical path for storage).
    /// For local storage, this is relative to the base directory.
    /// For S3, this is a prefix path.
    /// </summary>
    public string GetProjectDirectory(string projectName)
    {
        return projectName;
    }

    /// <summary>
    /// Lists all Sointu YAML song files in a project.
    /// </summary>
    public async Task<IEnumerable<string>> ListSointuSongsAsync(string projectName)
    {
        try
        {
            var files = await _storage.ListFilesAsync(projectName, "*.yml");
            return files.Select(f => Path.GetFileName(f))
                .Where(name => !string.IsNullOrEmpty(name))
                .Cast<string>()
                .OrderBy(name => name);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error listing Sointu songs");
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Deletes a Sointu YAML song file from the project directory.
    /// </summary>
    public async Task<bool> DeleteSointuSongAsync(string projectName, string fileName)
    {
        try
        {
            string filePath = $"{projectName}/{fileName}";
            bool success = await _storage.DeleteFileAsync(filePath);
            
            if (success)
            {
                _logger?.LogInformation("Sointu song deleted: {FilePath} ({StorageType})", filePath, _storage.StorageType);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error deleting Sointu song");
            return false;
        }
    }
}
