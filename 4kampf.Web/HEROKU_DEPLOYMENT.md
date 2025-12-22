# Heroku Deployment Guide for 4kampf Web

This guide covers deploying the 4kampf Web application to Heroku.

## ⚠️ Important Limitations

### 1. **Ephemeral File System**
Heroku dynos have an **ephemeral filesystem** - files written to disk are lost when the dyno restarts. This affects:
- **Project files** saved to `wwwroot/projects/` will be lost on restart
- **Rendered WAV files** and envelope data are temporary
- **Solution**: Consider using external storage (S3, database) for persistent data

### 2. **Sointu Dependency**
Sointu must be installed and available at runtime. Options:
- **Option A**: Pre-install Sointu in a custom buildpack
- **Option B**: Use a multi-buildpack setup with Go + Sointu
- **Option C**: Bundle Sointu binaries in the repository (not recommended for size)
- **Option D**: Disable music rendering (app works without it)

### 3. **Build Time**
- .NET 10.0 buildpack must support the target framework
- First build may take 5-10 minutes
- Subsequent builds are faster due to caching

## Prerequisites

1. **Heroku CLI** installed: [heroku.com/cli](https://devcenter.heroku.com/articles/heroku-cli)
2. **Git** repository initialized
3. **Heroku account** with a paid plan (free tier no longer available)

## Deployment Steps

### Step 1: Create Heroku App

```bash
# Login to Heroku
heroku login

# Create a new app (or use existing)
heroku create your-app-name

# Or create with a specific region
heroku create your-app-name --region us
```

### Step 2: Set Buildpacks

Heroku needs to know how to build your app. Use the .NET Core buildpack:

```bash
# Set the .NET buildpack (official Heroku buildpack)
heroku buildpacks:set heroku/dotnetcore

# If you need Sointu, add Go buildpack first (for building Sointu)
heroku buildpacks:add heroku/go
heroku buildpacks:add heroku/dotnetcore
```

**Note**: The order matters - buildpacks run in sequence.

### Step 3: Configure Environment Variables

```bash
# Set .NET environment
heroku config:set ASPNETCORE_ENVIRONMENT=Production

# Optional: Install Bucketeer for managed S3 storage (recommended)
heroku addons:create bucketeer:hobbyist

# Optional: Configure Sointu path if installed
heroku config:set SOINTU_PATH=/app/sointu/bin

# Optional: Disable HTTPS redirection (Heroku handles this)
heroku config:set DISABLE_HTTPS_REDIRECT=true
```

### Step 4: Deploy

```bash
# Deploy to Heroku
git push heroku main

# Or if using master branch
git push heroku master
```

### Step 5: Verify Deployment

```bash
# Check app status
heroku ps

# View logs
heroku logs --tail

# Open app in browser
heroku open
```

## Configuration Options

### Port Configuration
The app automatically reads the `PORT` environment variable (set by Heroku). No manual configuration needed.

### HTTPS/HTTP
- Heroku handles HTTPS at the load balancer
- The app detects Heroku (via `PORT` env var) and skips HTTPS redirection
- In development, HTTPS redirection still works

### File Storage

The application automatically uses **AWS S3** when configured, or falls back to **local file system** for development.

#### Option 1: Bucketeer (Recommended for Heroku)

**Bucketeer** is a Heroku add-on that provides managed S3 buckets with automatic credential management. This is the easiest option for Heroku deployments.

**Installation**:
```bash
# Install Bucketeer add-on (Hobbyist plan: ~$0.007/hour, max $5/month)
heroku addons:create bucketeer:hobbyist

# Or use a different plan
heroku addons:create bucketeer:micro    # ~$0.021/hour, max $15/month
heroku addons:create bucketeer:small    # ~$0.069/hour, max $50/month
```

**Configuration**: Bucketeer automatically sets these environment variables:
- `BUCKETEER_BUCKET_NAME` - The S3 bucket name
- `BUCKETEER_AWS_ACCESS_KEY_ID` - AWS access key
- `BUCKETEER_AWS_SECRET_ACCESS_KEY` - AWS secret key
- `BUCKETEER_AWS_REGION` - AWS region

**Benefits**:
- ✅ Automatic credential management
- ✅ One-click credential rotation
- ✅ Consolidated billing through Heroku
- ✅ No manual AWS account setup required
- ✅ Safe deletes (on Standard+ plans)

**Note**: The app automatically detects Bucketeer and uses it when `BUCKETEER_BUCKET_NAME` is set. No additional configuration needed!

See [Bucketeer documentation](https://elements.heroku.com/addons/bucketeer) for more details.

#### Option 2: Direct AWS S3

For direct AWS S3 integration (requires your own AWS account):

```bash
# Required: S3 bucket name
heroku config:set AWS_S3_BUCKET=your-bucket-name

# AWS credentials (use IAM roles on EC2, or access keys)
heroku config:set AWS_ACCESS_KEY_ID=your-access-key
heroku config:set AWS_SECRET_ACCESS_KEY=your-secret-key

# Optional: S3 region (defaults to us-east-1)
heroku config:set AWS_REGION=us-west-2

# Optional: S3 prefix for organizing files (defaults to "4kampf/projects")
heroku config:set AWS_S3_PREFIX=4kampf/projects
```

**Note**: The app automatically uses S3 if `AWS_S3_BUCKET` is set. Bucketeer takes precedence if both are configured.

#### Local File System (Development)

When S3 is not configured, the app uses local file system:
- **Development**: `wwwroot/projects/`
- **Custom path**: Set `PROJECTS_DIRECTORY` environment variable

#### Other Storage Options

1. **Heroku Postgres** (for metadata only):
   ```bash
   heroku addons:create heroku-postgresql:mini
   ```

2. **Browser localStorage** (client-side only):
   - Projects stored in browser (not synced across devices)

### Sointu Installation

If you need Sointu on Heroku, you have several options:

#### Option A: Custom Buildpack Script

Create `bin/compile` in your repo:

```bash
#!/bin/bash
# Install Go if not present
if ! command -v go &> /dev/null; then
    echo "Installing Go..."
    # Download and install Go
fi

# Build Sointu
cd /tmp
git clone https://github.com/vsariola/sointu.git
cd sointu
go build ./cmd/sointu-compile
go build ./cmd/sointu-play
mkdir -p $HOME/sointu/bin
cp sointu-compile sointu-play $HOME/sointu/bin
export PATH="$PATH:$HOME/sointu/bin"
```

#### Option B: Pre-built Binaries

1. Build Sointu on a Linux machine
2. Commit binaries to `bin/sointu/` (not recommended - large files)
3. Set `SOINTU_PATH` to `/app/bin/sointu`

#### Option C: Disable Music Features

If music rendering isn't critical:
- App works without Sointu
- Music player shows "Sointu not available"
- Users can still edit shaders and preview

## Troubleshooting

### Build Fails: "No buildpack detected"

**Solution**: Explicitly set buildpack:
```bash
heroku buildpacks:set heroku/dotnetcore
```

### Build Fails: ".NET SDK not found"

**Solution**: Ensure buildpack supports .NET 10.0. Check buildpack documentation or use:
```bash
heroku buildpacks:set heroku/dotnetcore
```

### App Crashes: "Port already in use"

**Solution**: App should use `PORT` env var automatically. Check `Program.cs` for port configuration.

### Files Disappear After Restart

**Expected Behavior**: Heroku's filesystem is ephemeral. Use external storage for persistence.

### Sointu Not Found

**Solution**: 
1. Check if Sointu is installed: `heroku run which sointu-compile`
2. Set `SOINTU_PATH` config var if using custom path
3. Or disable music features (app works without Sointu)

### Static Files Not Loading

**Solution**: Ensure `wwwroot` is included in build. Check `.csproj` for:
```xml
<ItemGroup>
  <Content Include="wwwroot/**" />
</ItemGroup>
```

## Scaling

```bash
# Scale to 1 web dyno
heroku ps:scale web=1

# Scale to multiple dynos
heroku ps:scale web=2
```

## Monitoring

```bash
# View real-time logs
heroku logs --tail

# View specific log lines
heroku logs -n 100

# Check dyno status
heroku ps

# View metrics
heroku metrics
```

## Cost Considerations

**Note**: Heroku no longer offers a free tier. All plans require payment.

- **Eco Dyno**: $5/month per dyno, sleeps after 30min inactivity, suitable for development/testing
- **Basic Dyno**: $7/month per dyno, always-on, no sleep, suitable for small production apps
- **Standard Dyno**: $25+/month per dyno, better performance, more features, suitable for production workloads

For development/testing, Eco dynos are cost-effective. For production, consider Basic or Standard dynos depending on your needs.

See [Heroku Pricing](https://www.heroku.com/pricing) for current pricing information.

## Alternative: Docker Deployment

If buildpacks are problematic, consider using Docker:

1. Create `Dockerfile`
2. Use `heroku container:push` and `heroku container:release`
3. More control over build process

## Next Steps

1. **Set up persistent storage** (S3, database)
2. **Configure custom domain** (if needed)
3. **Set up CI/CD** (GitHub Actions, etc.)
4. **Monitor performance** (New Relic, etc.)
5. **Add error tracking** (Sentry, etc.)

## References

- [Heroku .NET Core Buildpack](https://devcenter.heroku.com/articles/dotnet-core-support)
- [Heroku Node.js Buildpack](https://devcenter.heroku.com/articles/nodejs-support)
- [Heroku File System](https://devcenter.heroku.com/articles/dynos#ephemeral-filesystem)
- [Heroku Configuration](https://devcenter.heroku.com/articles/config-vars)

