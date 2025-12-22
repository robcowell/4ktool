# Testing Sointu WebAssembly Integration

## Quick Test

The WASM file has been successfully built! Here's how to test it:

### 1. Verify WASM File

```bash
# Check file exists and size
ls -lh 4kampf.Web/wwwroot/wasm/sointu.wasm

# Verify it's a valid WASM file
file 4kampf.Web/wwwroot/wasm/sointu.wasm
```

Expected output:
- File size: ~5MB (Go WASM builds are typically large)
- File type: `WebAssembly (wasm) binary module version 0x1 (MVP)`

### 2. Check Go WASM Runtime

Go programs compiled to WASM require `wasm_exec.js`:

```bash
ls -lh 4kampf.Web/wwwroot/js/wasm_exec.js
```

If missing, copy it:
```bash
cp "$(go env GOROOT)/misc/wasm/wasm_exec.js" 4kampf.Web/wwwroot/js/
```

### 3. Run the Application

```bash
cd 4kampf.Web
dotnet run
```

### 4. Test WASM Loading

#### Option A: Use Test Page

Navigate to: `http://localhost:5000/test-wasm.html`

The test page will:
1. ✅ Check WebAssembly support
2. ✅ Load the WASM module
3. ✅ Test JavaScript interop
4. ✅ Initialize audio context

#### Option B: Test in Main Application

1. Open: `http://localhost:5000`
2. Check status bar - "WASM" should show ✓ if loaded
3. Open browser console (F12) - look for:
   - `"Sointu WASM module loaded successfully"` ✅
   - Any error messages ❌

### 5. Test Real-Time Synthesis

1. **Enable WASM Rendering**:
   - Open Settings panel
   - Check "Use WebAssembly Rendering"
   - Save

2. **Create/Load Project with Song**:
   - Create a new project or load existing one
   - Ensure project has a Sointu YAML song file (`song.yml`)

3. **Render Music**:
   - Click "Build > Render Music"
   - Check console for messages
   - Verify real-time synthesis starts

## Expected Behavior

### Successful Load

**Console Output**:
```
Go WASM runtime loaded successfully
Sointu WASM module loaded with Go runtime
Sointu WASM module loaded successfully
```

**Note**: If you see CLI help text (like "Usage: js [flags]"), this is expected - Sointu is running as a CLI program. To use it for synthesis, you need to modify Sointu to export functions (see `SOINTU_WASM_MODIFICATION.md`).

**Status Bar**:
- WASM: ✓ (green checkmark)

### Common Issues

#### Issue: "Go is not defined"

**Cause**: `wasm_exec.js` not loaded

**Solution**:
1. Ensure `wasm_exec.js` exists in `wwwroot/js/`
2. Load it before the WASM module in `App.razor` or `_Host.cshtml`

#### Issue: "WASM module fails to load"

**Possible Causes**:
1. **File not found**: Check path `/wasm/sointu.wasm`
2. **MIME type**: Ensure server serves `.wasm` files with `application/wasm`
3. **CORS**: Check browser console for CORS errors

**Solution**:
- Verify file exists: `ls wwwroot/wasm/sointu.wasm`
- Check browser Network tab - WASM file should load with status 200
- Check browser console for specific error messages

#### Issue: "Functions not exported"

**Cause**: Go WASM programs don't directly export custom functions unless explicitly done

**Solution**:
The current JavaScript interop is a template. You may need to:

1. **Modify Sointu's Go code** to export functions via `js.Global().Set()`:
   ```go
   js.Global().Set("compileSong", js.FuncOf(compileSong))
   js.Global().Set("renderSamples", js.FuncOf(renderSamples))
   ```

2. **Or create a wrapper** that bridges Sointu's command-line interface to WASM exports

3. **Or use a different approach**: Call Sointu's functions through Go's runtime

#### Issue: "Audio not playing"

**Possible Causes**:
1. Browser requires user interaction to start audio
2. Audio context suspended
3. ScriptProcessorNode deprecated in some browsers

**Solution**:
- Click a button or interact with page before starting playback
- Check audio context state in console
- Consider using AudioWorkletNode instead of ScriptProcessorNode

## Next Steps

Once basic loading works:

1. **Verify Function Exports**: Check what functions Sointu actually exports
   ```javascript
   // In browser console after WASM loads
   console.log(Object.keys(window.sointuWasmInterop.wasmInstance.exports));
   ```

2. **Update Interop**: Modify `sointu-wasm-interop.js` to match actual exports

3. **Test Song Loading**: Try loading a simple Sointu YAML song

4. **Test Synthesis**: Verify audio generation works

## Debugging Tips

### Enable Detailed Logging

In browser console:
```javascript
// Enable verbose logging
window.sointuWasmInterop.debug = true;
```

### Inspect WASM Module

```javascript
// After initialization
const interop = window.sointuWasmInterop;
console.log('WASM Instance:', interop.wasmInstance);
console.log('Exports:', Object.keys(interop.wasmInstance.exports));
console.log('Memory:', interop.memory);
```

### Check Go Runtime

```javascript
// Check if Go runtime is loaded
console.log('Go available:', typeof Go !== 'undefined');
```

## References

- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [WebAssembly with Go](https://github.com/golang/go/wiki/WebAssembly)
- [Sointu Repository](https://github.com/vsariola/sointu)

