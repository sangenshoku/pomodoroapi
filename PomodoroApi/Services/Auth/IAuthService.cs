using Microsoft.AspNetCore.Identity;
using PomodoroApi.Data;

namespace PomodoroApi.Services.Auth;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(string email, string password);
    Task<SignInResult> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<ApplicationUser?> GetCurrentUserAsync();
}