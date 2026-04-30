using LeaveManagementSystem.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IOC container 

// Configuration -> materialized version of the appsettings.json
// connect the App to a Database
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

// add ApplicationDbContext as a Database Context -> main class for talking to the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
// use SQL Server and pass in a connection string to connect
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register Service Layer in the IoC container so it can be injected into other classes
// ILeaveTypeServices -> contract (interface)
// LeaveTypeServices -> implementation (concrete class)
// AddScoped -> one instance per HTTP request
// The same instance is reused throughout the entire request
// (e.g., one DB context per request)
builder.Services.AddScoped<ILeaveTypeServices, LeaveTypeServices>();

// AddTransient -> new instance every time it is requested
// A new object is created each time it is injected (lightweight, stateless services)
builder.Services.AddTransient<IEmailSender, EmailSender>();

//Add AutoMapper to IOC Container
builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

// Configure ASP.NET Identity system:
// - Use ApplicationUser as the user entity (custom user class)
// - Require users to confirm their email before they can sign in
// - Enable role management using IdentityRole (e.g., Employee, Supervisor, Admin)
// - Use Entity Framework Core (ApplicationDbContext) to store users, roles,
//   and related data in the database
builder.Services.AddDefaultIdentity<ApplicationUser>
    (options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Build the application (creates the app object using configured services)
var app = builder.Build();

// MIDDLEWARES

// Configure the HTTP request pipeline (middleware runs on every request)
if (app.Environment.IsDevelopment())
{
    // Enables detailed database error page (useful during development)
    app.UseMigrationsEndPoint();
}
else
{
    // Global error handler (redirects to /Home/Error in production)
    app.UseExceptionHandler("/Home/Error");

    // Adds HTTP Strict Transport Security (forces HTTPS for security)
    app.UseHsts();
}

// Redirect all HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enables serving static files (CSS, JS, images from wwwroot)
app.UseStaticFiles();

// Enables routing (figures out which endpoint should handle the request)
app.UseRouting();

// Enables authorization (checks if user has access to resources)
// ⚠️ Requires authentication middleware if you want login-based protection
app.UseAuthorization();

// Maps MVC controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Default route:
// / → HomeController → Index()
// /LeaveTypes → LeaveTypesController → Index()

// Enables Razor Pages (used by Identity pages like Login/Register)
app.MapRazorPages();

// Starts the application and begins listening for requests
app.Run();
