using PomodoroApi.Data;

namespace PomodoroApi.Services.Users;

public interface IUserService
{
    Task<ApplicationUser?> GetCurrentUser();
}