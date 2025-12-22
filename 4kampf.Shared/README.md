# 4kampf.Shared

Shared library containing core business logic, models, and utilities used by both the Windows application and the web application.

## Contents

### Models

- **`Project`**: Unified project model supporting both Windows (XML) and Web (JSON) serialization
- **`Synth`**: Synthesizer type enumeration (Sointu, 4klang, Clinkster, Oidos)

### Math

- **`Vector3f`**: 3D vector class with mathematical operations (dot product, cross product, normalization, etc.)

### Utilities

- **`ColorHandler`**: RGB/HSV color space conversion utilities
- **`ShaderUtils`**: Shader code cleaning and processing utilities
- **`ProjectExtensions`**: Extension methods for Project model (enum conversions, etc.)

## Usage

### In Web Application

```csharp
using _4kampf.Shared.Models;
using _4kampf.Shared.Utilities;

var project = new Project
{
    Name = "My Project",
    Synth = Synth.Sointu,
    EnableStandardUniforms = true
};

string synthId = project.Synth.ToStringIdentifier(); // "sointu"
Synth synth = "sointu".ToSynthEnum(); // Synth.Sointu
```

### In Windows Application

```csharp
using _4kampf.Shared.Models;
using _4kampf.Shared.Math;

var project = new Project
{
    Name = "My Project",
    Synth = Synth.Vierklang
};

var vector = new Vector3f(1.0f, 2.0f, 3.0f);
```

## Migration Notes

### From Windows App

The original Windows app used:
- `kampfpanzerin.core.Serialization.Project` → Now `_4kampf.Shared.Models.Project`
- `kampfpanzerin.Vector3f` → Now `_4kampf.Shared.Math.Vector3f`
- `kampfpanzerin.core.Serialization.Synth` → Now `_4kampf.Shared.Models.Synth`

### From Web App

The original web app used:
- `_4kampf.Web.Models.ProjectModel` → Now `_4kampf.Shared.Models.Project`
- String-based synth identifiers → Now `Synth` enum with extension methods

## Benefits

1. **Code Reuse**: Single source of truth for business logic
2. **Consistency**: Same models and utilities across platforms
3. **Maintainability**: Changes to core logic only need to be made once
4. **Type Safety**: Enum-based synth selection instead of strings
5. **Extensibility**: Easy to add new features that work on both platforms

