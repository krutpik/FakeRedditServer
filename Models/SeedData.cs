using Microsoft.AspNetCore.Identity;
using FakeReddit.Models;

namespace FakeReddit.Models;

public class SeedData
{
    private static RoleManager<IdentityRole>? _roleManager;
    private static UserManager<ApplicationUser>? _userManager;

    public static async Task Initialize(IServiceProvider serviceProvider, IEnumerable<User> user)
    {
        _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
        foreach (var i in user)
        {
            await CreateAccount(i.Email, i.Password, i.Role);
        }
    }


    private static async Task CreateAccount(string email, string password, string role)
    {
        if (_userManager == null) throw new InvalidOperationException();
        
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {   
                Email = email,
                UserName = email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Errors.Any())
            {
                throw new Exception("The admin password was probably not strong enough!");
            }
        }

        if (_roleManager == null) throw new InvalidOperationException();

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));

        }
        await _userManager.AddToRoleAsync(user, role);
    }
        
        
}