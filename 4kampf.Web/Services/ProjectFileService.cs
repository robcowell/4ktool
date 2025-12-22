using System.Text.Json;
using _4kampf.Web.Models;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for managing project files, including Sointu YAML song files.
/// </summary>
public class ProjectFileService
{
    private readonly ILogger<ProjectFileService>? _logger;
    private readonly string _projectsDirectory;

    public ProjectFileService(ILogger<ProjectFileService>? logger = null)
    {
        _logger = logger;
        
        // In a web app, projects could be stored in:
        // 1. Server file system (for server-side storage)
        // 2. Browser localStorage (for client-side storage)
        // 3. Database (for multi-user scenarios)
        
        // For now, we'll use a projects directory relative to wwwroot
        _projectsDirectory = Path.Combine("wwwroot", "projects");
        
        // Ensure directory exists
        if (!Directory.Exists(_projectsDirectory))
        {
            Directory.CreateDirectory(_projectsDirectory);
        }
    }

    /// <summary>
    /// Saves a project to a JSON file.
    /// </summary>
    public async Task<bool> SaveProjectAsync(ProjectModel project, string? projectPath = null)
    {
        try
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                projectPath = Path.Combine(_projectsDirectory, $"{project.Name}.json");
            }

            project.ModifiedAt = DateTime.UtcNow;
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string json = JsonSerializer.Serialize(project, options);
            await File.WriteAllTextAsync(projectPath, json);
            
            _logger?.LogInformation("Project saved: {ProjectPath}", projectPath);
            return true;
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
    public async Task<ProjectModel?> LoadProjectAsync(string projectPath)
    {
        try
        {
            if (!File.Exists(projectPath))
            {
                _logger?.LogWarning("Project file not found: {ProjectPath}", projectPath);
                return null;
            }

            string json = await File.ReadAllTextAsync(projectPath);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var project = JsonSerializer.Deserialize<ProjectModel>(json, options);
            
            if (project != null)
            {
                _logger?.LogInformation("Project loaded: {ProjectPath}", projectPath);
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
    public IEnumerable<string> ListProjects()
    {
        if (!Directory.Exists(_projectsDirectory))
            return Enumerable.Empty<string>();

        return Directory.GetFiles(_projectsDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => !string.IsNullOrEmpty(name))
            .Cast<string>();
    }

    /// <summary>
    /// Saves a Sointu YAML song file to the project directory.
    /// </summary>
    public async Task<bool> SaveSointuSongAsync(string projectName, string yamlContent, string? fileName = null)
    {
        try
        {
            string projectDir = Path.Combine(_projectsDirectory, projectName);
            if (!Directory.Exists(projectDir))
            {
                Directory.CreateDirectory(projectDir);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "song.yml";
            }

            string filePath = Path.Combine(projectDir, fileName);
            await File.WriteAllTextAsync(filePath, yamlContent);
            
            _logger?.LogInformation("Sointu song saved: {FilePath}", filePath);
            return true;
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
            string projectDir = Path.Combine(_projectsDirectory, projectName);
            
            if (string.IsNullOrEmpty(fileName))
            {
                // Try to find any .yml file in the project directory
                var ymlFiles = Directory.GetFiles(projectDir, "*.yml");
                if (ymlFiles.Length > 0)
                {
                    fileName = Path.GetFileName(ymlFiles[0]);
                }
                else
                {
                    return null;
                }
            }

            string filePath = Path.Combine(projectDir, fileName);
            if (!File.Exists(filePath))
            {
                _logger?.LogWarning("Sointu song file not found: {FilePath}", filePath);
                return null;
            }

            string content = await File.ReadAllTextAsync(filePath);
            _logger?.LogInformation("Sointu song loaded: {FilePath}", filePath);
            return content;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading Sointu song");
            return null;
        }
    }

    /// <summary>
    /// Gets the full path to a Sointu song file.
    /// </summary>
    public string GetSointuSongPath(string projectName, string? fileName = null)
    {
        string projectDir = Path.Combine(_projectsDirectory, projectName);
        
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "song.yml";
        }
        
        return Path.Combine(projectDir, fileName);
    }

    /// <summary>
    /// Checks if a Sointu song file exists for a project.
    /// </summary>
    public bool HasSointuSong(string projectName, string? fileName = null)
    {
        string path = GetSointuSongPath(projectName, fileName);
        return File.Exists(path);
    }

    /// <summary>
    /// Creates a new project with default settings.
    /// </summary>
    public ProjectModel CreateNewProject(string name)
    {
        return new ProjectModel
        {
            Name = name,
            Synth = "sointu",
            EnableStandardUniforms = true,
            EnableCamControls = true,
            UsePostProcess = true,
            UseVertexShader = true,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
}

