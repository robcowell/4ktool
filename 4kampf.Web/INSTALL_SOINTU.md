# Installing Sointu for 4kampf Web

Sointu is required for music synthesis. This guide covers installation on all platforms.

## Prerequisites

**Go Programming Language** must be installed first:

- **Windows**: Download from [go.dev/dl](https://go.dev/dl/) or use `choco install golang`
- **macOS**: `brew install go` or download from [go.dev/dl](https://go.dev/dl/)
- **Linux**: Use your package manager (`apt install golang`, `yum install golang`, etc.) or download from [go.dev/dl](https://go.dev/dl/)

Verify Go installation:
```bash
go version
```

## Installation Methods

### Method 1: Automated Scripts (Recommended)

#### Windows

**PowerShell** (recommended):
```powershell
cd 4kampf.Web
.\install-sointu.ps1
```

**Command Prompt**:
```cmd
cd 4kampf.Web
install-sointu.bat
```

#### macOS / Linux

```bash
cd 4kampf.Web
./install-sointu.sh
```

### Method 2: Manual Installation

1. **Clone Sointu repository**:
   ```bash
   git clone https://github.com/vsariola/sointu.git
   cd sointu
   ```

2. **Build Sointu**:
   ```bash
   go build ./cmd/sointu-compile
   go build ./cmd/sointu-play
   ```

3. **Install to PATH**:

   **Windows**:
   - Copy `sointu-compile.exe` and `sointu-play.exe` to a directory in your PATH
   - Or add the directory to your PATH:
     - System Properties → Environment Variables
     - Add directory to User PATH

   **macOS / Linux**:
   ```bash
   # Install to ~/.local/bin
   mkdir -p ~/.local/bin
   cp sointu-compile sointu-play ~/.local/bin
   
   # Add to PATH (add to ~/.zshrc, ~/.bashrc, or ~/.profile)
   export PATH="$PATH:$HOME/.local/bin"
   source ~/.zshrc  # or ~/.bashrc
   ```

### Method 3: Configure Custom Path

If Sointu is installed in a non-standard location, configure it in `appsettings.json`:

```json
{
  "Sointu": {
    "Path": "C:\\Program Files\\sointu"
  }
}
```

**Note**: Use forward slashes or escaped backslashes on Windows:
- `"C:/Program Files/sointu"` ✅
- `"C:\\Program Files\\sointu"` ✅

## Verification

### Check Installation

**Windows**:
```cmd
where sointu-compile
sointu-compile.exe --version
```

**macOS / Linux**:
```bash
which sointu-compile
sointu-compile --version
```

### Test in Application

1. **Start the application**:
   ```bash
   cd 4kampf.Web
   dotnet run
   ```

2. **Check status**:
   - Open `http://localhost:5000`
   - Look at status bar: should show "Sointu: ✓"
   - Or check log panel for "✓ Sointu is available"

3. **Test via API**:
   ```bash
   # Check status
   curl http://localhost:5000/api/SointuTest/status
   
   # Test compilation
   curl -X POST http://localhost:5000/api/SointuTest/test-compile
   ```

## Troubleshooting

### "Sointu not found" Error

**Symptoms**: Status shows "✗ Not Available"

**Solutions**:
1. Verify Sointu is in PATH:
   - Windows: `where sointu-compile`
   - macOS/Linux: `which sointu-compile`

2. Check application logs for detection messages

3. Configure explicit path in `appsettings.json`:
   ```json
   {
     "Sointu": {
       "Path": "/path/to/sointu"
     }
   }
   ```

4. Restart the application after adding to PATH

### Go Not Found

**Symptoms**: Installation script fails with "Go is not installed"

**Solutions**:
- Install Go from [go.dev/dl](https://go.dev/dl/)
- Verify: `go version`
- Ensure Go is in PATH

### Build Failures

**Symptoms**: `go build` fails

**Solutions**:
1. Ensure Go is properly installed
2. Check internet connection (needs to download dependencies)
3. Try: `go mod download` in the sointu directory
4. Check Go version: `go version` (should be 1.18+)

### Permission Errors

**macOS / Linux**:
```bash
# Make script executable
chmod +x install-sointu.sh

# If installing to system directory, may need sudo
sudo cp sointu-compile sointu-play /usr/local/bin/
```

**Windows**:
- Run PowerShell as Administrator if needed
- Or install to user directory (`%USERPROFILE%\bin`)

## Platform-Specific Notes

### Windows

- Executables are `.exe` files
- PATH separator is `;` (semicolon)
- Use PowerShell or Command Prompt scripts
- May need to restart terminal after adding to PATH

### macOS

- Executables have no extension
- PATH separator is `:` (colon)
- May need to allow execution: `chmod +x install-sointu.sh`
- Homebrew installation: `brew install go` (if available)

### Linux

- Executables have no extension
- PATH separator is `:` (colon)
- Package manager installation preferred
- May need `sudo` for system-wide installation

## Next Steps

After successful installation:
1. Verify Sointu is detected by the application
2. Create a test Sointu YAML song file
3. Test music rendering workflow
4. See [SOINTU_TEST_GUIDE.md](./SOINTU_TEST_GUIDE.md) for testing

## Additional Resources

- [Sointu GitHub Repository](https://github.com/vsariola/sointu)
- [Go Installation Guide](https://go.dev/doc/install)
- [Sointu Documentation](https://github.com/vsariola/sointu#readme)

