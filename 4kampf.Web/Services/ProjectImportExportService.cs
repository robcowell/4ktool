using System.Xml.Serialization;
using System.Text;
using System.Text.Json;
using _4kampf.Shared.Models;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for importing and exporting projects between different formats.
/// Supports:
/// - Import from original .kml (XML) format
/// - Export to Visual Studio project format (.vcxproj)
/// </summary>
public class ProjectImportExportService
{
    private readonly ILogger<ProjectImportExportService>? _logger;

    public ProjectImportExportService(ILogger<ProjectImportExportService>? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Legacy project structure from original Windows app (for XML deserialization)
    /// </summary>
    [XmlRoot("Project")]
    public class LegacyProject
    {
        [XmlElement("enableStandardUniforms")]
        public bool EnableStandardUniforms { get; set; }

        [XmlElement("useClinkster")]
        public bool UseClinkster { get; set; }

        [XmlElement("synth")]
        public string Synth { get; set; } = "undefined";

        [XmlElement("use4klangEnv")]
        public bool Use4klangEnv { get; set; }

        [XmlElement("usePP")]
        public bool UsePP { get; set; }

        [XmlElement("useSoundThread")]
        public bool UseSoundThread { get; set; }

        [XmlElement("useBitBucket")]
        public bool UseBitBucket { get; set; }

        [XmlElement("useVertShader")]
        public bool UseVertShader { get; set; } = true;

        [XmlElement("gitRemote")]
        public string? GitRemote { get; set; }

        [XmlElement("name")]
        public string? Name { get; set; }
    }

    /// <summary>
    /// Imports a project from the original .kml (XML) format.
    /// </summary>
    /// <param name="kmlContent">XML content from .kml file</param>
    /// <returns>Converted Project object, or null if import failed</returns>
    public Project? ImportFromKml(string kmlContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(kmlContent))
            {
                _logger?.LogWarning("Attempted to import empty .kml content");
                return null;
            }
            
            using var reader = new StringReader(kmlContent);
            var serializer = new XmlSerializer(typeof(LegacyProject));
            var legacyProject = (LegacyProject?)serializer.Deserialize(reader);

            if (legacyProject == null)
            {
                _logger?.LogWarning("Failed to deserialize .kml file - XML structure may be invalid");
                return null;
            }

            // Convert legacy project to new format
            var project = new Project
            {
                Name = !string.IsNullOrWhiteSpace(legacyProject.Name) 
                    ? legacyProject.Name 
                    : "Imported Project",
                EnableStandardUniforms = legacyProject.EnableStandardUniforms,
                UsePostProcess = legacyProject.UsePP,
                UseVertexShader = legacyProject.UseVertShader,
                GitRemote = legacyProject.GitRemote,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                // Set defaults for web version
                EnableCamControls = true,
                UseWasmRendering = true
            };

            // Convert synth enum with better validation
            project.Synth = legacyProject.Synth?.ToLower() switch
            {
                "vierklang" or "4klang" => Synth.Vierklang,
                "clinkster" => Synth.Clinkster,
                "oidos" => Synth.Oidos,
                "sointu" => Synth.Sointu,
                _ => legacyProject.UseClinkster ? Synth.Clinkster : Synth.Sointu // Default to Sointu for web
            };

            // Convert envelope sync
            project.EnableEnvelopeSync = legacyProject.Use4klangEnv;

            // Note: Shader files (frag.glsl, vert.glsl, ppfrag.glsl) are not included in .kml
            // They must be loaded separately or will use defaults

            _logger?.LogInformation("Successfully imported project from .kml: {ProjectName} (Synth: {Synth})", 
                project.Name, project.Synth);
            return project;
        }
        catch (XmlException ex)
        {
            _logger?.LogError(ex, "XML parsing error when importing .kml file");
            return null;
        }
        catch (InvalidOperationException ex)
        {
            _logger?.LogError(ex, "Invalid XML structure when importing .kml file");
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error importing project from .kml");
            return null;
        }
    }

    /// <summary>
    /// Imports a project from .kml file content (async version for file operations).
    /// </summary>
    public async Task<Project?> ImportFromKmlAsync(string kmlContent)
    {
        return await Task.Run(() => ImportFromKml(kmlContent));
    }

    /// <summary>
    /// Exports a project to Visual Studio project format (.vcxproj).
    /// This creates a basic .vcxproj file that can be opened in Visual Studio.
    /// </summary>
    /// <param name="project">Project to export</param>
    /// <param name="outputDirectory">Directory where project files should be created</param>
    /// <returns>Path to the created .vcxproj file, or null if export failed</returns>
    public async Task<string?> ExportToVisualStudioAsync(Project project, string outputDirectory)
    {
        try
        {
            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create basecode directory structure
            string basecodeDir = Path.Combine(outputDirectory, "basecode");
            if (!Directory.Exists(basecodeDir))
            {
                Directory.CreateDirectory(basecodeDir);
            }

            // Save shader files
            await File.WriteAllTextAsync(Path.Combine(outputDirectory, "frag.glsl"), project.FragmentShader);
            await File.WriteAllTextAsync(Path.Combine(outputDirectory, "vert.glsl"), project.VertexShader);
            await File.WriteAllTextAsync(Path.Combine(outputDirectory, "ppfrag.glsl"), project.PostProcessShader);

            // Generate .vcxproj file
            string vcxprojPath = Path.Combine(basecodeDir, $"{project.Name}.vcxproj");
            string vcxprojContent = GenerateVcxprojContent(project);
            await File.WriteAllTextAsync(vcxprojPath, vcxprojContent, Encoding.UTF8);

            // Generate .sln file (solution file)
            string slnPath = Path.Combine(basecodeDir, $"{project.Name}.sln");
            string slnContent = GenerateSlnContent(project);
            await File.WriteAllTextAsync(slnPath, slnContent, Encoding.UTF8);

            _logger?.LogInformation("Successfully exported project to Visual Studio format: {VcxprojPath}", vcxprojPath);
            return vcxprojPath;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error exporting project to Visual Studio format");
            return null;
        }
    }

    /// <summary>
    /// Generates the content for a .vcxproj file.
    /// </summary>
    private string GenerateVcxprojContent(Project project)
    {
        var guid = Guid.NewGuid().ToString("B").ToUpper();
        var projectName = SanitizeFileName(project.Name);

        return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project DefaultTargets=""Build"" ToolsVersion=""14.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <ItemGroup Label=""ProjectConfigurations"">
    <ProjectConfiguration Include=""Debug|Win32"">
      <Configuration>Debug</Configuration>
      <Platform>Win32</Platform>
    </ProjectConfiguration>
    <ProjectConfiguration Include=""Packed Release|Win32"">
      <Configuration>Packed Release</Configuration>
      <Platform>Win32</Platform>
    </ProjectConfiguration>
  </ItemGroup>
  <ItemGroup>
    <ClCompile Include=""main_deb.cpp"">
      <ExcludedFromBuild Condition=""'$(Configuration)|$(Platform)'=='Packed Release|Win32'"">true</ExcludedFromBuild>
    </ClCompile>
    <ClCompile Include=""main_rel.cpp"">
      <ExcludedFromBuild Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">true</ExcludedFromBuild>
    </ClCompile>
  </ItemGroup>
  <ItemGroup>
    <ClInclude Include=""..\\intro.h"" />
    <ClInclude Include=""..\\4klang.h"" />
    <ClInclude Include=""glext.h"" />
  </ItemGroup>
  <PropertyGroup Label=""Globals"">
    <ProjectName>{projectName}</ProjectName>
    <ProjectGuid>{guid}</ProjectGuid>
  </PropertyGroup>
  <Import Project=""$(VCTargetsPath)\\Microsoft.Cpp.Default.props"" />
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Packed Release|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseOfMfc>false</UseOfMfc>
    <CharacterSet>NotSet</CharacterSet>
    <PlatformToolset>v142</PlatformToolset>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseOfMfc>false</UseOfMfc>
    <PlatformToolset>v142</PlatformToolset>
  </PropertyGroup>
  <Import Project=""$(VCTargetsPath)\\Microsoft.Cpp.props"" />
  <PropertyGroup>
    <_ProjectFileVersion>10.0.30319.1</_ProjectFileVersion>
    <OutDir Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">.\\exe\\</OutDir>
    <IntDir Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">.\\bin\\</IntDir>
    <OutDir Condition=""'$(Configuration)|$(Platform)'=='Packed Release|Win32'"">.\\exe\\</OutDir>
    <IntDir Condition=""'$(Configuration)|$(Platform)'=='Packed Release|Win32'"">.\\bin\\</IntDir>
  </PropertyGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
    <ClCompile>
      <Optimization>Disabled</Optimization>
      <AdditionalIncludeDirectories>../;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
      <PreprocessorDefinitions>_DEBUG;WINDOWS;DEBUG;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <RuntimeLibrary>MultiThreaded</RuntimeLibrary>
      <WarningLevel>Level3</WarningLevel>
    </ClCompile>
    <Link>
      <AdditionalDependencies>../4klang.obj;opengl32.lib;glu32.lib;winmm.lib;kernel32.lib;user32.lib;gdi32.lib;%(AdditionalDependencies)</AdditionalDependencies>
      <SubSystem>Console</SubSystem>
      <OutputFile>.\\exe\\{projectName}.exe</OutputFile>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Packed Release|Win32'"">
    <ClCompile>
      <Optimization>MinSpace</Optimization>
      <AdditionalIncludeDirectories>../;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
      <PreprocessorDefinitions>WINDOWS;NDEBUG;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <RuntimeLibrary>MultiThreadedDLL</RuntimeLibrary>
      <StructMemberAlignment>1Byte</StructMemberAlignment>
    </ClCompile>
    <Link>
      <AdditionalDependencies>../4klang.obj;opengl32.lib;glu32.lib;winmm.lib;kernel32.lib;user32.lib;gdi32.lib;%(AdditionalDependencies)</AdditionalDependencies>
      <SubSystem>Windows</SubSystem>
      <OutputFile>.\\exe\\{projectName}.exe</OutputFile>
    </Link>
  </ItemDefinitionGroup>
  <Import Project=""$(VCTargetsPath)\\Microsoft.Cpp.targets"" />
</Project>";
    }

    /// <summary>
    /// Generates the content for a .sln (solution) file.
    /// </summary>
    private string GenerateSlnContent(Project project)
    {
        var projectGuid = Guid.NewGuid().ToString("B").ToUpper();
        var solutionGuid = Guid.NewGuid().ToString("B").ToUpper();
        var projectName = SanitizeFileName(project.Name);

        return $@"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}}"") = ""{projectName}"", ""{projectName}.vcxproj"", ""{projectGuid}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Win32 = Debug|Win32
		Packed Release|Win32 = Packed Release|Win32
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{projectGuid}.Debug|Win32.ActiveCfg = Debug|Win32
		{projectGuid}.Debug|Win32.Build.0 = Debug|Win32
		{projectGuid}.Packed Release|Win32.ActiveCfg = Packed Release|Win32
		{projectGuid}.Packed Release|Win32.Build.0 = Packed Release|Win32
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";
    }

    /// <summary>
    /// Sanitizes a file name to be safe for use in file system and Visual Studio.
    /// </summary>
    private string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return "UntitledProject";

        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new StringBuilder();
        
        foreach (var c in fileName)
        {
            if (!invalidChars.Contains(c))
            {
                sanitized.Append(c);
            }
            else
            {
                sanitized.Append('_');
            }
        }

        return sanitized.ToString();
    }

    /// <summary>
    /// Converts a project to JSON format (for web storage).
    /// </summary>
    public string ExportToJson(Project project)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        return JsonSerializer.Serialize(project, options);
    }

    /// <summary>
    /// Converts a project from JSON format (for web storage).
    /// </summary>
    public Project? ImportFromJson(string jsonContent)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Deserialize<Project>(jsonContent, options);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error importing project from JSON");
            return null;
        }
    }
}

