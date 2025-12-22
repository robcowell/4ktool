using Microsoft.JSInterop;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for interacting with Sointu WebAssembly module in the browser.
/// Provides real-time music synthesis without server dependency.
/// </summary>
public class SointuWasmService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<SointuWasmService>? _logger;
    private IJSObjectReference? _jsModule;
    private bool _isInitialized = false;

    public SointuWasmService(IJSRuntime jsRuntime, ILogger<SointuWasmService>? logger = null)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    /// <summary>
    /// Initialize the Sointu WASM module.
    /// </summary>
    /// <param name="wasmPath">Path to the Sointu WASM file (e.g., "/wasm/sointu.wasm")</param>
    /// <returns>True if initialization succeeded</returns>
    public async Task<bool> InitializeAsync(string wasmPath = "/wasm/sointu.wasm")
    {
        try
        {
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/sointu-wasm-interop.js");
            var result = await _jsRuntime.InvokeAsync<bool>("sointuWasmInterop.init", wasmPath);
            
            if (result)
            {
                _isInitialized = true;
                _logger?.LogInformation("Sointu WASM module initialized successfully");
            }
            else
            {
                _logger?.LogWarning("Sointu WASM module initialization failed");
            }
            
            return result;
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error initializing Sointu WASM module");
            return false;
        }
    }

    /// <summary>
    /// Check if the WASM module is available and initialized.
    /// </summary>
    public bool IsAvailable => _isInitialized;

    /// <summary>
    /// Load a Sointu YAML song file and compile it for synthesis.
    /// </summary>
    /// <param name="yamlContent">YAML song content</param>
    /// <returns>True if song loaded successfully</returns>
    public async Task<bool> LoadSongAsync(string yamlContent)
    {
        if (!_isInitialized)
        {
            _logger?.LogError("Sointu WASM module not initialized");
            return false;
        }

        try
        {
            var result = await _jsRuntime.InvokeAsync<bool>("sointuWasmInterop.loadSong", yamlContent);
            
            if (result)
            {
                _logger?.LogInformation("Song loaded into Sointu WASM module");
            }
            else
            {
                _logger?.LogWarning("Failed to load song into Sointu WASM module");
            }
            
            return result;
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error loading song into Sointu WASM module");
            return false;
        }
    }

    /// <summary>
    /// Start real-time audio synthesis.
    /// </summary>
    /// <returns>True if playback started</returns>
    public async Task<bool> PlayAsync()
    {
        if (!_isInitialized)
        {
            _logger?.LogError("Sointu WASM module not initialized");
            return false;
        }

        try
        {
            var result = await _jsRuntime.InvokeAsync<bool>("sointuWasmInterop.play");
            if (result)
            {
                _logger?.LogInformation("Sointu WASM playback started");
            }
            return result;
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error starting Sointu WASM playback");
            return false;
        }
    }

    /// <summary>
    /// Stop audio synthesis.
    /// </summary>
    public async Task StopAsync()
    {
        if (!_isInitialized) return;

        try
        {
            await _jsRuntime.InvokeVoidAsync("sointuWasmInterop.stop");
            _logger?.LogInformation("Sointu WASM playback stopped");
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error stopping Sointu WASM playback");
        }
    }

    /// <summary>
    /// Pause audio synthesis.
    /// </summary>
    public async Task PauseAsync()
    {
        if (!_isInitialized) return;

        try
        {
            await _jsRuntime.InvokeVoidAsync("sointuWasmInterop.pause");
            _logger?.LogInformation("Sointu WASM playback paused");
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error pausing Sointu WASM playback");
        }
    }

    /// <summary>
    /// Get current playback position in seconds.
    /// </summary>
    public async Task<double> GetPositionAsync()
    {
        if (!_isInitialized) return 0;

        try
        {
            return await _jsRuntime.InvokeAsync<double>("sointuWasmInterop.getPosition");
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error getting Sointu WASM position");
            return 0;
        }
    }

    /// <summary>
    /// Set playback position in seconds.
    /// </summary>
    public async Task SetPositionAsync(double time)
    {
        if (!_isInitialized) return;

        try
        {
            await _jsRuntime.InvokeVoidAsync("sointuWasmInterop.setPosition", time);
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error setting Sointu WASM position");
        }
    }

    /// <summary>
    /// Get envelope sync data for shader synchronization.
    /// </summary>
    /// <param name="numInstruments">Number of instruments to return</param>
    /// <returns>Envelope values for each instrument, or null if not available</returns>
    public async Task<float[]?> GetEnvelopeSyncAsync(int numInstruments)
    {
        if (!_isInitialized) return null;

        try
        {
            var data = await _jsRuntime.InvokeAsync<double[]>("sointuWasmInterop.getEnvelopeSync", numInstruments);
            return data?.Select(d => (float)d).ToArray();
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error getting Sointu WASM envelope sync");
            return null;
        }
    }

    /// <summary>
    /// Get the number of instruments in the loaded song.
    /// </summary>
    public async Task<int> GetNumInstrumentsAsync()
    {
        if (!_isInitialized) return 0;

        try
        {
            return await _jsRuntime.InvokeAsync<int>("sointuWasmInterop.getNumInstruments");
        }
        catch (JSException ex)
        {
            _logger?.LogError(ex, "Error getting number of instruments");
            return 0;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_isInitialized)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("sointuWasmInterop.cleanup");
                _isInitialized = false;
            }
            catch (JSException ex)
            {
                _logger?.LogError(ex, "Error cleaning up Sointu WASM module");
            }
        }

        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }
}

