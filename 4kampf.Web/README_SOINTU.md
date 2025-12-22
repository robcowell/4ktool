# Sointu Integration for 4kampf Web

## Overview

This project is migrating from 4klang (Windows-only, 32-bit x86 assembly) to [Sointu](https://github.com/vsariola/sointu), a cross-platform fork that supports:
- Windows, macOS, and Linux
- WebAssembly (for browser-based synthesis)
- Modern toolchain (Go-based)

## Quick Start

### Installing Sointu

1. **Download Sointu**:
   ```bash
   git clone https://github.com/vsariola/sointu.git
   cd sointu
   ```

2. **Build Sointu** (see Sointu's README for details):
   ```bash
   # Sointu is written in Go, so you'll need Go installed
   go build ./cmd/sointu-compile
   go build ./cmd/sointu-play
   # etc.
   ```

3. **Install to system PATH** or configure path in `appsettings.json`:
   ```json
   {
     "Sointu": {
       "Path": "/path/to/sointu/bin"
     }
   }
   ```

### Using Sointu in 4kampf Web

The `SointuService` provides methods to:
- Compile YAML song files to assembly
- Render audio to WAV files
- Generate envelope data for shader sync

Example usage:
```csharp
@inject SointuService Sointu

@code {
    private async Task RenderMusic()
    {
        if (!Sointu.IsAvailable)
        {
            // Sointu not installed - show error or fallback
            return;
        }
        
        // Compile YAML song to assembly
        await Sointu.CompileSongAsync("song.yml", "song.asm");
        
        // Render to WAV
        await Sointu.RenderAudioAsync("song.asm", "music.wav");
        
        // Generate envelopes (when implemented)
        var envelopes = await Sointu.GenerateEnvelopesAsync("song.asm", numInstruments: 4);
    }
}
```

## Migration Path

### Phase 1: Server-Side Rendering (Current)
- ✅ `SointuService` created
- ✅ Service registered in DI container
- ⏳ Integration with music player component
- ⏳ Project settings UI for Sointu option
- ⏳ Envelope generation implementation

### Phase 2: WebAssembly Support (Future)
- ⏳ Build Sointu to WebAssembly
- ⏳ Create JavaScript wrapper
- ⏳ Integrate with WebAudio API
- ⏳ Real-time synthesis in browser

## File Formats

### 4klang (Legacy)
- Header file: `4klang.h` (C header with defines)
- Envelopes: `envelope-*.dat` (comma-separated floats)

### Sointu (New)
- Song file: `*.yml` (YAML format)
- Compiled: `*.asm` (assembly, similar to 4klang)
- Envelopes: TBD (format may differ)

## Resources

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Sointu Documentation](https://github.com/vsariola/sointu#readme)
- [Integration Plan](./SOINTU_INTEGRATION.md)
- [Music Synthesis Options](./MUSIC_SYNTHESIS_OPTIONS.md)

