# Building Sointu to WebAssembly - Quick Start

This guide provides step-by-step instructions for building the Sointu WebAssembly module.

## Prerequisites

1. **Go Programming Language** (version 1.21 or later)
   - Download from: https://go.dev/dl/
   - Verify installation: `go version`

2. **Git** (for cloning Sointu repository)
   - Usually pre-installed on macOS/Linux
   - Windows: Download from https://git-scm.com/

## Quick Build (Automated)

### macOS / Linux

```bash
cd 4kampf.Web
./build-sointu-wasm.sh
```

### Windows

```cmd
cd 4kampf.Web
build-sointu-wasm.bat
```

The script will:
- ✅ Check for Go installation
- ✅ Clone/update Sointu repository
- ✅ Build Sointu to WebAssembly
- ✅ Copy files to the correct locations

## Manual Build

If the automated script doesn't work, follow these steps:

### Step 1: Clone Sointu

```bash
cd /path/to/4ktool
git clone https://github.com/vsariola/sointu.git
cd sointu
```

### Step 2: Build for WebAssembly

```bash
# Set Go environment for WASM
export GOOS=js
export GOARCH=wasm

# Build the WASM-specific wrapper (exports functions instead of CLI)
go build -o sointu.wasm ./cmd/sointu-wasm
```

**Note**: The `sointu-wasm` command is a custom entry point that exports JavaScript functions. If it doesn't exist, you may need to create it (see `SOINTU_WASM_MODIFICATION.md`).

### Step 3: Copy to Project

```bash
# From the sointu directory
cd ../4kampf.Web

# Create wasm directory if it doesn't exist
mkdir -p wwwroot/wasm

# Copy WASM file
cp ../sointu/sointu.wasm wwwroot/wasm/sointu.wasm

# Copy Go WASM runtime (optional, may be needed)
# Try standard location first, then alternative
GOROOT=$(go env GOROOT)
if [ -f "$GOROOT/misc/wasm/wasm_exec.js" ]; then
    cp "$GOROOT/misc/wasm/wasm_exec.js" wwwroot/js/wasm_exec.js
elif [ -f "$GOROOT/lib/wasm/wasm_exec.js" ]; then
    cp "$GOROOT/lib/wasm/wasm_exec.js" wwwroot/js/wasm_exec.js
else
    # Try to find it
    WASM_EXEC=$(find "$GOROOT" -name "wasm_exec.js" 2>/dev/null | head -1)
    if [ -n "$WASM_EXEC" ]; then
        cp "$WASM_EXEC" wwwroot/js/wasm_exec.js
    else
        echo "Warning: wasm_exec.js not found. Download from:"
        echo "https://github.com/golang/go/blob/master/misc/wasm/wasm_exec.js"
    fi
fi
```

## Verify Build

1. **Check file exists**:
   ```bash
   ls -lh 4kampf.Web/wwwroot/wasm/sointu.wasm
   ```
   You should see a `.wasm` file (typically several MB in size).

2. **Test in browser**:
   - Run the application: `dotnet run`
   - Navigate to: `http://localhost:5000/test-wasm.html`
   - Click "Load WASM Module" and verify it loads successfully

3. **Check application status**:
   - Open the main application
   - Look at the status bar - "WASM" should show ✓ if loaded

## Common Issues

### "Go is not installed"

**Solution**: Install Go from https://go.dev/dl/

**macOS**:
```bash
brew install go
```

**Linux**:
```bash
sudo apt install golang  # Debian/Ubuntu
sudo yum install golang  # RHEL/CentOS
```

### "command not found: sointu-wasm"

**Solution**: The `sointu-wasm` command should exist in the Sointu repository. If it doesn't:

1. Check if it exists: `ls sointu/cmd/sointu-wasm/`
2. If missing, it may need to be created (see `SOINTU_WASM_MODIFICATION.md`)
3. The build script should handle this automatically

### "WASM file is too large"

**Solution**: Use TinyGo for smaller builds:
```bash
# Install TinyGo (see https://tinygo.org/getting-started/)
tinygo build -o sointu.wasm -target wasi ./cmd/sointu-play
```

### "WASM module fails to load in browser"

**Solutions**:
1. Check browser console for errors
2. Verify file is served with correct MIME type (`application/wasm`)
3. Ensure file path is correct: `/wasm/sointu.wasm`
4. Check that `wasm_exec.js` is loaded if using Go's standard WASM runtime

### "Functions not exported"

**Solution**: The `sointu-wasm` command should export the required functions. If you see this error:

1. Verify `sointu/cmd/sointu-wasm/main.go` exists and exports functions
2. Check that the build completed successfully
3. Verify `wasm_exec.js` is loaded (required for Go WASM)
4. Check browser console for specific error messages

## Next Steps

After building the WASM module:

1. **Test the integration**: Use `/test-wasm.html` to verify loading and playback
2. **Check progress bar**: Verify visual progress updates during compilation
3. **Test audio playback**: Load and play the example song (`physics_girl_st.yml`)
4. **Enable WASM rendering**: Check "Use WebAssembly Rendering" in Settings
5. **Render music**: Create a project with a Sointu YAML song and render it

## Additional Resources

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [TinyGo WebAssembly Guide](https://tinygo.org/docs/guides/webassembly/wasm/)
- [WebAssembly Documentation](https://webassembly.org/)

