# Heroku Deployment Validation Checklist

This document validates that the 4kampf Web application can run on Heroku.

## ✅ Completed Validations

### 1. Port Configuration
- ✅ **Status**: Fixed
- ✅ **Implementation**: `Program.cs` reads `PORT` environment variable
- ✅ **Heroku Compatibility**: Heroku sets `PORT` automatically
- **Code**: `var port = Environment.GetEnvironmentVariable("PORT");`

### 2. HTTPS Redirection
- ✅ **Status**: Fixed
- ✅ **Implementation**: Skips HTTPS redirection when `PORT` env var is set (Heroku)
- ✅ **Heroku Compatibility**: Heroku handles HTTPS at load balancer
- **Code**: Conditional `app.UseHttpsRedirection()` based on `PORT` env var

### 3. Procfile
- ✅ **Status**: Created
- ✅ **File**: `Procfile`
- ✅ **Content**: `web: cd 4kampf.Web && dotnet run --urls "http://0.0.0.0:${PORT:-5000}"`
- **Note**: Assumes app is in `4kampf.Web` subdirectory

### 4. Build Configuration
- ✅ **Status**: Configured
- ✅ **Buildpack**: `.buildpacks` file references official Heroku .NET Core buildpack
- ✅ **Project File**: Includes `wwwroot` in publish
- **Note**: May need to verify .NET 10.0 support in buildpack

### 5. Environment Variables
- ✅ **Status**: Supported
- ✅ **PORT**: Automatically read from environment
- ✅ **ASPNETCORE_ENVIRONMENT**: Supported via Heroku config
- ✅ **SOINTU_PATH**: Configurable via `appsettings.json` or env var
- ✅ **PROJECTS_DIRECTORY**: Configurable for custom storage location

### 6. File System Handling
- ⚠️ **Status**: Documented limitation
- ⚠️ **Issue**: Heroku filesystem is ephemeral
- ✅ **Mitigation**: `ProjectFileService` logs warning about ephemeral storage
- ✅ **Solution**: Documented use of external storage (S3, database)
- **Code**: `_logger?.LogInformation("Projects directory: {Directory} (Note: Ephemeral on Heroku)")`

### 7. Static Files
- ✅ **Status**: Configured
- ✅ **Implementation**: `wwwroot` included in `.csproj`
- ✅ **Heroku Compatibility**: Static files served correctly

### 8. Production Configuration
- ✅ **Status**: Created
- ✅ **File**: `appsettings.Production.json`
- ✅ **Content**: Production logging and Kestrel configuration

## ⚠️ Known Limitations

### 1. Ephemeral File System
**Issue**: Files saved to `wwwroot/projects/` are lost on dyno restart.

**Impact**:
- Project files not persisted
- Rendered WAV files temporary
- Envelope data temporary

**Solutions**:
1. Use external storage (AWS S3, Azure Blob)
2. Use database for metadata (Heroku Postgres)
3. Use browser localStorage (client-side only)
4. Accept ephemeral nature for demo/testing

**Status**: Documented in `HEROKU_DEPLOYMENT.md`

### 2. Sointu Dependency
**Issue**: Sointu must be installed and available at runtime.

**Impact**:
- Music rendering unavailable if Sointu not found
- App still works (shaders, preview, etc.)

**Solutions**:
1. Custom buildpack to install Sointu
2. Pre-built binaries in repository
3. Multi-buildpack (Go + .NET)
4. Disable music features

**Status**: Documented in `HEROKU_DEPLOYMENT.md`

### 3. Build Time
**Issue**: First build may take 5-10 minutes.

**Impact**: Longer deployment times.

**Mitigation**: Subsequent builds are faster due to caching.

**Status**: Expected behavior, documented

## 🔧 Required Configuration

### Minimum Heroku Setup

```bash
# 1. Create app
heroku create your-app-name

# 2. Set buildpack
heroku buildpacks:set heroku/dotnetcore

# 3. Set environment
heroku config:set ASPNETCORE_ENVIRONMENT=Production

# 4. Deploy
git push heroku main
```

### Optional Configuration

```bash
# For Sointu support (if installed)
heroku config:set SOINTU_PATH=/app/sointu/bin

# For custom project storage
heroku config:set PROJECTS_DIRECTORY=/tmp/projects
```

## 📋 Testing Checklist

Before deploying to production, test:

- [ ] App starts successfully
- [ ] Static files load (CSS, JS, images)
- [ ] WebGL canvas renders
- [ ] Monaco editor loads
- [ ] Shader compilation works
- [ ] Camera controls function
- [ ] Project creation/saving (ephemeral)
- [ ] Music rendering (if Sointu available)
- [ ] Error handling (graceful degradation)

## 🚀 Deployment Steps

1. **Validate locally**:
   ```bash
   PORT=5000 dotnet run
   ```

2. **Test Procfile**:
   ```bash
   foreman start
   ```

3. **Deploy to Heroku**:
   ```bash
   git push heroku main
   ```

4. **Monitor logs**:
   ```bash
   heroku logs --tail
   ```

## 📝 Notes

- **Procfile location**: Assumes app is in `4kampf.Web` subdirectory. If deploying from root, adjust Procfile.
- **Buildpack**: Verify .NET 10.0 support. May need to update buildpack or use Docker.
- **Dyno type**: Eco dynos sleep after 30min inactivity. Use Basic or Standard dynos for always-on production.
- **Pricing**: Heroku no longer offers a free tier. All plans require payment. See [Heroku Pricing](https://www.heroku.com/pricing).
- **Scaling**: App is stateless (except ephemeral files), can scale horizontally.

## 🔗 References

- [Heroku Deployment Guide](./HEROKU_DEPLOYMENT.md)
- [Heroku .NET Buildpack](https://devcenter.heroku.com/articles/dotnet-core-support)
- [Heroku File System](https://devcenter.heroku.com/articles/dynos#ephemeral-filesystem)

