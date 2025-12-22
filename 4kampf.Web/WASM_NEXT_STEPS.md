# Sointu WASM - Next Steps

## ✅ Completed

1. **WASM File Built**: `sointu.wasm` (5.1MB) successfully compiled
2. **JavaScript Interop**: Updated to support Go WASM runtime
3. **Test Page**: Created `/test-wasm.html` for testing
4. **Documentation**: Created comprehensive guides

## ✅ Current Status

**WASM Module Loading**: ✅ **SUCCESS**
- Go WASM runtime loads correctly
- Sointu WASM module loads successfully
- Module is running (shows CLI help text, which is expected)

**Issue Identified**: Sointu's WASM build is a **command-line program**, not a library with exported functions. This means:
- It runs `main()` which executes the CLI interface
- It doesn't export functions like `compile_song`, `render_samples`, etc.
- We need to modify Sointu's source code to export functions instead

## ✅ Current Status: WASM Loads Successfully!

The WASM module is loading correctly! However, Sointu is running as a **command-line program** (you can see the CLI help text in the console), not as a library with exported functions.

**Next Step**: Modify Sointu's source code to export functions instead of running as CLI. See `SOINTU_WASM_MODIFICATION.md` for detailed instructions.

## ⚠️ Important Notes

### Go WASM Runtime Required

Go programs compiled to WASM require `wasm_exec.js`. The build script should have copied it, but verify:

```bash
ls -lh 4kampf.Web/wwwroot/js/wasm_exec.js
```

If missing, copy it manually:
```bash
cp "$(go env GOROOT)/misc/wasm/wasm_exec.js" 4kampf.Web/wwwroot/js/
```

### Function Exports

**Critical**: The current JavaScript interop (`sointu-wasm-interop.js`) is a **template** that assumes specific function exports. Go programs don't automatically export custom functions.

**Current Situation**: Sointu's WASM build runs as a CLI program (you can see the help text in the console). To use it for real-time synthesis, you need to modify Sointu's source code.

You have two options:

#### Option 1: Modify Sointu's Go Code (Recommended)

Add exports in Sointu's Go code:

```go
import "syscall/js"

func main() {
    // Export functions to JavaScript
    js.Global().Set("compileSong", js.FuncOf(compileSong))
    js.Global().Set("renderSamples", js.FuncOf(renderSamples))
    js.Global().Set("getNumInstruments", js.FuncOf(getNumInstruments))
    js.Global().Set("getEnvelopeData", js.FuncOf(getEnvelopeData))
    
    // Keep Go program running
    select {}
}

func compileSong(this js.Value, args []js.Value) interface{} {
    // Implementation
    return js.ValueOf(true)
}
```

Then rebuild:
```bash
cd sointu
export GOOS=js
export GOARCH=wasm
go build -o ../4kampf.Web/wwwroot/wasm/sointu.wasm ./cmd/sointu-play
```

#### Option 2: Use Command-Line Interface

If Sointu's WASM is a command-line program, you may need to:
- Pass arguments via Go's runtime
- Read output from stdout
- Use a different approach for real-time synthesis

## Testing Checklist

### 1. Basic Loading Test

```bash
cd 4kampf.Web
dotnet run
```

Navigate to: `http://localhost:5000/test-wasm.html`

**Expected**:
- ✅ WebAssembly support detected
- ✅ WASM module loads (may show warnings about missing exports)
- ✅ Audio context initializes

### 2. Check Browser Console

After loading, check console for:
- `"Sointu WASM module loaded successfully"` ✅
- Available exports: `Object.keys(window.sointuWasmInterop.wasmInstance.exports)`
- Any error messages ❌

### 3. Inspect Exports

In browser console:
```javascript
const interop = window.sointuWasmInterop;
console.log('Exports:', Object.keys(interop.wasmInstance.exports));
console.log('Go globals:', typeof window.compileSong, typeof window.renderSamples);
```

### 4. Test in Main Application

1. Open: `http://localhost:5000`
2. Check status bar - "WASM" indicator
3. Enable WASM rendering in Settings
4. Try rendering music

## Common Issues & Solutions

### Issue: "Go is not defined"

**Solution**: Ensure `wasm_exec.js` is loaded before WASM module
- Check `App.razor` includes the script
- Verify file exists: `wwwroot/js/wasm_exec.js`

### Issue: "Functions not exported"

**Solution**: Sointu needs to export functions. See "Function Exports" above.

### Issue: "WASM loads but functions don't work"

**Solution**: 
1. Check what functions are actually exported
2. Update `sointu-wasm-interop.js` to match actual exports
3. Or modify Sointu to export expected functions

## Recommended Path Forward

1. **Test Current Build**: Run test page and check console
2. **Inspect Exports**: See what Sointu actually exports
3. **Decide Approach**:
   - If Sointu exports functions → Update interop to match
   - If Sointu is CLI → Create wrapper or use different approach
   - If no exports → Modify Sointu to export functions
4. **Iterate**: Update code and test until working

## Resources

- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [Sointu Repository](https://github.com/vsariola/sointu)
- [WASM Testing Guide](./WASM_TESTING.md)
- [Build Instructions](./BUILD_WASM_INSTRUCTIONS.md)

