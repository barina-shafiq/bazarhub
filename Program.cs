using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.Repositories;
using WebProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddControllersWithViews();
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

if (args.Contains("--seed-fashion", StringComparer.OrdinalIgnoreCase))
{
    var csvPath = args.SkipWhile(a => !string.Equals(a, "--seed-fashion", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault()
        ?? throw new InvalidOperationException("Provide the CSV path after --seed-fashion.");
    var requestedCount = args.SkipWhile(a => !string.Equals(a, "--count", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
    var count = int.TryParse(requestedCount, out var parsedCount) ? parsedCount : 500;
    var imported = await FashionCsvSeeder.SeedAsync(app.Services, csvPath, count);
    Console.WriteLine($"Imported {imported} fashion products from {csvPath}.");
    return;
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

var startupLog = new System.Text.StringBuilder();
startupLog.AppendLine($"[Startup Log - {DateTime.UtcNow} UTC]");
startupLog.AppendLine($"Environment: {app.Environment.EnvironmentName}");
startupLog.AppendLine($"ContentRootPath: {app.Environment.ContentRootPath}");
startupLog.AppendLine($"BaseDirectory: {AppContext.BaseDirectory}");

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    var censoredConn = System.Text.RegularExpressions.Regex.Replace(connectionString, @"Password=[^;]+", "Password=********");
    startupLog.AppendLine($"Connection String: {censoredConn}");
}
else
{
    startupLog.AppendLine("Connection String 'DefaultConnection' is null or empty!");
}

if (app.Configuration.GetValue("AppSettings:AutoMigrateDatabase", false))
{
    try
    {
        startupLog.AppendLine("AutoMigrateDatabase is enabled. Applying migrations...");
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        startupLog.AppendLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        startupLog.AppendLine($"Migration Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            startupLog.AppendLine($"Migration Inner Exception: {ex.InnerException.Message}");
        }
        startupLog.AppendLine(ex.StackTrace);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations on startup.");
    }
}
else
{
    startupLog.AppendLine("AutoMigrateDatabase is disabled.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    if (app.Configuration.GetValue("AppSettings:UseHttpsRedirection", false))
    {
        app.UseHsts();
    }
}

if (app.Configuration.GetValue("AppSettings:UseHttpsRedirection", false))
{
    app.UseHttpsRedirection();
}

// UseStaticFiles MUST come before UseRouting to serve runtime-uploaded files
// (e.g. product images saved to wwwroot/uploads).
// app.MapStaticAssets() only serves build-time fingerprinted assets.
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

if (app.Configuration.GetValue("AppSettings:SeedAdminOnStartup", true))
{
    try
    {
        startupLog.AppendLine("SeedAdminOnStartup is enabled. Seeding admin...");
        await IdentitySeeder.SeedAsync(app.Services);
        startupLog.AppendLine("Admin user and roles verified/seeded successfully.");
    }
    catch (Exception ex)
    {
        startupLog.AppendLine($"Admin Seeding Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            startupLog.AppendLine($"Admin Seeding Inner Exception: {ex.InnerException.Message}");
        }
        startupLog.AppendLine(ex.StackTrace);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the admin user on startup.");
    }
}
else
{
    startupLog.AppendLine("SeedAdminOnStartup is disabled.");
}

if (app.Configuration.GetValue("AppSettings:SeedFashionOnStartup", true))
{
    try
    {
        startupLog.AppendLine("SeedFashionOnStartup is enabled. Resolving fashion.csv...");
        var csvPath = FashionCsvSeeder.ResolveCsvPath(app.Environment);
        startupLog.AppendLine($"fashion.csv resolved to '{csvPath}'. Starting import...");
        var seededCount = await FashionCsvSeeder.SeedAsync(app.Services, csvPath, 500);
        startupLog.AppendLine($"Fashion seeding complete. Seeded {seededCount} new products.");
    }
    catch (Exception ex)
    {
        startupLog.AppendLine($"Fashion Seeding Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            startupLog.AppendLine($"Fashion Seeding Inner Exception: {ex.InnerException.Message}");
        }
        startupLog.AppendLine(ex.StackTrace);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding fashion products on startup.");
    }
}
else
{
    startupLog.AppendLine("SeedFashionOnStartup is disabled.");
}

// Write the startup log to wwwroot/startup-log.txt
try
{
    var logDirectory = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }
    var logPath = Path.Combine(logDirectory, "startup-log.txt");
    await File.WriteAllTextAsync(logPath, startupLog.ToString());
}
catch (Exception logEx)
{
    Console.WriteLine($"Could not write startup log: {logEx.Message}");
}

app.Run();
