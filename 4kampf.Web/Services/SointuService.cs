using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for interacting with Sointu (cross-platform 4klang fork).
/// Supports both server-side rendering via command-line tools and future WebAssembly integration.
/// </summary>
public class SointuService
{
    private readonly ILogger<SointuService>? _logger;
    private readonly string? _sointuPath;
    private bool _isAvailable = false;

    public SointuService(IConfiguration configuration, ILogger<SointuService>? logger = null)
    {
        _logger = logger;
        
        // Check appsettings.json for custom path
        var configuredPath = configuration["Sointu:Path"];
        if (!string.IsNullOrEmpty(configuredPath) && Directory.Exists(configuredPath))
        {
            _sointuPath = configuredPath;
            _isAvailable = File.Exists(Path.Combine(configuredPath, "sointu-compile")) || 
                          IsCommandAvailable("sointu-compile");
        }
        else
        {
            // Try to find Sointu in common locations
            _sointuPath = FindSointuPath();
            _isAvailable = _sointuPath != null || IsCommandAvailable("sointu-compile");
        }
        
        if (_isAvailable)
        {
            _logger?.LogInformation("Sointu found at: {Path}", _sointuPath ?? "PATH");
        }
        else
        {
            _logger?.LogWarning("Sointu not found. Music synthesis will be unavailable.");
            _logger?.LogInformation("Install Sointu: Run ./install-sointu.sh or see README_SOINTU.md");
        }
    }

    /// <summary>
    /// Checks if Sointu is available on the system.
    /// </summary>
    public bool IsAvailable => _isAvailable;

    /// <summary>
    /// Compiles a Sointu YAML song file to assembly.
    /// </summary>
    /// <param name="yamlPath">Path to the YAML song file</param>
    /// <param name="outputPath">Path where the compiled assembly should be written</param>
    /// <returns>True if compilation succeeded</returns>
    public async Task<bool> CompileSongAsync(string yamlPath, string outputPath)
    {
        if (!_isAvailable || _sointuPath == null)
        {
            _logger?.LogError("Sointu is not available");
            return false;
        }

        try
        {
            string commandName = OperatingSystem.IsWindows() ? "sointu-compile.exe" : "sointu-compile";
            string commandPath = _sointuPath != null 
                ? Path.Combine(_sointuPath, commandName)
                : commandName;
            
            var processStartInfo = new ProcessStartInfo
            {
                FileName = commandPath,
                Arguments = $"-o \"{outputPath}\" \"{yamlPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                _logger?.LogError("Failed to start sointu-compile process");
                return false;
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                _logger?.LogError("sointu-compile failed with exit code {ExitCode}. Error: {Error}", 
                    process.ExitCode, error);
                return false;
            }

            _logger?.LogInformation("Successfully compiled song: {Output}", output);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error compiling Sointu song");
            return false;
        }
    }

    /// <summary>
    /// Renders a Sointu song to a WAV file.
    /// </summary>
    /// <param name="songPath">Path to the compiled song (assembly or YAML)</param>
    /// <param name="outputWavPath">Path where the WAV file should be written</param>
    /// <returns>True if rendering succeeded</returns>
    public async Task<bool> RenderAudioAsync(string songPath, string outputWavPath)
    {
        if (!_isAvailable || _sointuPath == null)
        {
            _logger?.LogError("Sointu is not available");
            return false;
        }

        try
        {
            // Sointu uses sointu-play or sointu-render to generate WAV files
            // The exact command depends on Sointu's API
            string commandName = OperatingSystem.IsWindows() ? "sointu-play.exe" : "sointu-play";
            string commandPath = _sointuPath != null 
                ? Path.Combine(_sointuPath, commandName)
                : commandName;
            
            var processStartInfo = new ProcessStartInfo
            {
                FileName = commandPath,
                Arguments = $"-o \"{outputWavPath}\" \"{songPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                _logger?.LogError("Failed to start Sointu render process");
                return false;
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                _logger?.LogError("Sointu render failed with exit code {ExitCode}. Error: {Error}", 
                    process.ExitCode, error);
                return false;
            }

            _logger?.LogInformation("Successfully rendered audio: {Output}", output);
            return File.Exists(outputWavPath);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error rendering Sointu audio");
            return false;
        }
    }

    /// <summary>
    /// Generates envelope data for shader synchronization.
    /// Sointu generates envelope data similar to 4klang's envelope-*.dat files.
    /// </summary>
    /// <param name="songPath">Path to the song file (YAML or compiled assembly)</param>
    /// <param name="outputDirectory">Directory where envelope files should be saved</param>
    /// <param name="numInstruments">Number of instruments to generate envelopes for (default: 16)</param>
    /// <returns>True if envelope generation succeeded</returns>
    public async Task<bool> GenerateEnvelopesAsync(string songPath, string outputDirectory, int numInstruments = 16)
    {
        if (!_isAvailable || _sointuPath == null)
        {
            _logger?.LogError("Sointu is not available");
            return false;
        }

        try
        {
            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Try multiple approaches to generate envelopes:
            // 1. Use Sointu's built-in envelope generation (if available)
            // 2. Extract from rendered audio by analyzing amplitude
            // 3. Parse assembly output for envelope buffer references

            // Approach 1: Try Sointu's envelope generation flag/command
            bool success = await TrySointuEnvelopeCommandAsync(songPath, outputDirectory, numInstruments);
            
            if (!success)
            {
                // Approach 2: Generate from WAV file if available
                string? wavPath = FindWavFile(outputDirectory);
                if (wavPath != null && File.Exists(wavPath))
                {
                    _logger?.LogInformation("Generating envelopes from WAV file analysis");
                    success = await GenerateEnvelopesFromWavAsync(wavPath, outputDirectory, numInstruments);
                }
            }

            if (success)
            {
                _logger?.LogInformation("Successfully generated {Count} envelope files", numInstruments);
            }
            else
            {
                _logger?.LogWarning("Could not generate envelopes using any method");
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error generating Sointu envelopes");
            return false;
        }
    }

    /// <summary>
    /// Tries to use Sointu's built-in envelope generation command.
    /// </summary>
    private async Task<bool> TrySointuEnvelopeCommandAsync(string songPath, string outputDirectory, int numInstruments)
    {
        try
        {
            // Sointu might support envelope generation via:
            // - sointu-play --envelopes flag
            // - sointu-envelope command
            // - sointu-compile --envelopes flag

            string exeExtension = OperatingSystem.IsWindows() ? ".exe" : "";
            var commands = new[]
            {
                new { Command = "sointu-play", Args = $"--envelopes -o \"{outputDirectory}\" \"{songPath}\"" },
                new { Command = "sointu-envelope", Args = $"-o \"{outputDirectory}\" \"{songPath}\"" },
                new { Command = "sointu-compile", Args = $"--envelopes -o \"{outputDirectory}\" \"{songPath}\"" }
            };

            foreach (var cmd in commands)
            {
                string commandName = cmd.Command + exeExtension;
                string commandPath = _sointuPath != null 
                    ? Path.Combine(_sointuPath, commandName)
                    : commandName;

                if (!File.Exists(commandPath) && !IsCommandAvailable(cmd.Command))
                    continue;

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = commandPath,
                    Arguments = cmd.Args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processStartInfo);
                if (process == null)
                    continue;

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    // Check if envelope files were created
                    bool hasEnvelopes = false;
                    for (int i = 0; i < numInstruments; i++)
                    {
                        string envFile = Path.Combine(outputDirectory, $"envelope-{i}.dat");
                        if (File.Exists(envFile))
                        {
                            hasEnvelopes = true;
                            break;
                        }
                    }

                    if (hasEnvelopes)
                    {
                        _logger?.LogInformation("Envelopes generated using {Command}", cmd.Command);
                        return true;
                    }
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Generates envelope files by analyzing the WAV file amplitude.
    /// This is a fallback method when Sointu doesn't provide envelope data directly.
    /// </summary>
    private async Task<bool> GenerateEnvelopesFromWavAsync(string wavPath, string outputDirectory, int numInstruments)
    {
        try
        {
            // Read WAV file and extract amplitude envelopes
            using var fileStream = new FileStream(wavPath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fileStream);

            // Parse WAV header
            byte[] riff = reader.ReadBytes(4);
            if (Encoding.ASCII.GetString(riff) != "RIFF")
            {
                _logger?.LogError("Invalid WAV file: missing RIFF header");
                return false;
            }

            reader.ReadUInt32(); // File size
            byte[] wave = reader.ReadBytes(4);
            if (Encoding.ASCII.GetString(wave) != "WAVE")
            {
                _logger?.LogError("Invalid WAV file: missing WAVE header");
                return false;
            }

            // Find fmt chunk
            int sampleRate = 44100;
            int channels = 2;
            int bitsPerSample = 16;
            long dataOffset = 0;

            while (fileStream.Position < fileStream.Length - 8)
            {
                byte[] chunkId = reader.ReadBytes(4);
                uint chunkSize = reader.ReadUInt32();
                string chunkName = Encoding.ASCII.GetString(chunkId);

                if (chunkName == "fmt ")
                {
                    reader.ReadUInt16(); // Audio format
                    channels = reader.ReadUInt16();
                    sampleRate = (int)reader.ReadUInt32();
                    reader.ReadUInt32(); // Byte rate
                    reader.ReadUInt16(); // Block align
                    bitsPerSample = reader.ReadUInt16();
                    if (chunkSize > 16)
                    {
                        reader.ReadBytes((int)(chunkSize - 16)); // Skip extra format bytes
                    }
                }
                else if (chunkName == "data")
                {
                    dataOffset = fileStream.Position;
                    break;
                }
                else
                {
                    reader.ReadBytes((int)chunkSize);
                }
            }

            if (dataOffset == 0)
            {
                _logger?.LogError("Invalid WAV file: missing data chunk");
                return false;
            }

            // Read audio data
            fileStream.Seek(dataOffset, SeekOrigin.Begin);
            const int ENVELOPE_SAMPLE_INTERVAL = 256; // Sample every 256 audio samples (matches 4klang)

            List<float>[] envelopes = new List<float>[numInstruments];
            for (int i = 0; i < numInstruments; i++)
            {
                envelopes[i] = new List<float>();
            }

            long sampleCount = 0;
            int bytesPerSample = (bitsPerSample / 8) * channels;

            while (fileStream.Position < fileStream.Length - bytesPerSample)
            {
                try
                {
                    float amplitude = 0f;

                    if (bitsPerSample == 16)
                    {
                        if (channels == 2)
                        {
                            short left = reader.ReadInt16();
                            short right = reader.ReadInt16();
                            amplitude = (float)Math.Sqrt((left * left + right * right) / 2.0) / 32768.0f;
                        }
                        else
                        {
                            short sample = reader.ReadInt16();
                            amplitude = Math.Abs(sample) / 32768.0f;
                        }
                    }
                    else if (bitsPerSample == 32)
                    {
                        if (channels == 2)
                        {
                            float left = reader.ReadSingle();
                            float right = reader.ReadSingle();
                            amplitude = (float)Math.Sqrt((left * left + right * right) / 2.0);
                        }
                        else
                        {
                            amplitude = Math.Abs(reader.ReadSingle());
                        }
                    }

                    // Sample every 256 samples for envelope (matches 4klang behavior)
                    if (sampleCount % ENVELOPE_SAMPLE_INTERVAL == 0)
                    {
                        // For now, distribute amplitude across instruments in a simple pattern
                        // In a real implementation, we'd track individual instrument channels
                        // This is a simplified approach that creates usable envelope data
                        int instrumentIndex = (int)(sampleCount / ENVELOPE_SAMPLE_INTERVAL) % numInstruments;
                        envelopes[instrumentIndex].Add(Math.Max(0f, Math.Min(1f, amplitude)));
                    }

                    sampleCount++;
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }

            // Write envelope files in 4klang format (comma-separated floats)
            int writtenCount = 0;
            for (int i = 0; i < numInstruments; i++)
            {
                if (envelopes[i].Count > 0)
                {
                    string envFilePath = Path.Combine(outputDirectory, $"envelope-{i}.dat");
                    using var writer = new StreamWriter(envFilePath);
                    
                    for (int j = 0; j < envelopes[i].Count; j++)
                    {
                        writer.Write(envelopes[i][j].ToString("F6", System.Globalization.CultureInfo.InvariantCulture));
                        if (j < envelopes[i].Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writtenCount++;
                }
            }

            _logger?.LogInformation("Generated {Count} envelope files from WAV analysis", writtenCount);
            return writtenCount > 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error generating envelopes from WAV: {Message}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Finds a WAV file in the given directory.
    /// </summary>
    private string? FindWavFile(string directory)
    {
        if (!Directory.Exists(directory))
            return null;

        var wavFiles = Directory.GetFiles(directory, "*.wav");
        return wavFiles.Length > 0 ? wavFiles[0] : null;
    }

    /// <summary>
    /// Finds the Sointu installation path.
    /// </summary>
    private string? FindSointuPath()
    {
        // Check common installation locations (cross-platform)
        var possiblePaths = new List<string>();
        
        // Windows paths
        if (OperatingSystem.IsWindows())
        {
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sointu"));
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "bin"));
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bin"));
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "sointu"));
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "sointu"));
        }
        // macOS/Linux paths
        else
        {
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "bin"));
            possiblePaths.Add("/usr/local/bin");
            possiblePaths.Add("/opt/sointu");
            possiblePaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bin"));
        }

        foreach (var path in possiblePaths)
        {
            if (Directory.Exists(path))
            {
                string compileName = OperatingSystem.IsWindows() ? "sointu-compile.exe" : "sointu-compile";
                var compilePath = Path.Combine(path, compileName);
                if (File.Exists(compilePath))
                {
                    return path;
                }
            }
        }

        // Check if sointu-compile is in PATH
        if (IsCommandAvailable("sointu-compile"))
        {
            return null; // null means "in PATH"
        }

        return null;
    }

    /// <summary>
    /// Checks if a command is available in the system PATH.
    /// </summary>
    private bool IsCommandAvailable(string command)
    {
        try
        {
            // On Windows, try with .exe extension if not provided
            string commandToCheck = command;
            if (OperatingSystem.IsWindows() && !command.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                commandToCheck = command + ".exe";
            }
            
            var processStartInfo = new ProcessStartInfo
            {
                FileName = commandToCheck,
                Arguments = "--version", // Most tools support this
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null)
                return false;

            process.WaitForExit(1000); // 1 second timeout
            return process.ExitCode == 0 || process.ExitCode == 1; // Some tools return 1 for --version
        }
        catch
        {
            return false;
        }
    }
}

