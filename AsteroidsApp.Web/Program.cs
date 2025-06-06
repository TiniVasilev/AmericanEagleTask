var builder = WebApplication.CreateBuilder(args);

// Dependency Injection registrations
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<AsteroidsApp.Application.Interfaces.INasaApiService, AsteroidsApp.Infrastructure.Services.NasaApiService>();
builder.Services.AddScoped<AsteroidsApp.Application.Interfaces.IExcelExportService, AsteroidsApp.Infrastructure.Services.ExcelExportService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
