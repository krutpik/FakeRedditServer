using FakeReddit.Data;
using Microsoft.EntityFrameworkCore;
using FakeReddit.Areas.Identity.Data;
using FakeReddit.Models;
using FakeReddit.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbContext") ?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

builder.Services.AddDbContext<FakeRedditIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FakeRedditIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FakeRedditIdentityDbContextConnection' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FakeRedditIdentityDbContext>();
    

builder.Services.AddScoped<SignInManager<IdentityUser>, MyIdentity>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var configurationAdmin = builder.Configuration.GetSection("Admin");
    await SeedData.Initialize(scope.ServiceProvider, 
        configurationAdmin.GetValue<string>("email"),
        configurationAdmin.GetValue<string>("password"), configurationAdmin.GetValue<string>("role"));
    
}

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Main}/{action=Index}/{theme?}");

app.MapRazorPages();

app.Run();