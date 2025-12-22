# Sointu WebAssembly Integration

## Overview

This document describes the WebAssembly (WASM) integration for Sointu, enabling browser-based music synthesis without server-side rendering. The implementation uses a Web Worker to prevent UI blocking during song compilation and pre-renders the entire song for smooth playback.

## Architecture

The WASM integration consists of:

1. **Sointu WASM Module** (`/wasm/sointu.wasm`): Compiled Sointu synthesizer for browser execution
2. **Web Worker** (`/js/sointu-wasm-worker.js`): Background thread for WASM compilation to prevent UI blocking
3. **JavaScript Interop** (`/js/sointu-wasm-interop.js`): Main thread wrapper for loading and using the WASM module
4. **Go WASM Runtime** (`/js/wasm_exec.js`): Required runtime for Go programs compiled to WASM
5. **C# Service** (`SointuWasmService`): Blazor service for interacting with WASM from C#
6. **WebAudio Integration**: Pre-rendered audio playback using WebAudio API

## Building Sointu to WebAssembly

Sointu is written in Go, which can be compiled to WebAssembly. To build:

### Prerequisites

- Go 1.21 or later
- Sointu source code (from https://github.com/vsariola/sointu)

### Build Steps

#### Option 1: Automated Build Script (Recommended)

**macOS/Linux**:
```bash
cd 4kampf.Web
./build-sointu-wasm.sh
```

**Windows**:
```cmd
cd 4kampf.Web
build-sointu-wasm.bat
```

The script will:
- Check for Go installation
- Clone/update the Sointu repository
- Build Sointu to WebAssembly
- Copy the WASM file to `wwwroot/wasm/sointu.wasm`
- Copy Go WASM runtime to `wwwroot/js/wasm_exec.js` (if needed)

#### Option 2: Manual Build

1. **Install Go** (if not already installed):
   - **macOS**: `brew install go` or download from [go.dev/dl](https://go.dev/dl/)
   - **Linux**: `sudo apt install golang` or download from [go.dev/dl](https://go.dev/dl/)
   - **Windows**: Download from [go.dev/dl](https://go.dev/dl/)

2. **Clone Sointu repository**:
   ```bash
   git clone https://github.com/vsariola/sointu.git
   cd sointu
   ```

3. **Build for WebAssembly**:
   ```bash
   # Set Go environment for WASM
   export GOOS=js
   export GOARCH=wasm
   
   # Build the WASM-specific wrapper (exports functions instead of CLI)
   go build -o sointu.wasm ./cmd/sointu-wasm
   ```

4. **Copy WASM files to project**:
   ```bash
   # Ensure wasm directory exists
   mkdir -p 4kampf.Web/wwwroot/wasm
   
   # Copy WASM module
   cp sointu.wasm 4kampf.Web/wwwroot/wasm/sointu.wasm
   
   # Copy Go WASM runtime (if needed)
   cp "$(go env GOROOT)/misc/wasm/wasm_exec.js" 4kampf.Web/wwwroot/js/
   ```

#### Option 3: Using TinyGo (Alternative)

For smaller WASM builds, you can use [TinyGo](https://tinygo.org/):

```bash
# Install TinyGo (see https://tinygo.org/getting-started/)
tinygo build -o sointu.wasm -target wasi ./cmd/sointu-play
```

### WASM Module Exports

The compiled WASM module exports the following JavaScript functions (via Go's `syscall/js`):

- `compileSong(yamlContent) -> {success, numInstruments, duration, audioBuffer, envelopeData}`: Compile YAML song and pre-render audio
- `renderSamples(...)`: Legacy function (not used - audio is pre-rendered)
- `getNumInstruments() -> int`: Get number of instruments in loaded song
- `getEnvelopeSync(...)`: Legacy function (not used - envelope data is pre-rendered)
- `resetPlayback()`: Reset playback position

**Note**: The implementation uses **pre-rendered audio** - the entire song is rendered during compilation and stored in memory for efficient playback.

## Usage

### Enabling WASM Rendering

1. Open the Settings panel
2. Check "Use WebAssembly Rendering"
3. Ensure the WASM module is available at `/wasm/sointu.wasm`

### Rendering Music

When WASM rendering is enabled:

1. Click "Build > Render Music"
2. The application will:
   - Load the Sointu YAML song file
   - **Compile and pre-render** the entire song in a Web Worker (prevents UI blocking)
   - Show a **progress bar** (0-100%) during compilation
   - Transfer the pre-rendered audio buffer to the main thread
   - Start playback via WebAudio API using the pre-rendered buffer
   - Generate envelope data for shader synchronization during compilation

### Envelope Synchronization

With WASM rendering, envelope sync works automatically:

- Envelope data is **pre-rendered** during song compilation (along with audio)
- Envelope values are read from the pre-rendered buffer during playback
- Shader uniforms (`ev[]`) are updated each frame based on playback position

## Implementation Details

### Web Worker Architecture

To prevent UI blocking during song compilation (which can take 10-30 seconds for complex songs), the implementation uses a **Web Worker**:

1. **Worker Thread** (`sointu-wasm-worker.js`):
   - Loads and initializes the WASM module
   - Handles song compilation in the background
   - Pre-renders the entire audio buffer
   - Transfers audio and envelope data to the main thread

2. **Main Thread** (`sointu-wasm-interop.js`):
   - Manages WebAudio playback
   - Reads from the pre-rendered buffer
   - Updates progress bar during compilation
   - Handles user interactions

### Pre-Rendered Audio

The implementation uses **pre-rendered audio** rather than real-time synthesis:

- **Advantages**: Smooth playback, no audio glitches, efficient envelope sync
- **Trade-off**: Requires full song compilation before playback starts
- **Memory**: Entire song is stored in memory (Float32Array, interleaved stereo)

### Progress Reporting

During compilation, the Go code logs progress messages that are intercepted and displayed:
- Progress messages: `"DEBUG: Render progress: X%"`
- Completion: `"DEBUG: Song render complete"`
- These are forwarded from the worker to the main thread for UI updates

### Fallback

If WASM is not available, the application automatically falls back to server-side rendering using the `SointuService`.

## Future Improvements

1. **AudioWorklet Support**: Replace deprecated `ScriptProcessorNode` with `AudioWorkletNode` for better performance
2. **Streaming Synthesis**: For very long songs, implement chunked rendering to reduce memory usage
3. **Pre-compiled Songs**: Cache compiled song data to reduce load times
4. **WASM Size Optimization**: Use TinyGo or other tools to reduce WASM file size (currently ~5MB)
5. **Error Handling**: Better error messages when WASM module fails to load
6. **Real-time Synthesis**: Option to use real-time synthesis instead of pre-rendering for lower memory usage

## Testing

### Test Page

A comprehensive test page is available at `/test-wasm.html` to verify WASM integration:

1. Build the WASM module (see Build Steps above)
2. Run the application: `dotnet run`
3. Navigate to: `http://localhost:5000/test-wasm.html`
4. Click through the test sections to verify:
   - ✅ WebAssembly support detection
   - ✅ WASM module loading (via Web Worker)
   - ✅ JavaScript interop (function exports)
   - ✅ Audio context initialization
   - ✅ **Song loading and playback** (loads `physics_girl_st.yml` example)
   - ✅ **Progress bar** (visual updates during compilation)
   - ✅ **Console output** (real-time logging)

The test page loads the `physics_girl_st.yml` example song from the Sointu repository and plays it back, demonstrating the full integration.

### Manual Testing in Application

1. **Start the application**:
   ```bash
   cd 4kampf.Web
   dotnet run
   ```

2. **Open the application** in a browser:
   - Navigate to `http://localhost:5000`

3. **Check WASM status**:
   - Look at the status bar - "WASM" should show ✓ if loaded
   - Check browser console for any errors

4. **Enable WASM rendering**:
   - Open Settings panel
   - Check "Use WebAssembly Rendering"
   - Save settings

5. **Test music rendering**:
   - Create or load a project with a Sointu YAML song file
   - Click "Build > Render Music"
   - Verify real-time synthesis starts

## Troubleshooting

### WASM Module Not Found

- Ensure `sointu.wasm` exists at `/wasm/sointu.wasm` in `wwwroot`
- Check browser console for loading errors
- Verify WASM file is served with correct MIME type (`application/wasm`)
- Check that the file was copied correctly: `ls -lh wwwroot/wasm/sointu.wasm`

### Initialization Fails

- Check browser console for WebAssembly errors
- Verify Go WASM runtime is loaded (if using `wasm_exec.js`)
- Ensure browser supports WebAssembly (all modern browsers do)

### Audio Not Playing

- Check browser audio context state (may need user interaction to start)
- Verify WebAudio API is available
- Check console for audio processing errors

### Envelope Sync Not Working

- Ensure song has been loaded and compiled (envelope data is pre-rendered)
- Check that `preRenderedBuffer` and `envelopeData` are populated after compilation
- Verify envelope data is being read from the pre-rendered buffer during playback

### Progress Bar Not Updating

- Check browser console for progress messages (`"DEBUG: Render progress: X%"`)
- Ensure progress bar container is visible (not `display: none`)
- Verify `updateProgress()` function is being called (check console logs)
- Check that CSS allows the progress bar to expand (no `max-width: 100%` constraint)

### Audio Not Playing

- Check that `audioBuffer` and `envelopeData` are present in the compile result
- Verify `preRenderedBuffer` is populated after `loadSong()` completes
- Check browser console for audio processing errors
- Ensure audio context is not suspended (may require user interaction)

## References

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [WebAudio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API)
- [WebAssembly Documentation](https://webassembly.org/)

