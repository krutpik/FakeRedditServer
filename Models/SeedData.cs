using Microsoft.AspNetCore.Identity;


namespace FakeReddit.Models;

public class SeedData
{
    private static RoleManager<IdentityRole>? _roleManager;
    private static UserManager<ApplicationUser>? _userManager;

    public static async Task Initialize(IServiceProvider serviceProvider, string email, string password, string role)
    {
        _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        await CreateAccount(email, password, role);

    }

    
    public static async Task CreateAccount(string email, string password, string role)
    {
        if (_userManager == null) throw new InvalidOperationException();
        
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
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

        if (_roleManager == null) throw new InvalidOperationException();

        if (await _roleManager.FindByNameAsync(role) == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));

        }
        await _userManager.AddToRoleAsync(user, role);
    }
        
        
}