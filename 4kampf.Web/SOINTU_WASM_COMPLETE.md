# Sointu WebAssembly Integration - Complete

## ✅ Status: Implementation Complete

The Sointu source code has been successfully modified to export functions for WebAssembly instead of running as a CLI program.

## What Was Done

### 1. Created WASM-Specific Build (`sointu/cmd/sointu-wasm/main.go`)

A new Go program was created that:
- Exports JavaScript functions using `syscall/js`
- Uses Sointu's library API (`sointu.Play`, `sointu.Song`, etc.)
- **Pre-renders entire songs** during compilation (not real-time synthesis)
- Generates envelope data for shader synchronization
- Includes progress logging for UI feedback
- Implements Web Worker architecture to prevent UI blocking

### 2. Exported Functions

The following functions are now available in JavaScript:

- **`compileSong(yamlContent)`**: Compiles a YAML song string and pre-renders audio
  - Returns: `{success: bool, numInstruments: int, duration: float, audioBuffer: Float32Array, envelopeData: Float32Array, error?: string}`
  - Pre-renders entire song during compilation
  - Generates envelope data simultaneously
  
- **`getNumInstruments()`**: Returns number of instruments
  - Returns: `int`
  
- **`renderSamples(...)`**: Legacy function (not used - audio is pre-rendered)
  
- **`getEnvelopeSync(...)`**: Legacy function (not used - envelope data is pre-rendered)
  
- **`resetPlayback()`**: Resets playback position

### 3. Updated JavaScript Interop

The `sointu-wasm-interop.js` file was updated to:
- Use Web Worker for WASM compilation (prevents UI blocking)
- Handle pre-rendered audio playback with `ScriptProcessorNode`
- Read audio from pre-rendered buffer (not real-time synthesis)
- Support envelope synchronization from pre-rendered data
- Manage playback state and timing
- Update progress bar during compilation

### 4. Created Web Worker (`sointu-wasm-worker.js`)

A new Web Worker was created to:
- Load and initialize WASM module in background thread
- Handle song compilation without blocking UI
- Transfer audio and envelope data to main thread
- Forward progress messages for UI updates

### 5. Updated Build Scripts

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

4. Test song loading and playback:
   - Click "Load & Play Sample Song"
   - Verify progress bar updates (0-100%)
   - Verify audio plays correctly
   - Check console for progress messages

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

1. **Song Compilation (Web Worker)**:
   - YAML song is sent to Web Worker
   - Worker calls `compileSong()` in WASM module
   - Song is parsed, validated, and compiled
   - **Entire song is pre-rendered** to an audio buffer (Float32Array, interleaved stereo)
   - Envelope data is generated simultaneously (Float32Array, per-instrument per-sample)
   - Progress is logged: `"DEBUG: Render progress: X%"`
   - Audio and envelope buffers are transferred to main thread

2. **Audio Playback (Main Thread)**:
   - `ScriptProcessorNode` requests audio samples
   - Samples are read from pre-rendered buffer (not synthesized in real-time)
   - Samples are sent to WebAudio API
   - Playback position is tracked

3. **Envelope Synchronization**:
   - Envelope values are read from pre-rendered buffer based on playback position
   - Values are passed to shaders as uniform arrays (`ev[]`)
   - Shaders can react to music in real-time

## Limitations

1. **Pre-Rendered Audio**: The entire song is rendered upfront, which:
   - Requires memory for full song buffer (can be large for long songs)
   - Limits to songs that fit in memory
   - Doesn't support real-time parameter changes
   - Compilation takes 10-30 seconds for complex songs

2. **Envelope Approximation**: Current envelope data is:
   - Derived from audio amplitude (not actual synth envelopes)
   - Simplified per-instrument distribution
   - May not match actual synth envelope outputs
   - Pre-rendered (not extracted in real-time)

3. **ScriptProcessorNode**: Uses deprecated API:
   - Consider migrating to `AudioWorkletNode` for better performance
   - Current implementation works but may be removed in future browsers

4. **Web Worker Communication**: 
   - Audio buffer transfer can be large (several MB for long songs)
   - Progress messages are forwarded but may be delayed

## Future Improvements

1. **AudioWorklet Migration**: Replace ScriptProcessorNode
   - Better performance (off main thread)
   - More reliable timing
   - Future-proof (ScriptProcessorNode is deprecated)

2. **Streaming Synthesis**: For very long songs, render in chunks
   - Would reduce memory usage
   - Would allow playback to start before full render completes
   - Would require chunked buffer management

3. **Real-Time Synthesis Option**: Allow users to choose pre-rendered vs real-time
   - Real-time would use less memory but may have audio glitches
   - Would support real-time parameter changes
   - Would require more complex state management

4. **True Envelope Extraction**: Extract actual envelope data from synth
   - Requires access to synth internal state during rendering
   - Would provide accurate envelope values
   - Would match server-side envelope generation

## Files Modified/Created

- **Created**: `sointu/cmd/sointu-wasm/main.go` - WASM entry point with exported functions
- **Created**: `4kampf.Web/wwwroot/js/sointu-wasm-worker.js` - Web Worker for background compilation
- **Updated**: `4kampf.Web/wwwroot/js/sointu-wasm-interop.js` - Main thread interop with Web Worker
- **Updated**: `4kampf.Web/wwwroot/test-wasm.html` - Comprehensive test page with progress bar
- **Updated**: `4kampf.Web/build-sointu-wasm.sh` - Build script to use `sointu-wasm`
- **Updated**: `4kampf.Web/build-sointu-wasm.bat` - Windows build script

## References

- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [syscall/js Package](https://pkg.go.dev/syscall/js)
- [Sointu Repository](https://github.com/vsariola/sointu)
- [WebAudio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API)

