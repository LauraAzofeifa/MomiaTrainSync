using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
#if DEBUG
    .AddRazorRuntimeCompilation()
#endif
    ;

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";

    var rutasPublicas = new[]
    {
        "/Account/Login",
        "/Account/Register",
        "/Account/ForgotPassword",
        "/Home/Index"
    };

    var usuarioAutenticado = context.Session.Keys.Contains("UsuarioId");

    if (!usuarioAutenticado && !rutasPublicas.Any(r => path.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
