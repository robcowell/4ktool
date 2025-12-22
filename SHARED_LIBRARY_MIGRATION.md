# Shared Library Migration Summary

This document summarizes the migration of core C# business logic to the `4kampf.Shared` library.

## ✅ Completed Migration

### Models

1. **`Project` Model** (`4kampf.Shared/Models/Project.cs`)
   - Unified project model replacing both `kampfpanzerin.core.Serialization.Project` (Windows) and `_4kampf.Web.Models.ProjectModel` (Web)
   - Supports JSON serialization (web) and can be extended for XML (Windows)
   - Includes legacy property mappings for backward compatibility
   - Added `FixLegacy()` method for migrating old project files

2. **`Synth` Enum** (`4kampf.Shared/Models/Synth.cs`)
   - Unified synthesizer enumeration
   - Values: `Undefined`, `Vierklang`, `Clinkster`, `Oidos`, `Sointu`
   - Replaces string-based synth identifiers in web app

### Math Utilities

3. **`Vector3f`** (`4kampf.Shared/Math/Vector3f.cs`)
   - 3D vector class with mathematical operations
   - Platform-agnostic implementation
   - Includes legacy property names (`x`, `y`, `z`) for compatibility
   - Operations: dot product, cross product, normalization, magnitude, etc.

### Utilities

4. **`ColorHandler`** (`4kampf.Shared/Utilities/ColorHandler.cs`)
   - RGB/HSV color space conversion utilities
   - Platform-agnostic (no Windows-specific dependencies)

5. **`ShaderUtils`** (`4kampf.Shared/Utilities/ShaderUtils.cs`)
   - Shader code cleaning and processing
   - Extracted from `BuildUtils.CleanShader()` method
   - Handles AUTOREP directives, comment removal, etc.

6. **`ProjectExtensions`** (`4kampf.Shared/Utilities/ProjectExtensions.cs`)
   - Extension methods for `Synth` enum ↔ string conversions
   - `ToStringIdentifier()`: Converts enum to string ("sointu", "4klang", etc.)
   - `ToSynthEnum()`: Converts string to enum

## Updated Projects

### Web Application (`4kampf.Web`)

**Updated Files**:
- `Services/ProjectFileService.cs`: Now uses `_4kampf.Shared.Models.Project`
- `Components/Pages/Home.razor`: Updated to use shared `Project` model and enum conversions
- `Components/_Imports.razor`: Added shared library imports
- `4kampf.Web.csproj`: Added project reference to `4kampf.Shared`

**Changes**:
- Replaced `ProjectModel` with `Project`
- Replaced string-based `Synth` with enum
- Added enum ↔ string conversion helpers

### Windows Application (`4kampf`)

**Status**: Not yet migrated (requires manual update due to old-style .csproj format)

**Recommended Migration Steps**:
1. Add project reference to `4kampf.Shared` in `4kampf.csproj`
2. Update `using` statements:
   - `kampfpanzerin.core.Serialization.Project` → `_4kampf.Shared.Models.Project`
   - `kampfpanzerin.Vector3f` → `_4kampf.Shared.Math.Vector3f`
   - `kampfpanzerin.core.Serialization.Synth` → `_4kampf.Shared.Models.Synth`
3. Update XML serialization to use shared `Project` model
4. Test project save/load functionality

## Benefits

1. **Code Reuse**: Single source of truth for business logic
2. **Consistency**: Same models across Windows and Web applications
3. **Type Safety**: Enum-based synth selection instead of error-prone strings
4. **Maintainability**: Changes to core logic only need to be made once
5. **Extensibility**: Easy to add new features that work on both platforms

## File Structure

```
4kampf.Shared/
├── Models/
│   ├── Project.cs          # Unified project model
│   └── Synth.cs            # Synthesizer enumeration
├── Math/
│   └── Vector3f.cs         # 3D vector class
├── Utilities/
│   ├── ColorHandler.cs     # RGB/HSV conversion
│   ├── ShaderUtils.cs      # Shader processing
│   └── ProjectExtensions.cs # Extension methods
└── README.md               # Library documentation
```

## Testing

To verify the migration:

1. **Web Application**:
   ```bash
   cd 4kampf.Web
   dotnet build
   dotnet run
   ```
   - Verify project creation/saving works
   - Verify synth selection works
   - Verify enum conversions work correctly

2. **Shared Library**:
   ```bash
   cd 4kampf.Shared
   dotnet build
   ```

## Next Steps

1. **Windows App Migration** (Optional):
   - Add project reference
   - Update using statements
   - Test compatibility

2. **Additional Utilities** (Future):
   - Consider migrating `BuildUtils` export logic (if platform-agnostic)
   - Consider migrating camera math (if platform-agnostic)

3. **Documentation**:
   - Update Windows app documentation to reference shared library
   - Add migration guide for existing projects

