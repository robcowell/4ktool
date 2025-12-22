# 4kampf Web Edition

A cross-platform web-based development environment for creating 4k intros (demoscene productions). This is a modern web port of the original Windows-only 4kampf application, featuring real-time shader editing, music synthesis, and WebGL rendering.

## 🎯 Project Overview

4kampf Web Edition brings the power of 4k intro development to any platform with a modern web browser. It combines:
- **WebGL** for real-time 3D graphics rendering
- **WebAudio** for music playback and synchronization
- **Sointu** (cross-platform 4klang fork) for music synthesis
- **Monaco Editor** (VS Code's editor) for syntax-highlighted shader editing
- **Blazor Server** for a responsive, interactive UI

### What is 4kampf?

4kampf is a development tool for creating "4k intros" - demoscene productions that are exactly 4096 bytes or less. These productions combine real-time graphics (shaders) with synthesized music, all packed into a tiny executable.

## ✨ Features

### ✅ Implemented

- **Real-time Shader Editing**
  - Syntax-highlighted GLSL editing with Monaco Editor
  - Live compilation and preview
  - Support for vertex, fragment, and post-process shaders
  - Real-time error reporting

- **WebGL Rendering**
  - Full WebGL 2.0 support
  - Real-time shader compilation
  - Camera controls (WASD + mouse look)
  - Standard uniforms (resolution, time, camera position/rotation)
  - Envelope sync for music-driven visuals

- **Music Synthesis & Playback**
  - **Sointu** integration (cross-platform 4klang fork)
  - YAML-based song files
  - Automatic envelope generation
  - WebAudio playback with synchronization
  - Envelope data for shader sync

- **Project Management**
  - JSON-based project files
  - Save/load projects
  - Project-specific file organization
  - Settings persistence

- **User Interface**
  - Modern, responsive web UI
  - Settings panel with all project options
  - Status bar with real-time indicators
  - Log panel for build/compile messages
  - Music player controls

### 🚧 In Progress / Planned

- Sointu WebAssembly support for browser-based synthesis
- Full project import/export
- Git integration (BitBucket)
- Advanced camera controls (freefly, lockfly, etc.)
- Toolbar buttons matching original UI
- Screenshot functionality
- Color helper tools

### ✅ Storage Features

- **AWS S3 Integration**: Automatic S3 storage for Heroku deployments
- **Local Storage**: File system storage for local development
- **Automatic Selection**: Chooses storage backend based on configuration
- **Persistent Storage**: Projects persist across dyno restarts on Heroku

## 🏗️ Architecture & Design Choices

### Technology Stack

- **Backend**: ASP.NET Core 10.0 with Blazor Server
- **Frontend**: Blazor Server Components, Bootstrap 5
- **Rendering**: WebGL 2.0 (via JavaScript interop)
- **Audio**: WebAudio API (via JavaScript interop)
- **Editor**: Monaco Editor (VS Code's editor engine)
- **Music**: Sointu (cross-platform 4klang fork)

### Design Decisions

#### 1. **Blazor Server over Blazor WebAssembly**
   - **Reason**: Faster initial load, better server-side integration
   - **Trade-off**: Requires server connection, but enables server-side Sointu rendering
   - **Future**: Can add WebAssembly support for offline mode

#### 2. **Sointu over 4klang**
   - **Reason**: Cross-platform compatibility (Windows, Mac, Linux)
   - **Benefit**: Modern toolchain, WebAssembly support, active development
   - **Migration**: Drop-in replacement for 4klang functionality

#### 3. **JavaScript Interop for WebGL/WebAudio**
   - **Reason**: Direct browser API access, better performance
   - **Approach**: Thin JavaScript wrappers, C# services for orchestration
   - **Benefit**: Maintains C# codebase while leveraging browser APIs

#### 4. **Monaco Editor over Custom Editor**
   - **Reason**: Industry-standard, feature-rich, well-maintained
   - **Benefit**: Syntax highlighting, IntelliSense, familiar UX
   - **Trade-off**: Larger bundle size, but loaded from CDN

#### 5. **JSON Project Files over XML**
   - **Reason**: Modern, human-readable, easier to parse
   - **Benefit**: Better web compatibility, easier debugging
   - **Migration**: Can convert from original `.kml` format if needed

#### 6. **Server-Side Music Rendering (MVP)**
   - **Reason**: Faster to implement, leverages existing Sointu tools
   - **Future**: Add WebAssembly support for client-side rendering
   - **Benefit**: Works immediately, no browser compatibility issues

### Project Structure

```
4ktool/
├── 4kampf/              # Original Windows application (reference)
├── 4kampf.Web/          # Web application
│   ├── Components/      # Blazor components
│   ├── Models/          # Data models
│   ├── Services/        # Business logic services
│   └── wwwroot/         # Static assets (JS, CSS)
└── 4kampf.Shared/       # Shared library (future)
```

## 🚀 Build & Run Instructions

### Prerequisites

- **.NET SDK 10.0** or later
- **Node.js** (optional, for development tools)
- **Sointu** (for music synthesis) - see [Sointu Installation](#sointu-installation)

### Building the Project

```bash
# Clone the repository
git clone <repository-url>
cd 4ktool

# Restore dependencies and build
dotnet restore
dotnet build

# Build web application specifically
cd 4kampf.Web
dotnet build
```

### Running the Application

```bash
# From project root
cd 4kampf.Web
dotnet run

# Or specify a port
dotnet run --urls "http://localhost:5000"
```

The application will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001` (if configured)

### Development Mode

For detailed error messages and hot reload:

```bash
dotnet watch run
```

### Sointu Installation

Sointu is required for music synthesis. See [4kampf.Web/INSTALL_SOINTU.md](4kampf.Web/INSTALL_SOINTU.md) for detailed cross-platform instructions.

**Quick Install**:

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

**Manual Installation**: See [INSTALL_SOINTU.md](4kampf.Web/INSTALL_SOINTU.md) for step-by-step instructions.

## 📖 Usage Instructions

### Getting Started

1. **Start the Application**
   ```bash
   cd 4kampf.Web
   dotnet run
   ```

2. **Open in Browser**
   - Navigate to `http://localhost:5000`
   - The application loads with a default sample project

3. **Edit Shaders**
   - Click on the **Vertex**, **Fragment**, or **Post-Process** tabs
   - Edit shader code in the Monaco editor
   - Changes are automatically compiled and previewed

4. **Control Camera**
   - Click on the canvas to focus it
   - **WASD** keys to move
   - **Right-click + drag** for mouse look
   - **Shift** for faster movement

5. **Render Music**
   - Create or select a Sointu YAML song file
   - Go to **Build → Render Music**
   - Envelopes are automatically generated
   - Music loads into the player

### Project Workflow

1. **Create New Project**
   - **File → New** creates a new project with default shaders
   - Project is saved in `wwwroot/projects/`

2. **Edit Shaders**
   - Edit vertex, fragment, and post-process shaders
   - Real-time compilation and preview
   - Errors shown in log panel

3. **Configure Settings**
   - Click **Settings** in the menu bar
   - Toggle standard uniforms, camera controls, envelope sync
   - Select synthesizer (Sointu/4klang/Clinkster/Oidos)

4. **Save Project**
   - **File → Save All** saves project and shaders
   - Project stored as JSON in `wwwroot/projects/ProjectName/`

5. **Render Music**
   - **Build → Render Music** compiles and renders audio
   - Generates WAV file and envelope data
   - Music loads into player automatically

### Keyboard Shortcuts

- **W/A/S/D**: Camera movement
- **Right-click + Drag**: Mouse look
- **Shift**: Faster camera movement
- **Tab**: Focus canvas (for keyboard input)

### Settings Panel

Access via **Settings** menu item:

- **Rendering**
  - Enable Standard Uniforms: `vec3 u {resX, resY, time}`
  - Enable Camera Controls: WASD + mouse look
  - Enable Envelope Sync: `float ev[MAX_INSTRUMENTS]`

- **Music Synthesis**
  - Synthesizer selection (Sointu/4klang/Clinkster/Oidos)
  - Sointu availability status

- **Shaders**
  - Use Post-Process Shader
  - Use Custom Vertex Shader

- **Playback**
  - Loop Music

## 🎨 Shader Development

### Shader Format

Shaders use **GLSL ES 300** (WebGL 2.0 compatible):

```glsl
#version 300 es
precision mediump float;

in vec2 uv;
out vec4 fragColor;

uniform vec3 u; // {resX, resY, time}
uniform vec3 cp, cr; // Camera position and rotation

void main() {
    fragColor = vec4(uv, 0.0, 1.0);
}
```

### Standard Uniforms

When "Enable Standard Uniforms" is enabled:
- `vec3 u`: `{resolutionX, resolutionY, time}`

When "Enable Camera Controls" is enabled:
- `vec3 cp`: Camera position `{x, y, z}`
- `vec3 cr`: Camera rotation `{x, y, z}`

When "Enable Envelope Sync" is enabled:
- `float ev[MAX_INSTRUMENTS]`: Envelope values per instrument

### Default Sample Project

The application loads with a default raymarching scene demonstrating:
- Domain repetition
- Ray-sphere intersection
- Ambient occlusion
- Camera controls
- Post-processing effects

## 🔧 Configuration

### appsettings.json

```json
{
  "Sointu": {
    "Path": "/path/to/sointu/bin"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Project Storage

Projects are stored in:
- **Location**: `wwwroot/projects/`
- **Format**: JSON files (`ProjectName.json`)
- **Structure**: Each project has its own directory with shaders, music, and outputs

## 🐛 Troubleshooting

See [4kampf.Web/TROUBLESHOOTING.md](4kampf.Web/TROUBLESHOOTING.md) for common issues and solutions.

### Common Issues

1. **Sointu Not Found**
   - Install Sointu and add to PATH
   - Or configure path in `appsettings.json`

2. **WebGL Not Initializing**
   - Check browser console for errors
   - Ensure WebGL2 is supported
   - Try a different browser

3. **Shader Compilation Errors**
   - Check log panel for error messages
   - Ensure GLSL ES 300 syntax
   - Verify precision qualifiers are set

4. **Permission Errors (macOS)**
   - See TROUBLESHOOTING.md for .NET SDK permission fixes

## 📋 TODO List

### High Priority

- [ ] **Sointu WebAssembly Support**
  - Build Sointu to WebAssembly
  - Create JavaScript wrapper
  - Integrate with WebAudio for real-time synthesis
  - Enable offline music rendering

- [ ] **Project Import/Export**
  - Import from original `.kml` format
  - Export to Visual Studio project format
  - Project file conversion utilities

- [ ] **Git Integration**
  - BitBucket integration (from original)
  - Git operations UI
  - Commit/push/pull functionality

- [ ] **UI Improvements**
  - Toolbar buttons matching original
  - Advanced camera controls (freefly, lockfly)
  - Screenshot functionality
  - Color helper tools
  - Line number navigation for errors

### Medium Priority

- [ ] **Core Logic Migration**
  - Migrate business logic to shared library
  - Extract reusable components
  - Create shared models

- [ ] **Sointu YAML Editor**
  - UI for editing Sointu song files
  - Visual song editor
  - Pattern editor

- [ ] **Performance Optimization**
  - Optimize render loop
  - Reduce JavaScript interop overhead
  - Cache compiled shaders

- [ ] **Testing**
  - Unit tests for services
  - Integration tests for rendering
  - E2E tests for UI workflows

### Low Priority

- [ ] **Documentation**
  - API documentation
  - Shader examples library
  - Video tutorials

- [ ] **Advanced Features**
  - Multi-project workspace
  - Project templates
  - Export to various formats
  - Plugin system

- [ ] **Accessibility**
  - Keyboard navigation
  - Screen reader support
  - High contrast mode

## 🤝 Contributing

Contributions are welcome! Areas that need help:

1. **Sointu Integration**: WebAssembly support, envelope generation improvements
2. **UI/UX**: Matching original functionality, new features
3. **Documentation**: Examples, tutorials, API docs
4. **Testing**: Unit tests, integration tests
5. **Performance**: Optimization, profiling

## 📚 Additional Documentation

- [Sointu Integration Guide](4kampf.Web/README_SOINTU.md) - Sointu setup and usage
- [Sointu Integration Plan](4kampf.Web/SOINTU_INTEGRATION.md) - Technical integration details
- [Troubleshooting Guide](4kampf.Web/TROUBLESHOOTING.md) - Common issues and solutions

## 🔗 Links

- **Sointu**: https://github.com/vsariola/sointu
- **Original 4klang**: (legacy, Windows-only)
- **Blazor Documentation**: https://learn.microsoft.com/aspnet/core/blazor/
- **WebGL Reference**: https://developer.mozilla.org/en-US/docs/Web/API/WebGL_API

## 📄 License

[Add license information here]

## 🙏 Acknowledgments

- Original 4kampf by Fell and Skomp (2012-2015)
- Sointu by Veikko Sariola (pestis/bC!) and contributors
- 4klang by Dominik Ries (gopher/Alcatraz) & Paul Kraus (pOWL/Alcatraz)

---

**Status**: 🟢 Active Development - Core features implemented, enhancements in progress

