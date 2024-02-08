using Microsoft.AspNetCore.Identity;
using PomodoroApi.Data;

namespace PomodoroApi.Services.Users;

public class UserService(IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<ApplicationUser?> GetCurrentUser()
    {
        var userClaimsPrincipal = httpContext.HttpContext?.User;
        ApplicationUser? user = null;

        if (userClaimsPrincipal is not null)
        {
            user = await userManager.GetUserAsync(userClaimsPrincipal);
        }

        return user;
    }
}