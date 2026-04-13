using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Composition;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Infrastructure.Email.AzureEmail;
using MomiaTrainSync.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CONFIGURATIONS (Application Settings)
// ======================================================

DotNetEnv.Env.Load();

builder.Configuration.AddEnvironmentVariables();

// Azure Email ConnectionString
var azureConnection =
    builder.Configuration["MomiaTrainSync_AzureEmail_ConnectionString"]
    ?? builder.Configuration["AzureEmail:ConnectionString"];

if (string.IsNullOrWhiteSpace(azureConnection))
    throw new InvalidOperationException(
        "AzureEmail:ConnectionString is not configured.");

builder.Configuration["AzureEmail:ConnectionString"] = azureConnection;

var azureFrom =
    builder.Configuration["MomiaTrainSync_AzureEmail_From"]
    ?? builder.Configuration["AzureEmail:From"];

if (string.IsNullOrWhiteSpace(azureFrom))
    throw new InvalidOperationException(
        "AzureEmail:From is not configured.");

builder.Configuration["AzureEmail:From"] = azureFrom;

// Bind Settings
builder.Services.Configure<AzureEmailOptions>(
    builder.Configuration.GetSection("AzureEmail"));


// ======================================================
// SERVICES
// ======================================================

// Dependency Injection (Composition Root)
builder.Services.AddMomiaTrainSyncServices(builder.Configuration);

// Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.LogoutPath = "/Authentication/Logout";
        options.AccessDeniedPath = "/Error/403";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// MVC
builder.Services.AddControllersWithViews();


// ======================================================
// BUILD APP
// ======================================================

var app = builder.Build();


// ======================================================
// SEED DATA & MIGRATIONS
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var db = provider.GetRequiredService<MomiaTrainSyncDbContext>();

    try
    {
        await db.Database.MigrateAsync();
        // Roles iniciales
        await SeedData.EnsureRolesAsync(provider);
        // Permisos iniciales
        await SeedData.EnsurePermissionsAsync(provider);
        // Relación Roles-Permisos iniciales
        await SeedData.EnsureRolePermissionsAsync(provider);
        // Usuario Inicial (si no existe)
        await SeedData.EnsureUsuariosAsync(provider);
        // Zonas iniciales
        await SeedData.EnsureZonasAsync(provider);
        // Tipos de sesión iniciales
        await SeedData.EnsureTipoSesionAsync(provider);

    }
    catch (Exception ex)
    {
        var scopedLogger =
            provider.GetRequiredService<ILogger<Program>>();

        scopedLogger.LogError(
            ex,
            "Error applying migrations or seeding database.");
    }
}


// ======================================================
// MIDDLEWARE PIPELINE
// ======================================================

if (!app.Environment.IsDevelopment())
{
    // Producción
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // Desarrollo
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}");


// ======================================================
// ENDPOINTS
// ======================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ======================================================
// RUN APP
// ======================================================

app.Run();