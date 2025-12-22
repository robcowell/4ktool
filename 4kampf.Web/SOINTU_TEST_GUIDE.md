# Sointu Integration Test Guide

## Quick Test

### 1. Check Sointu Status via API

```bash
# Start the application
cd 4kampf.Web
dotnet run

# In another terminal, test the status endpoint
curl http://localhost:5000/api/SointuTest/status
```

Expected response if Sointu is available:
```json
{
  "available": true,
  "message": "Sointu is available and ready to use"
}
```

### 2. Test Compilation

```bash
curl -X POST http://localhost:5000/api/SointuTest/test-compile
```

This will:
- Create a simple test YAML song
- Attempt to compile it with Sointu
- Return success/failure status

## Installation Steps

See [INSTALL_SOINTU.md](./INSTALL_SOINTU.md) for detailed cross-platform installation instructions.

### Quick Install

**Windows (PowerShell)**:
```powershell
cd 4kampf.Web
.\install-sointu.ps1
```

**Windows (Command Prompt)**:
```cmd
cd 4kampf.Web
install-sointu.bat
```

**macOS / Linux**:
```bash
cd 4kampf.Web
./install-sointu.sh
```

### Manual Installation

1. Install Go from [go.dev/dl](https://go.dev/dl/)
2. Clone and build Sointu:
   ```bash
   git clone https://github.com/vsariola/sointu.git
   cd sointu
   go build ./cmd/sointu-compile
   go build ./cmd/sointu-play
   ```
3. Add to PATH (see INSTALL_SOINTU.md for platform-specific instructions)

### Configure Custom Path

Add to `appsettings.json`:
```json
{
  "Sointu": {
    "Path": "C:/path/to/sointu"  // Windows: use forward slashes or escaped backslashes
  }
}
```

## Verification Steps

### Step 1: Verify Sointu Installation

```bash
# Check if commands are available
which sointu-compile
which sointu-play

# Test version (if supported)
sointu-compile --version
sointu-play --version
```

### Step 2: Test in Application

1. **Start the web application**:
   ```bash
   cd 4kampf.Web
   dotnet run
   ```

2. **Open browser** to `http://localhost:5000`

3. **Check Status Bar**:
   - Look for "Sointu: ✓" in the status bar
   - If it shows "✗", Sointu is not found

4. **Check Settings Panel**:
   - Click "Settings" in menu
   - Look at "Sointu Status" indicator
   - Should show "✓ Available" if installed

### Step 3: Test Music Rendering

1. **Create a test project**:
   - File → New
   - This creates a new project

2. **Add a Sointu song file**:
   - Create `wwwroot/projects/New Project/song.yml`
   - Use a simple Sointu YAML song (see example below)

3. **Render Music**:
   - Build → Render Music
   - Check log panel for success/error messages

## Example Sointu YAML Song

Create `wwwroot/projects/New Project/song.yml`:

```yaml
song:
  bpm: 120
  rows_per_beat: 4
  length: 64
  patterns:
    - name: pattern1
      rows: 16
      tracks:
        - name: track1
          columns:
            - note: C-4
              length: 4
              velocity: 127
  sequence:
    - pattern: pattern1
      repeat: 4
```

## Troubleshooting

### Sointu Not Found

**Symptoms**: Status shows "✗ Not Available"

**Solutions**:
1. Verify Sointu is in PATH:
   ```bash
   which sointu-compile
   ```

2. Check if SointuService can find it:
   - Look at application logs for "Sointu found at: ..." or "Sointu not found"

3. Try configuring explicit path in `appsettings.json`

### Compilation Fails

**Symptoms**: "Error: Failed to compile song"

**Check**:
1. YAML file syntax is correct
2. Sointu version is compatible
3. Check application logs for detailed error

### Rendering Fails

**Symptoms**: "Error: Failed to render audio"

**Check**:
1. Compilation succeeded (check for `.asm` file)
2. Sointu-play command is available
3. Output directory is writable

## Expected Test Results

### ✅ Success Indicators

- Status API returns `"available": true`
- Status bar shows "Sointu: ✓"
- Settings panel shows "✓ Available"
- Test compilation creates `.asm` file
- Music rendering creates `.wav` file
- Envelope files are generated

### ❌ Failure Indicators

- Status API returns `"available": false`
- Status bar shows "Sointu: ✗"
- Compilation errors in log
- Missing output files

## Next Steps After Verification

Once Sointu is verified working:
1. Create actual song files using Sointu tracker
2. Test full workflow: compile → render → play
3. Verify envelope generation
4. Test envelope sync in shaders

