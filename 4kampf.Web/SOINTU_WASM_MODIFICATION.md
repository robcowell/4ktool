# Modifying Sointu for WebAssembly Function Exports

## Current Situation

✅ **WASM module loads successfully** - The Sointu WASM file loads and runs.

⚠️ **Issue**: Sointu's WASM build is a command-line program, not a library. When it loads, it runs `main()` which shows the CLI help text. This is expected behavior for a Go program compiled to WASM.

## Solution: Create a WASM-Specific Build

To use Sointu for real-time synthesis in the browser, we need to modify Sointu's source code to export functions instead of running as a CLI.

### Step 1: Create a WASM Wrapper

Create a new file in Sointu's repository: `cmd/sointu-wasm/main.go`

```go
package main

import (
    "encoding/json"
    "syscall/js"
    "github.com/vsariola/sointu" // Adjust import path as needed
)

func main() {
    // Export functions to JavaScript
    js.Global().Set("compileSong", js.FuncOf(compileSong))
    js.Global().Set("renderSamples", js.FuncOf(renderSamples))
    js.Global().Set("getNumInstruments", js.FuncOf(getNumInstruments))
    js.Global().Set("getEnvelopeData", js.FuncOf(getEnvelopeData))
    
    // Keep Go program running
    select {}
}

// compileSong compiles a YAML song string to internal format
func compileSong(this js.Value, args []js.Value) interface{} {
    if len(args) < 1 {
        return js.ValueOf(map[string]interface{}{
            "success": false,
            "error": "Missing YAML content argument",
        })
    }
    
    yamlContent := args[0].String()
    
    // Parse YAML and compile song
    // This depends on Sointu's internal API
    // You'll need to adapt this to Sointu's actual API
    
    return js.ValueOf(map[string]interface{}{
        "success": true,
        "songData": "pointer_or_id", // Return identifier for the compiled song
    })
}

// renderSamples generates audio samples for a given time range
func renderSamples(this js.Value, args []js.Value) interface{} {
    if len(args) < 3 {
        return js.ValueOf(nil)
    }
    
    songDataID := args[0].String()
    startTime := args[1].Float()
    numSamples := args[2].Int()
    
    // Generate samples using Sointu's synthesis engine
    // Return pointer to sample buffer in WASM memory
    
    return js.ValueOf("sample_buffer_pointer")
}

// getNumInstruments returns the number of instruments in the loaded song
func getNumInstruments(this js.Value, args []js.Value) interface{} {
    // Return number of instruments
    return js.ValueOf(0) // Replace with actual count
}

// getEnvelopeData returns envelope data for shader sync
func getEnvelopeData(this js.Value, args []js.Value) interface{} {
    // Return pointer to envelope data buffer
    return js.ValueOf("envelope_buffer_pointer")
}
```

### Step 2: Build the WASM Module

```bash
cd sointu
export GOOS=js
export GOARCH=wasm
go build -o ../4kampf.Web/wwwroot/wasm/sointu.wasm ./cmd/sointu-wasm
```

### Step 3: Update JavaScript Interop

Once Sointu exports functions, update `sointu-wasm-interop.js` to use them:

```javascript
async loadSong(yamlContent) {
    // Call the exported function
    const result = window.compileSong(yamlContent);
    if (result.success) {
        this.songData = result.songData;
        return true;
    }
    return false;
}
```

## Alternative: Use Sointu's Library API

If Sointu has a library API (not just CLI), you can:

1. **Import Sointu as a library** in your WASM wrapper
2. **Call library functions** directly instead of CLI commands
3. **Export wrapper functions** that call the library

## Testing the Modified Build

After modifying and rebuilding:

1. **Reload the test page**: `http://localhost:5000/test-wasm.html`
2. **Check console**: Should not show CLI help text
3. **Check exports**: `Object.keys(window)` should include `compileSong`, `renderSamples`, etc.
4. **Test functions**: Try calling `window.compileSong("test yaml")` in console

## References

- [Go WebAssembly with js package](https://pkg.go.dev/syscall/js)
- [Sointu Repository](https://github.com/vsariola/sointu)
- [Exporting Go Functions to JavaScript](https://go.dev/wiki/WebAssembly)

