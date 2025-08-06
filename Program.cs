using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
#else
builder.Services.AddControllersWithViews();
#endif

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
