using Audit.Core;
using Audit.EntityFramework;

using FustWebApp.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString).EnableSensitiveDataLogging(true));




builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();





var app = builder.Build();



Audit.Core.Configuration.Setup().UseFileLogProvider(_ => _

.Directory(@"./Data/Logs")
);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(

    name: "areaRoute",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();