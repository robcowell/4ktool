# Sointu Integration Plan

## Overview

[Sointu](https://github.com/vsariola/sointu) is a cross-platform fork of 4klang that:
- Can target 386, amd64, and **WebAssembly**
- Tools run on Windows, Mac & Linux
- Uses YAML song files (instead of binary formats)
- Can be used as a library (Go API) or compiled to WASM
- Supports envelope sync (like 4klang)

## Integration Options

### Option 1: WebAssembly (Recommended for Browser)
**Approach**: Use Sointu's WebAssembly build in the browser

**Pros**:
- Fully client-side (no server dependency)
- Real-time synthesis in browser
- Cross-platform by nature
- No server load

**Cons**:
- Need to compile Sointu to WASM
- WASM file size considerations
- Browser compatibility

**Implementation**:
1. Build Sointu to WebAssembly (or use pre-built if available)
2. Create JavaScript wrapper for WebAudio integration
3. Load WASM module in browser
4. Call Sointu render functions from JavaScript
5. Stream audio to WebAudio API

### Option 2: Server-Side Rendering (Recommended for MVP)
**Approach**: Use Sointu's Go library on the server to render audio

**Pros**:
- Faster to implement (Go library is ready)
- Can use existing Sointu command-line tools
- Server has more resources
- Can cache rendered audio

**Cons**:
- Requires server processing
- Network latency for rendering
- Server load

**Implementation**:
1. Add Sointu Go tools to server (or call via subprocess)
2. Create C# service to call Sointu compiler/renderer
3. Render audio on server when project changes
4. Serve WAV files to client
5. Client loads WAV via WebAudio

### Option 3: Hybrid (Best Long-term)
**Approach**: Support both server-side rendering (MVP) and WASM (future)

**Pros**:
- Immediate cross-platform support
- Can add WASM later for offline/real-time
- Flexible architecture

**Cons**:
- More code to maintain
- Two code paths

## Current 4klang Integration Points

From codebase analysis:

1. **MusicPlayer.cs**: 
   - `LoadFrom4Klang()` - loads `4klang.h` file
   - `Get4klangSync()` - gets envelope sync values
   - Uses `envelope-*.dat` files

2. **Project.cs**:
   - `use4klangEnv` flag for envelope sync

3. **BuildUtils.cs**:
   - `DoExportHeader()` - exports shader header with `USE_4KLANG_ENV_SYNC` define

4. **Kampfpanzerin.cs**:
   - `RenderMusic()` - calls build scripts to render music
   - `SoundCommandLine()` - generates command for music rendering

5. **GraphicsManager.cs**:
   - Uses `Get4klangSync()` to get envelope values for shader uniforms

## Migration Strategy

### Phase 1: Server-Side Sointu Integration (MVP)
1. Add Sointu command-line tools to server
2. Create `SointuService` in C# to:
   - Compile YAML songs to assembly
   - Render audio to WAV
   - Generate envelope data
3. Update `MusicEnvelopeService` to load Sointu envelope data
4. Update `WebAudioService` to load Sointu-rendered WAV files
5. Add project setting to choose between 4klang/Sointu/Clinkster/Oidos

### Phase 2: WebAssembly Integration (Future)
1. Build Sointu to WebAssembly
2. Create JavaScript wrapper (`sointu-wasm.js`)
3. Integrate with WebAudio for real-time synthesis
4. Support offline rendering in browser

### Phase 3: Full Migration
1. Replace all 4klang references with Sointu
2. Update project file format to support Sointu YAML
3. Remove Windows-only dependencies

## Implementation Details

### Sointu Service Interface

```csharp
public interface ISointuService
{
    Task<string> CompileSongAsync(string yamlPath, string outputPath);
    Task<string> RenderAudioAsync(string songPath, string outputWavPath);
    Task<float[][]> GenerateEnvelopesAsync(string songPath, int numInstruments);
    Task<bool> IsAvailableAsync();
}
```

### Sointu vs 4klang Differences

1. **File Format**: YAML instead of `.h` header files
2. **Compilation**: Uses `sointu-compile` tool instead of direct assembly
3. **Envelope Format**: May differ slightly, need to verify
4. **API**: Go library API vs C/assembly API

## Next Steps

1. ✅ Research Sointu integration (this document)
2. ⏳ Test Sointu command-line tools locally
3. ⏳ Create `SointuService` for server-side rendering
4. ⏳ Update `MusicEnvelopeService` for Sointu envelopes
5. ⏳ Add Sointu option to project settings
6. ⏳ Test end-to-end music rendering with Sointu

