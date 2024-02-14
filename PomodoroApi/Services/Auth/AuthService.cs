using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PomodoroApi.Contracts.Auth;
using PomodoroApi.Data;

namespace PomodoroApi.Services.Auth;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;

        if (user == null)
            return null;

        return await userManager.GetUserAsync(user);
    }

    public async Task<SignInResult> LoginAsync(string email, string password)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await signInManager.PasswordSignInAsync(user, password, false, false);
    }


    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match" });
        }

        return await userManager.CreateAsync(new ApplicationUser { UserName = request.Email, Email = request.Email }, request.Password);
    }
}