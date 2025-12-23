using Microsoft.JSInterop;

namespace _4kampf.Web.Services;

public class CameraService
{
    private readonly IJSRuntime _jsRuntime;
    private string? _cameraId;

    public CameraService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> InitializeAsync(string canvasId)
    {
        _cameraId = $"camera_{canvasId}";
        await _jsRuntime.InvokeVoidAsync("cameraControls.init", _cameraId);
        return _cameraId;
    }

    public async Task ResetAsync()
    {
        if (_cameraId == null) return;
        await _jsRuntime.InvokeVoidAsync("cameraControls.reset", _cameraId);
    }

    public async Task<(float x, float y, float z)> GetPositionAsync()
    {
        if (_cameraId == null) return (0, 0, 0);
        
        var pos = await _jsRuntime.InvokeAsync<CameraPosition>("cameraControls.getPosition", _cameraId);
        return (pos.x, pos.y, pos.z);
    }

    public async Task<(float x, float y, float z)> GetRotationAsync()
    {
        if (_cameraId == null) return (0, (float)Math.PI, 0);
        
        var rot = await _jsRuntime.InvokeAsync<CameraRotation>("cameraControls.getRotation", _cameraId);
        return (rot.x, rot.y, rot.z);
    }

    public async Task SetKeyStateAsync(string key, bool pressed)
    {
        if (_cameraId == null) return;
        await _jsRuntime.InvokeVoidAsync("cameraControls.setKeyState", _cameraId, key, pressed);
    }

    public async Task SetMouseStateAsync(bool down, float x, float y)
    {
        if (_cameraId == null) return;
        await _jsRuntime.InvokeVoidAsync("cameraControls.setMouseState", _cameraId, down, x, y);
    }

    public async Task UpdateAsync(bool shiftPressed)
    {
        if (_cameraId == null) return;
        await _jsRuntime.InvokeVoidAsync("cameraControls.update", _cameraId, shiftPressed);
    }

    public async Task SetModeAsync(string mode)
    {
        if (_cameraId == null) return;
        await _jsRuntime.InvokeVoidAsync("cameraControls.setMode", _cameraId, mode);
    }

    public async Task<string> GetModeAsync()
    {
        if (_cameraId == null) return "freefly";
        return await _jsRuntime.InvokeAsync<string>("cameraControls.getMode", _cameraId) ?? "freefly";
    }
}

public class CameraPosition
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

public class CameraRotation
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

