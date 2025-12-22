using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

namespace _4kampf.Web.Services;

/// <summary>
/// AWS S3 storage implementation.
/// Used for Heroku deployments where local file system is ephemeral.
/// </summary>
public class S3StorageService : IStorageService
{
    private readonly ILogger<S3StorageService>? _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _basePrefix;

    public string StorageType { get; private set; } = "AWS S3";

    public S3StorageService(
        IAmazonS3 s3Client,
        IConfiguration configuration,
        ILogger<S3StorageService>? logger = null)
    {
        _logger = logger;
        _s3Client = s3Client;
        
        // Check for Bucketeer (Heroku add-on) first, then direct AWS S3
        var bucketeerBucket = Environment.GetEnvironmentVariable("BUCKETEER_BUCKET_NAME");
        if (!string.IsNullOrEmpty(bucketeerBucket))
        {
            _bucketName = bucketeerBucket;
            StorageType = "Bucketeer (Managed S3)";
            _logger?.LogInformation("Using Bucketeer managed S3 bucket");
        }
        else
        {
            // Get bucket name from configuration
            _bucketName = configuration["AWS:S3:Bucket"] 
                ?? Environment.GetEnvironmentVariable("AWS_S3_BUCKET") 
                ?? throw new InvalidOperationException("AWS S3 bucket name not configured. Set AWS:S3:Bucket, AWS_S3_BUCKET, or provision Bucketeer add-on.");
            StorageType = "AWS S3";
        }
        
        // Optional prefix for organizing files
        _basePrefix = configuration["AWS:S3:Prefix"] 
            ?? Environment.GetEnvironmentVariable("AWS_S3_PREFIX") 
            ?? "4kampf/projects";
        
        // Ensure prefix doesn't end with /
        if (_basePrefix.EndsWith("/"))
        {
            _basePrefix = _basePrefix.TrimEnd('/');
        }
        
        _logger?.LogInformation("S3 storage initialized: Type={Type}, Bucket={Bucket}, Prefix={Prefix}", StorageType, _bucketName, _basePrefix);
    }

    public async Task<bool> SaveTextAsync(string path, string content)
    {
        try
        {
            string key = GetS3Key(path);
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                ContentBody = content,
                ContentType = GetContentType(path)
            };
            
            await _s3Client.PutObjectAsync(request);
            _logger?.LogDebug("Saved to S3: {Key}", key);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error saving to S3: {Path}", path);
            return false;
        }
    }

    public async Task<string?> LoadTextAsync(string path)
    {
        try
        {
            string key = GetS3Key(path);
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            
            using var response = await _s3Client.GetObjectAsync(request);
            using var reader = new StreamReader(response.ResponseStream);
            string content = await reader.ReadToEndAsync();
            
            _logger?.LogDebug("Loaded from S3: {Key}", key);
            return content;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger?.LogDebug("File not found in S3: {Path}", path);
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading from S3: {Path}", path);
            return null;
        }
    }

    public async Task<bool> FileExistsAsync(string path)
    {
        try
        {
            string key = GetS3Key(path);
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            
            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error checking file existence in S3: {Path}", path);
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListFilesAsync(string directory, string pattern)
    {
        try
        {
            string prefix = GetS3Key(directory);
            if (!prefix.EndsWith("/"))
            {
                prefix += "/";
            }
            
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };
            
            var files = new List<string>();
            ListObjectsV2Response response;
            
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);
                foreach (var obj in response.S3Objects)
                {
                    string fileName = Path.GetFileName(obj.Key);
                    if (MatchesPattern(fileName, pattern))
                    {
                        files.Add(fileName);
                    }
                }
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            
            return files;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error listing files in S3: {Directory}", directory);
            return Enumerable.Empty<string>();
        }
    }

    public async Task<bool> DeleteFileAsync(string path)
    {
        try
        {
            string key = GetS3Key(path);
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            
            await _s3Client.DeleteObjectAsync(request);
            _logger?.LogDebug("Deleted from S3: {Key}", key);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error deleting from S3: {Path}", path);
            return false;
        }
    }

    private string GetS3Key(string path)
    {
        // Normalize path separators
        string normalizedPath = path.Replace('\\', '/');
        
        // Remove leading / if present
        if (normalizedPath.StartsWith("/"))
        {
            normalizedPath = normalizedPath.Substring(1);
        }
        
        // Combine with base prefix
        return $"{_basePrefix}/{normalizedPath}";
    }

    private string GetContentType(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();
        return extension switch
        {
            ".json" => "application/json",
            ".yml" or ".yaml" => "text/yaml",
            ".dat" => "application/octet-stream",
            ".wav" => "audio/wav",
            ".asm" => "text/plain",
            _ => "text/plain"
        };
    }

    private bool MatchesPattern(string fileName, string pattern)
    {
        // Simple wildcard matching (e.g., "*.json")
        if (pattern.StartsWith("*."))
        {
            string extension = pattern.Substring(1);
            return fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        }
        
        // Exact match
        return fileName.Equals(pattern, StringComparison.OrdinalIgnoreCase);
    }
}

