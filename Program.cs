using TaskManager.Models;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- Logging -------------------------------------------------------------
// Console logging is what Azure App Service "Log stream" and
// "App Service logs" (Application Logging) pick up in real time.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ---- Configuration (Task 2.3) --------------------------------------------
// Values below come from appsettings.json locally, and are OVERRIDDEN by
// Azure App Service > Configuration > Application settings when deployed.
// Nothing sensitive is hardcoded in source.
builder.Services.Configure<TaskManagerOptions>(
    builder.Configuration.GetSection(TaskManagerOptions.SectionName));

builder.Services.AddRazorPages();
builder.Services.AddSingleton<TaskService>();

// ---- Health checks (Task 2.3) ---------------------------------------------
builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Lightweight endpoint Azure (or any uptime monitor) can poll to confirm
// the app is alive. Visit /health directly to see it.
app.MapHealthChecks("/health");

app.MapRazorPages();

app.Logger.LogInformation("Task Manager application starting up");

app.Run();
