using FakeReddit.Data;
using Microsoft.EntityFrameworkCore;
using FakeReddit.Areas.Identity.Data;
using FakeReddit.Models;
using FakeReddit.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbContext") ?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

builder.Services.AddDbContext<FakeRedditIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FakeRedditIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FakeRedditIdentityDbContextConnection' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FakeRedditIdentityDbContext>();

builder.Services.AddScoped<SignInManager<ApplicationUser>, MyIdentity>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    var admin = new User(builder.Configuration["Admin:email"]  ?? throw new InvalidOperationException(), 
        builder.Configuration["Admin:password"]  ?? throw new InvalidOperationException(),
        builder.Configuration["Admin:role"]  ?? throw new InvalidOperationException()
        );
    
    var user = new User(builder.Configuration["User:email"]  ?? throw new InvalidOperationException(), 
        builder.Configuration["User:password"]  ?? throw new InvalidOperationException(),
        builder.Configuration["User:role"]  ?? throw new InvalidOperationException()
    );

    
    await SeedData.Initialize(scope.ServiceProvider, new User[] {admin, user});


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
    pattern: "{controller=Main}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();