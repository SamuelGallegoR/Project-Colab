using Lab1.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lab1.Services;
using Lab1.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Serilog;
// Ensure necessary namespaces are included

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the DbContext to use MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


// Configure Identity with the correct approach
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

/*
// Configure Identity with the correct approach
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
*/



// This ensures that whenever an IEmail sender is injected, an instance of EmailSender is provided
builder.Services.AddSingleton<IEmailSender, EmailSender>();


//Configure logger - Set up Serilog as loggin provider
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    //Configure Serilog to read from the apps settings (appsettings.json)
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Home/NotFound?statusCode={0}");
}

using var scope = app.Services.CreateScope();
var loggerFactory= scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

try
{

    //Get servides needed for role seeding
    // scope.ServiceProvider - used to access instances of registered services
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //Seed roles
    await ContextSeed.SeedRolesAsync(userManager, roleManager);

    //Seed superAdmin
    await ContextSeed.SuperSeedRoleAsync(userManager, roleManager);

}
catch(Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occured when attempting to seed the roles for the system.");

}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Make sure to call this before UseAuthorization
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Project}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
