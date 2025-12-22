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

# Build
go build -o sointu.wasm ./cmd/sointu-play
```

**Note**: If `./cmd/sointu-play` doesn't exist, try:
- `./cmd/sointu`
- `./cmd/sointu-render`
- Check available commands: `ls cmd/`

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

### "command not found: sointu-play"

**Solution**: The Sointu repository structure may have changed. Check available commands:
```bash
cd sointu
ls cmd/
```

Then build the appropriate command:
```bash
go build -o sointu.wasm ./cmd/[command-name]
```

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

**Solution**: The JavaScript interop (`sointu-wasm-interop.js`) expects specific function exports. You may need to:
1. Modify Sointu's Go code to export the required functions
2. Create a wrapper that bridges Sointu's API to the expected interface
3. Update `sointu-wasm-interop.js` to match Sointu's actual exports

## Next Steps

After building the WASM module:

1. **Test the integration**: Use `/test-wasm.html` to verify loading
2. **Enable WASM rendering**: Check "Use WebAssembly Rendering" in Settings
3. **Render music**: Create a project with a Sointu YAML song and render it

## Additional Resources

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Go WebAssembly Documentation](https://go.dev/doc/asm)
- [TinyGo WebAssembly Guide](https://tinygo.org/docs/guides/webassembly/wasm/)
- [WebAssembly Documentation](https://webassembly.org/)

