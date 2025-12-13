using Microsoft.JSInterop;

using JSException = Microsoft.JSInterop.JSException;

namespace _4kampf.Web.Services;

public class WebAudioService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private string? _contextId;

    public WebAudioService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _contextId = $"audio_{Guid.NewGuid()}";
            await _jsRuntime.InvokeVoidAsync("webaudioInterop.init", _contextId);
        }
        catch (JSException ex)
        {
            throw new InvalidOperationException($"WebAudio initialization failed: {ex.Message}", ex);
        }
    }

    public async Task<double> LoadAudioAsync(string audioUrl)
    {
        if (_contextId == null) return 0;
        
        return await _jsRuntime.InvokeAsync<double>("webaudioInterop.loadAudio", _contextId, audioUrl);
    }

    public async Task PlayAsync()
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.play", _contextId);
    }

    public async Task PauseAsync()
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.pause", _contextId);
    }

    public async Task StopAsync()
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.stop", _contextId);
    }

    public async Task SetPositionAsync(double time)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.setPosition", _contextId, time);
    }

    public async Task<double> GetPositionAsync()
    {
        if (_contextId == null) return 0;
        
        return await _jsRuntime.InvokeAsync<double>("webaudioInterop.getPosition", _contextId);
    }

    public async Task<double> GetDurationAsync()
    {
        if (_contextId == null) return 0;
        
        return await _jsRuntime.InvokeAsync<double>("webaudioInterop.getDuration", _contextId);
    }

    public async Task SetVolumeAsync(double volume)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.setVolume", _contextId, volume);
    }

    public async Task<double> GetVolumeAsync()
    {
        if (_contextId == null) return 1.0;
        
        return await _jsRuntime.InvokeAsync<double>("webaudioInterop.getVolume", _contextId);
    }

    public async Task SetLoopAsync(bool loop)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webaudioInterop.setLoop", _contextId, loop);
    }

    public async Task<float[]?> GetEnvelopeDataAsync(int numChannels)
    {
        if (_contextId == null) return null;
        
        var data = await _jsRuntime.InvokeAsync<double[]>("webaudioInterop.getEnvelopeData", _contextId, numChannels);
        return data?.Select(d => (float)d).ToArray();
    }

    public async ValueTask DisposeAsync()
    {
        if (_contextId != null)
        {
            await _jsRuntime.InvokeVoidAsync("webaudioInterop.cleanup", _contextId);
        }
    }
}

