using FakeReddit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FakeReddit.Services;

public class MyIdentity : SignInManager<ApplicationUser>

{
    public MyIdentity(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
    
    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password,
        bool isPersistent, bool lockoutOnFailure)
    {
        var user = await UserManager.FindByNameAsync(userName);
        if (user != null) return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
       
        user = await UserManager.FindByEmailAsync(userName);
        if (user != null) return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
       
        return SignInResult.Failed;
        
    }
}