using Microsoft.AspNetCore.Identity;
using PomodoroApi.Contracts.Auth;
using PomodoroApi.Data;

namespace PomodoroApi.Services.Auth;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterRequest request);
    Task<SignInResult> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<ApplicationUser?> GetCurrentUserAsync();
}