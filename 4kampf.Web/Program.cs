using _4kampf.Web.Components;
using _4kampf.Web.Services;

var builder = WebApplication.CreateBuilder(args);

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
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
