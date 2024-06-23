using FakeReddit.Data;
using FakeReddit.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Models;

public class SeedData
{
    private static RoleManager<IdentityRole>? _roleManager;
    private static UserManager<IdentityUser>? _userManager;

    public static async Task Initialize(IServiceProvider serviceProvider, string? email, string? password, string? role)
    {
        _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        await CreateAccount(email, password, role);

    }

    
    public static async Task CreateAccount(string? email, string? password, string? role)
        {
            IdentityUser? user = await _userManager?.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser()
                {   
                    Email = email,
                    UserName = email,
                };
                var result = await _userManager.CreateAsync(user, password);
                
                if (result.Errors.Any())
                {
                    throw new Exception("The admin password was probably not strong enough!");
                }
            }
    
    
            if (await _roleManager?.FindByNameAsync(role) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
    
            }
            await _userManager.AddToRoleAsync(user, role);
        }
        
        
}