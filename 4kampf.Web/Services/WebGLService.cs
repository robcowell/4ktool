using Microsoft.JSInterop;

using JSException = Microsoft.JSInterop.JSException;

namespace _4kampf.Web.Services;

public class WebGLService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private string? _contextId;

    public WebGLService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync(string canvasId)
    {
        try
        {
            _contextId = await _jsRuntime.InvokeAsync<string>("webglInterop.init", canvasId);
            
            if (string.IsNullOrEmpty(_contextId))
            {
                throw new InvalidOperationException("Failed to initialize WebGL context");
            }
        }
        catch (JSException ex)
        {
            throw new InvalidOperationException($"WebGL initialization failed: {ex.Message}", ex);
        }
    }

    public async Task<string?> CreateShaderAsync(int type, string source)
    {
        if (_contextId == null) return null;
        
        return await _jsRuntime.InvokeAsync<string>("webglInterop.createShader", _contextId, type, source);
    }

    public async Task<string?> CreateProgramAsync(string? vertexShaderId, string? fragmentShaderId)
    {
        if (_contextId == null) return null;
        
        return await _jsRuntime.InvokeAsync<string>("webglInterop.createProgram", 
            _contextId, vertexShaderId, fragmentShaderId);
    }

    public async Task UseProgramAsync(string? programId)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.useProgram", _contextId, programId);
    }

    public async Task SetUniform3fAsync(string? programId, string name, float x, float y, float z)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.setUniform3f", _contextId, programId, name, x, y, z);
    }

    public async Task SetUniform1fvAsync(string? programId, string name, float[] values)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.setUniform1fv", _contextId, programId, name, values);
    }

    public async Task<string?> CreateFramebufferAsync(int width, int height)
    {
        if (_contextId == null) return null;
        
        return await _jsRuntime.InvokeAsync<string>("webglInterop.createFramebuffer", _contextId, width, height);
    }

    public async Task BindFramebufferAsync(string? fboId)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.bindFramebuffer", _contextId, fboId);
    }

    public async Task SetViewportAsync(int x, int y, int width, int height)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.setViewport", _contextId, x, y, width, height);
    }

    public async Task ClearAsync(float r, float g, float b, float a)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.clear", _contextId, r, g, b, a);
    }

    public async Task RenderQuadAsync()
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.renderQuad", _contextId);
    }

    public async Task ResizeAsync(int width, int height)
    {
        if (_contextId == null) return;
        
        await _jsRuntime.InvokeVoidAsync("webglInterop.resize", _contextId, width, height);
    }

    public async ValueTask DisposeAsync()
    {
        if (_contextId != null)
        {
            await _jsRuntime.InvokeVoidAsync("webglInterop.cleanup", _contextId);
        }
    }
}

