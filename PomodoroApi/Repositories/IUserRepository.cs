using PomodoroApi.Data;

namespace PomodoroApi.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
}