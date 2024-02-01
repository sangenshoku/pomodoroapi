namespace PomodoroApi.Contracts.Auth;
public record CreateUserRequest(
    string Email,
    string Password);