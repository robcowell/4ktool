# Sointu WebAssembly Integration - Complete

## ✅ Status: Implementation Complete

The Sointu source code has been successfully modified to export functions for WebAssembly instead of running as a CLI program.

## What Was Done

### 1. Created WASM-Specific Build (`sointu/cmd/sointu-wasm/main.go`)

A new Go program was created that:
- Exports JavaScript functions using `syscall/js`
- Uses Sointu's library API (`sointu.Play`, `sointu.Song`, etc.)
- Pre-renders songs for real-time playback
- Generates envelope data for shader synchronization

### 2. Exported Functions

The following functions are now available in JavaScript:

- **`compileSong(yamlContent)`**: Compiles a YAML song string
  - Returns: `{success: bool, numInstruments: int, duration: float, error?: string}`
  
- **`renderSamples(startTime, numSamples)`**: Generates audio samples
  - Returns: `Float32Array` of stereo samples (interleaved L/R)
  
- **`getNumInstruments()`**: Returns number of instruments
  - Returns: `int`
  
- **`getEnvelopeSync(time)`**: Gets envelope values for shader sync
  - Returns: `Float32Array` of envelope values per instrument
  
- **`resetPlayback()`**: Resets playback position

### 3. Updated JavaScript Interop

The `sointu-wasm-interop.js` file was updated to:
- Use exported Go functions via `window.compileSong`, etc.
- Handle real-time audio synthesis with `ScriptProcessorNode`
- Support envelope synchronization for shaders
- Manage playback state and timing

### 4. Updated Build Scripts

Both `build-sointu-wasm.sh` and `build-sointu-wasm.bat` now:
- Prefer building `sointu-wasm` (with exported functions)
- Fall back to CLI versions if `sointu-wasm` doesn't exist
- Show warnings when building CLI versions

## Build Instructions

### Build the WASM Module

```bash
cd 4kampf.Web
./build-sointu-wasm.sh
```

Or manually:

```bash
cd sointu
export GOOS=js
export GOARCH=wasm
go build -o ../4kampf.Web/wwwroot/wasm/sointu.wasm ./cmd/sointu-wasm
```

### Verify Build

```bash
ls -lh 4kampf.Web/wwwroot/wasm/sointu.wasm
# Should show ~4.7MB file
```

## Testing

### 1. Test WASM Loading

1. Start the application:
   ```bash
   cd 4kampf.Web
   dotnet run
   ```

2. Navigate to: `http://localhost:5000/test-wasm.html`

3. Check browser console:
   - Should see: `"Sointu WASM module initialized"`
   - Should see: `"Sointu WASM module loaded successfully"`
   - Should NOT see CLI help text

4. Verify exports:
   ```javascript
   console.log(typeof window.compileSong); // Should be "function"
   console.log(typeof window.renderSamples); // Should be "function"
   console.log(typeof window.getNumInstruments); // Should be "function"
   console.log(typeof window.getEnvelopeSync); // Should be "function"
   ```

### 2. Test in Main Application

1. Open: `http://localhost:5000`

2. Check status bar:
   - "WASM: ✓" should be green

3. Enable WASM rendering:
   - Settings → Music Synthesis → "Use WebAssembly Rendering" ✓

4. Load a Sointu song:
   - File → Manage Sointu Song... (or create a `song.yml` file)

5. Render music:
   - Build → Render Music
   - Should see: "Using WebAssembly rendering (client-side)..."
   - Should see: "Sointu WASM song loaded successfully"
   - Should hear music playing

## Architecture

### How It Works

1. **Song Compilation**: 
   - YAML song is parsed and validated
   - Synth is compiled from the patch
   - Entire song is pre-rendered to an audio buffer
   - Envelope data is generated from audio amplitude

2. **Real-Time Playback**:
   - `ScriptProcessorNode` requests audio samples
   - `renderSamples()` extracts samples from pre-rendered buffer
   - Samples are sent to WebAudio API
   - Playback position is tracked

3. **Envelope Synchronization**:
   - `getEnvelopeSync()` returns envelope values for current time
   - Values are passed to shaders as uniform arrays
   - Shaders can react to music in real-time

## Limitations

1. **Pre-Rendered Audio**: The entire song is rendered upfront, which:
   - Requires memory for full song buffer
   - Limits to songs that fit in memory
   - Doesn't support real-time parameter changes

2. **Envelope Approximation**: Current envelope data is:
   - Derived from audio amplitude (not actual synth envelopes)
   - Simplified per-instrument distribution
   - May not match actual synth envelope outputs

3. **ScriptProcessorNode**: Uses deprecated API:
   - Consider migrating to `AudioWorkletNode` for better performance
   - Current implementation works but may be removed in future browsers

## Future Improvements

1. **Real-Time Synthesis**: Instead of pre-rendering, synthesize on-demand
   - Would require more complex state management
   - Would support real-time parameter changes
   - Would reduce memory usage

2. **True Envelope Extraction**: Extract actual envelope data from synth
   - Requires access to synth internal state
   - Would provide accurate envelope values
   - Would match server-side envelope generation

3. **AudioWorklet Migration**: Replace ScriptProcessorNode
   - Better performance (off main thread)
   - More reliable timing
   - Future-proof

## Files Modified/Created

- **Created**: `sointu/cmd/sointu-wasm/main.go` - WASM entry point with exported functions
- **Updated**: `4kampf.Web/wwwroot/js/sointu-wasm-interop.js` - JavaScript interop for exported functions
- **Updated**: `4kampf.Web/build-sointu-wasm.sh` - Build script to use `sointu-wasm`
- **Updated**: `4kampf.Web/build-sointu-wasm.bat` - Windows build script

## References

- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [syscall/js Package](https://pkg.go.dev/syscall/js)
- [Sointu Repository](https://github.com/vsariola/sointu)
- [WebAudio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API)

