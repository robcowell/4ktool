# Storage Configuration Guide

The 4kampf Web application supports multiple storage backends for project files, automatically selecting the appropriate one based on configuration.

## Storage Backends

### 1. Bucketeer (Recommended for Heroku)

**When Used**: Automatically selected when `BUCKETEER_BUCKET_NAME` environment variable is set (provisioned via Heroku add-on).

**Benefits**:
- ✅ Managed S3 buckets via Heroku add-on
- ✅ Automatic credential management
- ✅ One-click credential rotation
- ✅ Consolidated billing through Heroku
- ✅ No manual AWS account setup
- ✅ Persistent storage (survives dyno restarts)

**Installation**:
```bash
# Install Bucketeer add-on
heroku addons:create bucketeer:hobbyist  # ~$0.007/hour, max $5/month
```

**Configuration**: Bucketeer automatically sets:
- `BUCKETEER_BUCKET_NAME` - S3 bucket name
- `BUCKETEER_AWS_ACCESS_KEY_ID` - AWS access key
- `BUCKETEER_AWS_SECRET_ACCESS_KEY` - AWS secret key
- `BUCKETEER_AWS_REGION` - AWS region

**Note**: No additional configuration needed! The app automatically detects and uses Bucketeer.

See [Bucketeer documentation](https://elements.heroku.com/addons/bucketeer) for pricing and plans.

### 2. Direct AWS S3 (Production/Heroku)

**When Used**: Automatically selected when `AWS_S3_BUCKET` environment variable or `AWS:S3:Bucket` configuration is set (and Bucketeer is not configured).

**Benefits**:
- Persistent storage (survives dyno restarts)
- Scalable and reliable
- Suitable for production deployments
- Full control over AWS account

**Configuration**:

```bash
# Required
export AWS_S3_BUCKET=your-bucket-name

# AWS Credentials (use IAM roles on EC2, or access keys)
export AWS_ACCESS_KEY_ID=your-access-key
export AWS_SECRET_ACCESS_KEY=your-secret-key

# Optional
export AWS_REGION=us-west-2  # Defaults to us-east-1
export AWS_S3_PREFIX=4kampf/projects  # Defaults to "4kampf/projects"
```

Or via `appsettings.json`:

```json
{
  "AWS": {
    "S3": {
      "Bucket": "your-bucket-name",
      "Prefix": "4kampf/projects"
    }
  }
}
```

**Heroku Configuration**:

**Option A: Use Bucketeer (Recommended)**:
```bash
heroku addons:create bucketeer:hobbyist
# Bucketeer automatically sets all required environment variables
```

**Option B: Direct AWS S3**:
```bash
heroku config:set AWS_S3_BUCKET=your-bucket-name
heroku config:set AWS_ACCESS_KEY_ID=your-access-key
heroku config:set AWS_SECRET_ACCESS_KEY=your-secret-key
heroku config:set AWS_REGION=us-west-2
heroku config:set AWS_S3_PREFIX=4kampf/projects
```

### 2. Local File System (Development)

**When Used**: Automatically selected when S3 is not configured.

**Benefits**:
- No external dependencies
- Fast for local development
- Easy to debug

**Configuration**:

```bash
# Optional: Custom directory (defaults to wwwroot/projects)
export PROJECTS_DIRECTORY=/path/to/projects
```

**Default Locations**:
- **Development**: `wwwroot/projects/`
- **Fallback**: System temp directory if default fails

## Automatic Selection

The application automatically selects the storage backend in this order:

1. **Checks for Bucketeer** (Heroku add-on):
   - Environment variable: `BUCKETEER_BUCKET_NAME`
   - If found: Uses `S3StorageService` with Bucketeer credentials
   
2. **Checks for direct S3 configuration**:
   - Environment variable: `AWS_S3_BUCKET`
   - Configuration: `AWS:S3:Bucket`
   - If found: Uses `S3StorageService` with direct AWS credentials
   
3. **Falls back to local storage**:
   - Uses `LocalStorageService` for development

**Priority**: Bucketeer > Direct S3 > Local Storage

No code changes needed - just configure the appropriate environment variables or provision the Bucketeer add-on.

## Storage Interface

Both storage backends implement `IStorageService`:

```csharp
public interface IStorageService
{
    Task<bool> SaveTextAsync(string path, string content);
    Task<string?> LoadTextAsync(string path);
    Task<bool> FileExistsAsync(string path);
    Task<IEnumerable<string>> ListFilesAsync(string directory, string pattern);
    Task<bool> DeleteFileAsync(string path);
    string StorageType { get; }
}
```

## File Path Structure

Both storage backends use the same logical path structure:

```
{projectName}/{projectName}.json          # Project file
{projectName}/song.yml                     # Sointu song file
{projectName}/music.wav                    # Rendered audio
{projectName}/envelope-0.dat               # Envelope data
{projectName}/envelope-1.dat
...
```

**S3**: Files are stored with prefix: `{AWS_S3_PREFIX}/{projectName}/...`
**Local**: Files are stored at: `{PROJECTS_DIRECTORY}/{projectName}/...`

## Migration

### From Local to S3

1. Configure S3 (see above)
2. Restart application
3. Files will automatically use S3 going forward
4. **Note**: Existing local files are not automatically migrated

### From S3 to Local

1. Remove S3 configuration variables
2. Restart application
3. Files will automatically use local storage going forward
4. **Note**: Existing S3 files are not automatically downloaded

## Troubleshooting

### S3 Not Working

**Check**:
1. `AWS_S3_BUCKET` is set
2. AWS credentials are valid
3. IAM permissions allow S3 access
4. Bucket exists and is accessible

**Logs**: Check application logs for storage type:
```
ProjectFileService initialized with storage: AWS S3
```

### Local Storage Not Working

**Check**:
1. Directory permissions
2. Disk space
3. Path configuration

**Logs**: Check application logs:
```
Local storage initialized: /path/to/projects
```

### Files Not Persisting on Heroku

**Solution**: Configure S3 storage. Local file system on Heroku is ephemeral.

## Best Practices

1. **Development**: Use local file system (default)
2. **Production/Heroku**: Use S3 for persistence
3. **Testing**: Can use either, but S3 requires AWS credentials
4. **Backup**: S3 provides built-in durability; local files should be backed up

## Cost Considerations

### AWS S3

- **Storage**: ~$0.023 per GB/month
- **Requests**: 
  - PUT: $0.005 per 1,000 requests
  - GET: $0.0004 per 1,000 requests
- **Data Transfer**: Free within same region

For typical usage (small project files), costs are minimal (< $1/month).

### Local Storage

- **Cost**: Free (uses server disk space)
- **Limitation**: Ephemeral on Heroku

