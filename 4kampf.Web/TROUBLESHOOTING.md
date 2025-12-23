# Troubleshooting Guide

## .NET SDK Permission Error

If you encounter this error:
```
error : Access to the path '/usr/local/share/dotnet/sdk/10.0.101/trustedroots/codesignctl.pem' is denied.
```

This is a known issue with .NET SDK on macOS where the SDK files are owned by root.

### Solution 1: Fix Permissions (Recommended)

Run this command to fix permissions on the .NET SDK directory:

```bash
sudo chown -R $(whoami) /usr/local/share/dotnet
```

Then try building again:
```bash
dotnet build
```

### Solution 2: Use User-Local .NET Installation

Install .NET SDK via Homebrew or download from Microsoft, which installs to a user-writable location:

```bash
# Via Homebrew
brew install --cask dotnet-sdk

# Or download from Microsoft
# https://dotnet.microsoft.com/download
```

Then ensure your PATH uses the user-local installation first:
```bash
export PATH="$HOME/.dotnet:$PATH"
```

### Solution 3: Reinstall .NET SDK

If permissions can't be fixed, reinstall the .NET SDK:

```bash
# Remove old installation
sudo rm -rf /usr/local/share/dotnet

# Reinstall from Microsoft
# Download from https://dotnet.microsoft.com/download
```

### Solution 4: Use Docker/Container

If you can't fix permissions, use Docker:

```bash
docker run -it -v $(pwd):/workspace mcr.microsoft.com/dotnet/sdk:10.0 bash
cd /workspace/4kampf.Web
dotnet build
```

## Other Common Issues

### Sointu Not Found

If `SointuService` reports that Sointu is not available:

1. Install Sointu (see [README_SOINTU.md](./README_SOINTU.md))
2. Add Sointu to your PATH, or
3. Configure the path in `appsettings.json`:
   ```json
   {
     "Sointu": {
       "Path": "/path/to/sointu/bin"
     }
   }
   ```

### WebGL Not Initializing

- Ensure your browser supports WebGL2 or WebGL
- Check browser console for errors
- Verify `webgl-interop.js` is loaded in `App.razor`

### CodeMirror Editor Not Loading

- Check browser console for CDN errors
- Verify internet connection (CodeMirror loads from CDN)
- Check that `codemirror-loader.js` is included in `App.razor`

