using Microsoft.JSInterop;

namespace _4kampf.Web.Services;

public class CodeEditorService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isInitialized = false;

    public CodeEditorService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        
        // Call the global monacoLoader.init function directly
        await _jsRuntime.InvokeVoidAsync("monacoLoader.init");
        _isInitialized = true;
    }

    public async Task CreateEditorAsync(string containerId, string language, string initialValue, DotNetObjectReference<CodeEditorCallback>? callbackRef)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        
        await _jsRuntime.InvokeVoidAsync("monacoLoader.createEditor", containerId, language, initialValue, callbackRef);
    }

    public async Task SetValueAsync(string containerId, string value)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.setValue", containerId, value);
    }

    public async Task<string> GetValueAsync(string containerId)
    {
        if (!_isInitialized) return string.Empty;
        return await _jsRuntime.InvokeAsync<string>("monacoLoader.getValue", containerId) ?? string.Empty;
    }

    public async Task DestroyEditorAsync(string containerId)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.destroyEditor", containerId);
    }

    public async Task UndoAsync(string containerId)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.undo", containerId);
    }

    public async Task RedoAsync(string containerId)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.redo", containerId);
    }

    public async Task<bool> CanUndoAsync(string containerId)
    {
        if (!_isInitialized) return false;
        return await _jsRuntime.InvokeAsync<bool>("monacoLoader.canUndo", containerId);
    }

    public async Task<bool> CanRedoAsync(string containerId)
    {
        if (!_isInitialized) return false;
        return await _jsRuntime.InvokeAsync<bool>("monacoLoader.canRedo", containerId);
    }

    public async Task ShowFindAsync(string containerId)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.showFind", containerId);
    }

    public async Task SetLineNumbersAsync(string containerId, bool visible)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.setLineNumbers", containerId, visible);
    }

    public async Task GotoLineAsync(string containerId, int lineNumber)
    {
        if (!_isInitialized) return;
        await _jsRuntime.InvokeVoidAsync("monacoLoader.gotoLine", containerId, lineNumber);
    }

    public async ValueTask DisposeAsync()
    {
        // Nothing to dispose for global scripts
        await Task.CompletedTask;
    }
}

public class CodeEditorCallback
{
    private readonly Func<string, Task> _onChange;

    public CodeEditorCallback(Func<string, Task> onChange)
    {
        _onChange = onChange;
    }

    [JSInvokable]
    public async Task OnChange(string value)
    {
        await _onChange(value);
    }
}

