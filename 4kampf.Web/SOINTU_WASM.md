# Sointu WebAssembly Integration

## Overview

This document describes the WebAssembly (WASM) integration for Sointu, enabling browser-based real-time music synthesis without server-side rendering.

## Architecture

The WASM integration consists of:

1. **Sointu WASM Module** (`/wasm/sointu.wasm`): Compiled Sointu synthesizer for browser execution
2. **JavaScript Interop** (`/js/sointu-wasm-interop.js`): Wrapper for loading and using the WASM module
3. **C# Service** (`SointuWasmService`): Blazor service for interacting with WASM from C#
4. **WebAudio Integration**: Real-time audio synthesis using WebAudio API

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
   
   # Build the synthesizer library
   go build -o sointu.wasm ./cmd/sointu-play
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

### WASM Module Requirements

The compiled WASM module should export the following functions:

- `compile_song(yamlPtr, yamlLen) -> songDataPtr`: Compile YAML song to internal format
- `render_samples(songDataPtr, time, bufferSize) -> samplesPtr`: Generate audio samples
- `get_num_instruments() -> int`: Get number of instruments in loaded song
- `get_envelope_data() -> envelopePtr`: Get envelope data for shader sync
- `allocate(size) -> ptr`: Allocate memory in WASM heap
- `deallocate(ptr)`: Free memory in WASM heap

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
   - Compile it in the browser using WASM
   - Start real-time synthesis via WebAudio
   - Generate envelope data for shader synchronization

### Envelope Synchronization

With WASM rendering, envelope sync works automatically:

- Envelope data is generated during song compilation
- Real-time envelope values are extracted during playback
- Shader uniforms (`ev[]`) are updated each frame

## Limitations

### Current Implementation

The current JavaScript interop (`sointu-wasm-interop.js`) is a **template** that assumes:

1. Sointu WASM exports match the expected function signatures
2. Memory management functions (`allocate`/`deallocate`) are available
3. The WASM module uses a compatible memory layout

### Actual Sointu WASM Build

**Note**: The actual Sointu repository may not yet have full WASM support or may require different build steps. You may need to:

1. Modify Sointu's Go code to export WASM-compatible functions
2. Create a custom WASM wrapper
3. Use a different approach (e.g., TinyGo for smaller WASM builds)

### Fallback

If WASM is not available, the application automatically falls back to server-side rendering using the `SointuService`.

## Future Improvements

1. **AudioWorklet Support**: Replace deprecated `ScriptProcessorNode` with `AudioWorkletNode` for better performance
2. **Streaming Synthesis**: Implement streaming audio generation for longer songs
3. **Pre-compiled Songs**: Cache compiled song data to reduce load times
4. **WASM Size Optimization**: Use TinyGo or other tools to reduce WASM file size
5. **Error Handling**: Better error messages when WASM module fails to load

## Testing

### Test Page

A test page is available at `/test-wasm.html` to verify WASM integration:

1. Build the WASM module (see Build Steps above)
2. Run the application: `dotnet run`
3. Navigate to: `http://localhost:5000/test-wasm.html`
4. Click through the test buttons to verify:
   - WebAssembly support
   - WASM module loading
   - JavaScript interop
   - Audio context initialization

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

- Ensure song has been loaded into WASM module
- Check that `getEnvelopeSync` is being called during render loop
- Verify envelope data was generated during song compilation

## References

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [WebAudio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API)
- [WebAssembly Documentation](https://webassembly.org/)

