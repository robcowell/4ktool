using Amazon.S3;
using _4kampf.Web.Components;
using _4kampf.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use PORT environment variable (Heroku requirement)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Enable detailed errors for development
        if (builder.Environment.IsDevelopment())
        {
            options.DetailedErrors = true;
        }
    });

// Register storage service (S3 for Heroku, local for development)
// Check for Bucketeer (Heroku add-on) first, then direct AWS S3, then local
var useBucketeer = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUCKETEER_BUCKET_NAME"));
var useS3 = useBucketeer ||
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_S3_BUCKET")) ||
            !string.IsNullOrEmpty(builder.Configuration["AWS:S3:Bucket"]);

if (useS3)
{
    // Configure AWS S3
    if (useBucketeer)
    {
        // Bucketeer provides its own credentials via environment variables
        // Map Bucketeer env vars to standard AWS env vars for AWS SDK
        var bucketeerAccessKey = Environment.GetEnvironmentVariable("BUCKETEER_AWS_ACCESS_KEY_ID");
        var bucketeerSecretKey = Environment.GetEnvironmentVariable("BUCKETEER_AWS_SECRET_ACCESS_KEY");
        var bucketeerRegion = Environment.GetEnvironmentVariable("BUCKETEER_AWS_REGION");
        
        // Temporarily set standard AWS env vars for AWS SDK initialization
        if (!string.IsNullOrEmpty(bucketeerAccessKey))
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", bucketeerAccessKey);
        }
        if (!string.IsNullOrEmpty(bucketeerSecretKey))
        {
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", bucketeerSecretKey);
        }
        if (!string.IsNullOrEmpty(bucketeerRegion))
        {
            Environment.SetEnvironmentVariable("AWS_REGION", bucketeerRegion);
        }
    }
    
    // Configure AWS SDK (will use environment variables or appsettings)
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
    builder.Services.AddAWSService<IAmazonS3>();
    builder.Services.AddSingleton<IStorageService, S3StorageService>();
}
else
{
    // Use local file system
    builder.Services.AddSingleton<IStorageService, LocalStorageService>();
}

// Register custom services
builder.Services.AddScoped<WebGLService>();
builder.Services.AddScoped<WebAudioService>();
builder.Services.AddScoped<CodeEditorService>();
builder.Services.AddScoped<CameraService>();
builder.Services.AddSingleton<ProjectService>();
builder.Services.AddSingleton<SointuService>();
builder.Services.AddScoped<MusicEnvelopeService>();
builder.Services.AddSingleton<ProjectFileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Heroku handles HTTPS at the load balancer, so only redirect in non-Heroku environments
// Check for Heroku by looking for PORT env var (Heroku always sets this)
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT")))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map API controllers for testing
app.MapControllers();

app.Run();
