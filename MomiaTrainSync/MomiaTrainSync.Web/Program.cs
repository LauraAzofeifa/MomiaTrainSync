using Microsoft.AspNetCore.Authentication.Cookies;
using MomiaTrainSync.Composition;
using MomiaTrainSync.Core.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMomiaTrainSyncServices(builder.Configuration);

// Configuracion de EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.LogoutPath = "/Authentication/Logout";
        options.AccessDeniedPath = "/Error/403";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Captura errores globales (500, excepciones no controladas)
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}
else
{
    // En desarrollo muestra la página detallada de error
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
